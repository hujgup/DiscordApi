using System;
using System.Runtime.Serialization;

namespace Echo.Discord.Api {
	public class DiscordException : Exception {
		protected DiscordException(SerializationInfo info, StreamingContext context) : base(info, context) {
		}
		public DiscordException(string message, Exception innerException) : base(message, innerException) {
		}
		public DiscordException(string message) : base(message) {
		}
		public DiscordException() {
		}
	}
	public class InvalidMessageStructureException : DiscordException {
		protected InvalidMessageStructureException(SerializationInfo info, StreamingContext context) : base(info, context) {
		}
		public InvalidMessageStructureException(string message, Exception innerException) : base(message, innerException) {
		}
		public InvalidMessageStructureException(string message) : base(message) {
		}
		public InvalidMessageStructureException() {
		}
	}
	public class NoNicknameException : DiscordException {
		protected NoNicknameException(SerializationInfo info, StreamingContext context) : base(info, context) {
		}
		public NoNicknameException(string message, Exception innerException) : base(message, innerException) {
		}
		public NoNicknameException(string message) : base(message) {
		}
		public NoNicknameException() {
		}
	}
	public class VoiceUnsupportedException : DiscordException {
		protected VoiceUnsupportedException(SerializationInfo info, StreamingContext context) : base(info, context) {
		}
		public VoiceUnsupportedException(string message, Exception innerException) : base(message, innerException) {
		}
		public VoiceUnsupportedException(string message) : base(message) {
		}
		public VoiceUnsupportedException() {
		}
	}
	public class EnvironmentException : DiscordException {
		protected EnvironmentException(SerializationInfo info, StreamingContext context) : base(info, context) {
		}
		public EnvironmentException(string message, Exception innerException) : base(message, innerException) {
		}
		public EnvironmentException(string message) : base(message) {
		}
		public EnvironmentException() {
		}
	}
	public class ItemNotCachedException : DiscordException {
		protected ItemNotCachedException(SerializationInfo info, StreamingContext context) : base(info, context) {
		}
		public ItemNotCachedException(string message, Exception innerException) : base(message, innerException) {
		}
		public ItemNotCachedException(string message) : base(message) {
		}
		public ItemNotCachedException() {
		}
	}
	public class NotInEnvironmentException : EnvironmentException {
		protected NotInEnvironmentException(SerializationInfo info, StreamingContext context) : base(info, context) {
		}
		public NotInEnvironmentException(string message, Exception innerException) : base(message, innerException) {
		}
		public NotInEnvironmentException(string message) : base(message) {
		}
		public NotInEnvironmentException() {
		}
	}
	public class EnvironmentReassignmentException : EnvironmentException {
		protected EnvironmentReassignmentException(SerializationInfo info, StreamingContext context) : base(info, context) {
		}
		public EnvironmentReassignmentException(string message, Exception innerException) : base(message, innerException) {
		}
		public EnvironmentReassignmentException(string message) : base(message) {
		}
		public EnvironmentReassignmentException() {
		}
	}
	public class NoSuchObjectException : DiscordException {
		protected NoSuchObjectException(SerializationInfo info, StreamingContext context) : base(info, context) {
		}
		public NoSuchObjectException(string message, Exception innerException) : base(message, innerException) {
		}
		public NoSuchObjectException(string message) : base(message) {
		}
		public NoSuchObjectException() {
		}
	}
	public class NoSuchEmojiException : NoSuchObjectException {
		protected NoSuchEmojiException(SerializationInfo info, StreamingContext context) : base(info, context) {
		}
		public NoSuchEmojiException(string message, Exception innerException) : base(message, innerException) {
		}
		public NoSuchEmojiException(string message) : base(message) {
		}
		public NoSuchEmojiException() {
		}
	}
	public class NoSuchRoleException : NoSuchObjectException {
		protected NoSuchRoleException(SerializationInfo info, StreamingContext context) : base(info, context) {
		}
		public NoSuchRoleException(string message, Exception innerException) : base(message, innerException) {
		}
		public NoSuchRoleException(string message) : base(message) {
		}
		public NoSuchRoleException() {
		}
	}
	public class NoSuchUserException : NoSuchObjectException {
		protected NoSuchUserException(SerializationInfo info, StreamingContext context) : base(info, context) {
		}
		public NoSuchUserException(string message, Exception innerException) : base(message, innerException) {
		}
		public NoSuchUserException(string message) : base(message) {
		}
		public NoSuchUserException() {
		}
	}
	public class NoSuchServerDataException : NoSuchObjectException {
		protected NoSuchServerDataException(SerializationInfo info, StreamingContext context) : base(info, context) {
		}
		public NoSuchServerDataException(string message, Exception innerException) : base(message, innerException) {
		}
		public NoSuchServerDataException(string message) : base(message) {
		}
		public NoSuchServerDataException() {
		}
	}
}

