import asyncio
import json
import logging
from typing import Dict, Any
from dataclasses import asdict

from ..core.MainArchitecture import DigitalArchitect
from ..chat.ConversationHandler import ConversationHandler
from .LLMSystem import MessageHandler, ArchitectRequest
from ..core.websocketManager import WebSocketClientManager
from ..config.ConfigManager import ConfigManager

class DigitalArchitectServer:
    def __init__(self, config: ConfigManager = None):
        self.config = config or ConfigManager()
        self.port = self.config.get_environment_config().default_port
        
        # Initialize WebSocket Manager
        self.ws_manager = WebSocketClientManager(
            uri=f"ws://localhost:{self.port}/unity",
            api_key=self.config.get_websocket_config().api_key
        )
        
        # Initialize core components
        self.architect = DigitalArchitect()
        self.conversation_handler = ConversationHandler(MessageHandler())
        
        # Setup logging
        self.logger = logging.getLogger(__name__)
        
        # Message handlers
        self.message_handlers = {
            "architect_request": self._handle_architect_request,
            "environment_update": self._handle_environment_update,
            "state_sync": self._handle_state_sync
        }

    async def start(self):
        """Start the server and initialize WebSocket connection"""
        try:
            # Setup WebSocket connection
            await self.ws_manager.connect()
            
            # Start message monitoring
            await self.ws_manager.start()
            
            # Add message handler
            self.ws_manager.on_message_received = self.handle_message
            
            self.logger.info(f"Digital Architect Server started and connected to Unity")
            
            # Keep server running
            while True:
                await asyncio.sleep(1)
                
        except Exception as e:
            self.logger.error(f"Server error: {e}")
            raise

    async def handle_message(self, message_data: Dict[str, Any]) -> Dict[str, Any]:
        """Route incoming messages to appropriate handlers"""
        try:
            message_type = message_data.get("type")
            if message_type in self.message_handlers:
                return await self.message_handlers[message_type](message_data)
            else:
                return {"type": "error", "message": f"Unknown message type: {message_type}"}
        except Exception as e:
            self.logger.error(f"Error handling message: {e}")
            return {"type": "error", "message": str(e)}

    async def _handle_architect_request(self, data: Dict[str, Any]) -> Dict[str, Any]:
        """Handle requests for architect actions"""
        message = data.get("message")
        project_context = data.get("metadata", {}).get("project_context", {})
        
        # Process through conversation handler
        conversation_response = await self.conversation_handler.process_request(
            message, project_context
        )
        
        if conversation_response.is_valid:
            return await self.architect.handle_request(message, project_context)
        
        return {"type": "error", "message": "Invalid request"}

    async def _handle_environment_update(self, data: Dict[str, Any]) -> Dict[str, Any]:
        """Handle environment state updates from Unity"""
        # Process environment updates
        return {"type": "acknowledgment", "status": "processed"}

    async def _handle_state_sync(self, data: Dict[str, Any]) -> Dict[str, Any]:
        """Handle state synchronization requests"""
        # Process state sync
        return {"type": "state_sync", "state": self.architect.get_state()}

    async def cleanup(self):
        """Cleanup resources"""
        await self.ws_manager.disconnect()

if __name__ == "__main__":
    server = DigitalArchitectServer()
    try:
        asyncio.run(server.start())
    except KeyboardInterrupt:
        print("\nShutting down server...")
        asyncio.run(server.cleanup())