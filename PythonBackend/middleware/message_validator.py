from enum import Enum
from typing import Dict, Any, Optional
from dataclasses import dataclass
from datetime import datetime

class MessageCategory(Enum):
    """Core message categories"""
    ARCHITECT = "architect"      # Architect-related communication
    SYSTEM = "system"           # System-level messages
    ENVIRONMENT = "environment"  # Environment state updates
    UI = "ui"                   # UI-related messages

@dataclass
class ValidationResult:
    is_valid: bool
    error_message: Optional[str] = None
    validated_data: Optional[Dict[str, Any]] = None

class MessageValidator:
    """Validates messages between Unity, Python backend, and UI"""
    
    @staticmethod
    def validate_message(message: Dict[str, Any]) -> ValidationResult:
        """Main validation entry point"""
        try:
            # Basic message structure validation
            if not isinstance(message, dict):
                return ValidationResult(False, "Message must be a dictionary")

            # Check required base fields
            if "type" not in message:
                return ValidationResult(False, "Message type not specified")
            if "category" not in message:
                return ValidationResult(False, "Message category not specified")

            # Validate authentication if present
            if "api_key" in message and not MessageValidator._validate_auth(message["api_key"]):
                return ValidationResult(False, "Invalid authentication")

            # Route to category-specific validation
            try:
                category = MessageCategory(message["category"])
                validator_method = getattr(
                    MessageValidator,
                    f"_validate_{category.value}",
                    MessageValidator._validate_default
                )
                return validator_method(message)
            except ValueError:
                return ValidationResult(False, f"Invalid message category: {message['category']}")

        except Exception as e:
            return ValidationResult(False, f"Validation error: {str(e)}")

    @staticmethod
    def _validate_auth(api_key: str) -> bool:
        """Validate API key if provided"""
        # TODO: Implement actual API key validation
        return True

    @staticmethod
    def _validate_architect(message: Dict[str, Any]) -> ValidationResult:
        """Validate architect-related messages"""
        # For requests
        if message["type"] == "request":
            if "message" not in message:
                return ValidationResult(False, "Missing message content")
            if "metadata" not in message:
                return ValidationResult(False, "Missing request metadata")

        # For responses
        elif message["type"] == "response":
            if "status" not in message:
                return ValidationResult(False, "Missing response status")

        # For state updates
        elif message["type"] == "state_update":
            if "state" not in message:
                return ValidationResult(False, "Missing state data")

        return ValidationResult(True, validated_data=message)

    @staticmethod
    def _validate_system(message: Dict[str, Any]) -> ValidationResult:
        """Validate system-level messages"""
        if "action" not in message:
            return ValidationResult(False, "Missing system action")

        # System messages should always have a timestamp
        if "timestamp" not in message:
            return ValidationResult(False, "Missing timestamp")

        return ValidationResult(True, validated_data=message)

    @staticmethod
    def _validate_environment(message: Dict[str, Any]) -> ValidationResult:
        """Validate environment state messages"""
        if "state_type" not in message:
            return ValidationResult(False, "Missing state type")
        if "state_data" not in message:
            return ValidationResult(False, "Missing state data")

        return ValidationResult(True, validated_data=message)

    @staticmethod
    def _validate_ui(message: Dict[str, Any]) -> ValidationResult:
        """Validate UI-related messages"""
        if "action" not in message:
            return ValidationResult(False, "Missing UI action")
        if "data" not in message:
            return ValidationResult(False, "Missing UI data")

        return ValidationResult(True, validated_data=message)

    @staticmethod
    def _validate_default(message: Dict[str, Any]) -> ValidationResult:
        """Default validation for unknown categories"""
        return ValidationResult(True, validated_data=message)

    @staticmethod
    def format_error_response(error_message: str) -> Dict[str, Any]:
        """Format error response"""
        return {
            "category": "system",
            "type": "error",
            "message": error_message,
            "timestamp": datetime.datetime.now().isoformat()
        }