namespace Echo.Discord.Api.Channels {
	public class UnsupportedChannelTypeException : DiscordException {
		protected UnsupportedChannelTypeException(SerializationInfo info, StreamingContext context) : base(info, context) {
		}
		public UnsupportedChannelTypeException(string message, Exception innerException) : base(message, innerException) {
		}
		public UnsupportedChannelTypeException(string message) : base(message) {
		}
		public UnsupportedChannelTypeException() {
		}
	}
	public class WrongChannelTypeException : DiscordException {
		protected WrongChannelTypeException(SerializationInfo info, StreamingContext context) : base(info, context) {
		}
		public WrongChannelTypeException(string message, Exception innerException) : base(message, innerException) {
		}
		public WrongChannelTypeException(string message) : base(message) {
		}
		public WrongChannelTypeException() {
		}
	}
	public class NoSuchChannelException : NoSuchObjectException {
		protected NoSuchChannelException(SerializationInfo info, StreamingContext context) : base(info, context) {
		}
		public NoSuchChannelException(string message, Exception innerException) : base(message, innerException) {
		}
		public NoSuchChannelException(string message) : base(message) {
		}
		public NoSuchChannelException() {
		}
	}
}

namespace Echo.Discord.Api.Http {
	public class CommunicationException : DiscordException {
		protected CommunicationException(SerializationInfo info, StreamingContext context) : base(info, context) {
		}
		public CommunicationException(string message, Exception innerException) : base(message, innerException) {
		}
		public CommunicationException(string message) : base(message) {
		}
		public CommunicationException() {
		}
	}
	public class IncorrectApiStructureException : DiscordException {
		protected IncorrectApiStructureException(SerializationInfo info, StreamingContext context) : base(info, context) {
		}
		public IncorrectApiStructureException(string message, Exception innerException) : base(message, innerException) {
		}
		public IncorrectApiStructureException(string message) : base(message) {
		}
		public IncorrectApiStructureException() {
		}
	}
	public class NotFoundException : DiscordException {
		protected NotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) {
		}
		public NotFoundException(string message, Exception innerException) : base(message, innerException) {
		}
		public NotFoundException(string message) : base(message) {
		}
		public NotFoundException() {
		}
	}
	public class PermissionDeniedException : DiscordException {
		protected PermissionDeniedException(SerializationInfo info, StreamingContext context) : base(info, context) {
		}
		public PermissionDeniedException(string message, Exception innerException) : base(message, innerException) {
		}
		public PermissionDeniedException(string message) : base(message) {
		}
		public PermissionDeniedException() {
		}
	}
	public class UnknownImageFormatException : DiscordException {
		protected UnknownImageFormatException(SerializationInfo info, StreamingContext context) : base(info, context) {
		}
		public UnknownImageFormatException(string message, Exception innerException) : base(message, innerException) {
		}
		public UnknownImageFormatException(string message) : base(message) {
		}
		public UnknownImageFormatException() {
		}
	}
	public class GatewayException : DiscordException {
		protected GatewayException(SerializationInfo info, StreamingContext context) : base(info, context) {
		}
		public GatewayException(string message, Exception innerException) : base(message, innerException) {
		}
		public GatewayException(string message) : base(message) {
		}
		public GatewayException() {
		}
	}
	public class GatewayConnectException : GatewayException {
		protected GatewayConnectException(SerializationInfo info, StreamingContext context) : base(info, context) {
		}
		public GatewayConnectException(string message, Exception innerException) : base(message, innerException) {
		}
		public GatewayConnectException(string message) : base(message) {
		}
		public GatewayConnectException() {
		}
	}
	public class UnknownOpCodeException : GatewayException {
		protected UnknownOpCodeException(SerializationInfo info, StreamingContext context) : base(info, context) {
		}
		public UnknownOpCodeException(string message, Exception innerException) : base(message, innerException) {
		}
		public UnknownOpCodeException(string message) : base(message) {
		}
		public UnknownOpCodeException() {
		}
	}
	public class UnknownEventException : GatewayException {
		protected UnknownEventException(SerializationInfo info, StreamingContext context) : base(info, context) {
		}
		public UnknownEventException(string message, Exception innerException) : base(message, innerException) {
		}
		public UnknownEventException(string message) : base(message) {
		}
		public UnknownEventException() {
		}
	}
}

namespace Echo.Discord.Api.Messages {
	public class NoSuchMessageException : NoSuchObjectException {
		protected NoSuchMessageException(SerializationInfo info, StreamingContext context) : base(info, context) {
		}
		public NoSuchMessageException(string message, Exception innerException) : base(message, innerException) {
		}
		public NoSuchMessageException(string message) : base(message) {
		}
		public NoSuchMessageException() {
		}
	}
}

namespace Echo.Discord.Api.Servers {
	public class NoSuchServerException : NoSuchObjectException {
		protected NoSuchServerException(SerializationInfo info, StreamingContext context) : base(info, context) {
		}
		public NoSuchServerException(string message, Exception innerException) : base(message, innerException) {
		}
		public NoSuchServerException(string message) : base(message) {
		}
		public NoSuchServerException() {
		}
	}
}
