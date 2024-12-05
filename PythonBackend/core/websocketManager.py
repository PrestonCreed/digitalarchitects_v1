import asyncio
import websockets
import json
import logging
from typing import Dict, Any, Optional
from ..utils.logging_config import LoggerMixin
from ..middleware.message_validator import MessageValidator, ValidationResult
from datetime import datetime

class WebSocketClientManager(LoggerMixin):
    """Manages WebSocket connection and communication with Unity."""
    
    def __init__(self, uri: str, api_key: str, autonomous_mode: bool = False):
        self.uri = uri
        self.api_key = api_key
        self.connection = None
        self.message_queue = asyncio.Queue()
        self.action_queue = asyncio.Queue()
        self.reconnect_interval = 5
        self.is_connected = asyncio.Event()
        self.autonomous_mode = autonomous_mode
        self.message_validator = MessageValidator()
        
        # Initialize logger
        self.logger = self.get_logger()
        
    async def start(self):
        """Start the WebSocket manager"""
        await self.connect()
        if self.autonomous_mode:
            asyncio.create_task(self._autonomous_action_processor())
        asyncio.create_task(self._connection_monitor())
        asyncio.create_task(self._process_message_queue())

    async def connect(self):
        """Establish WebSocket connection"""
        try:
            self.connection = await websockets.connect(
                self.uri,
                extra_headers={"X-API-KEY": self.api_key}
            )
            self.is_connected.set()
            self.logger.info(f"Connected to Unity WebSocket server at {self.uri}")
            
            # Send initial handshake
            await self._send_handshake()
            
        except Exception as e:
            self.logger.error(f"Failed to connect to Unity WebSocket server: {e}")
            self.is_connected.clear()
            raise

    async def _send_handshake(self):
        """Send initial handshake message"""
        handshake = {
            "category": "system",
            "type": "handshake",
            "timestamp": datetime.now().isoformat(),
            "api_key": self.api_key
        }
        await self.send_command(handshake)

    async def send_command(self, command: Dict[str, Any]) -> Dict[str, Any]:
        """Send command to Unity with validation"""
        validation_result = self.message_validator.validate_message(command)
        if not validation_result.is_valid:
            self.logger.error(f"Invalid command format: {validation_result.error_message}")
            return {"status": "error", "message": validation_result.error_message}
            
        try:
            if not self.connection or not self.is_connected.is_set():
                await self.connect()
            
            message = json.dumps(command)
            await self.connection.send(message)
            self.logger.debug(f"Sent command: {message}")
            
            response = await self.connection.recv()
            self.logger.debug(f"Received response: {response}")
            
            parsed_response = json.loads(response)
            response_validation = self.message_validator.validate_message(parsed_response)
            if not response_validation.is_valid:
                raise ValueError(f"Invalid response format: {response_validation.error_message}")
                
            return response_validation.validated_data
            
        except Exception as e:
            self.logger.error(f"Error sending command: {e}")
            return {"status": "failure", "error": str(e)}

    async def _connection_monitor(self):
        """Monitor and maintain connection"""
        while True:
            try:
                if not self.connection or not self.is_connected.is_set():
                    await self.connect()
                await asyncio.sleep(self.reconnect_interval)
            except Exception as e:
                self.logger.error(f"Connection monitor error: {e}")
                self.is_connected.clear()
                await asyncio.sleep(self.reconnect_interval)

    async def _process_message_queue(self):
        """Process queued messages"""
        while True:
            message = await self.message_queue.get()
            await self.is_connected.wait()
            
            try:
                await self.send_command(message)
            except Exception as e:
                self.logger.error(f"Failed to send queued message: {e}")
                await self.message_queue.put(message)
                
            self.message_queue.task_done()
            await asyncio.sleep(0.1)

    async def _autonomous_action_processor(self):
        """Process autonomous actions"""
        while True:
            try:
                action = await self.action_queue.get()
                if not self._requires_user_approval(action):
                    await self.send_command(action)
                else:
                    self.logger.warning(f"Action {action['type']} requires user approval")
                await asyncio.sleep(0.1)
            except Exception as e:
                self.logger.error(f"Error in autonomous processing: {e}")

    def _requires_user_approval(self, action: Dict[str, Any]) -> bool:
        """Check if action requires user approval"""
        CRITICAL_ACTIONS = ['delete_environment', 'reset_scene', 'modify_core_settings']
        return action.get('type') in CRITICAL_ACTIONS

    async def disconnect(self):
        """Close WebSocket connection"""
        if self.connection:
            try:
                goodbye = {
                    "category": "system",
                    "type": "disconnect",
                    "timestamp": datetime.now().isoformat()
                }
                await self.send_command(goodbye)
            except:
                pass
            finally:
                await self.connection.close()
                self.is_connected.clear()
                self.logger.info("Disconnected from Unity WebSocket server")