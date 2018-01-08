using Echo.Utils;
using Echo.Discord.Api.Channels;
using Echo.Discord.Api.Messages;

namespace Echo.Discord.Api.Events {
	public partial class ChannelEventArgs : BubbleEventArgs {
		internal ChannelEventArgs(IChannel channel) {
			Channel = channel;
		}
		public IChannel Channel {
			get;
		}
		public ChannelEventArgs<T> Convert<T>() where T : IChannel {
			return new ChannelEventArgs<T>(this);
		}
	}
	/// <summary>
	/// Aliases <see cref="ChannelEventArgs"/> for types derived from <see cref="IChannel"/>.
	/// </summary>
	/// <typeparam name="T">The more specific type of the IChannel.</typeparam>
	public class ChannelEventArgs<T> : AliasEventArgs<ChannelEventArgs> where T : IChannel {
		internal ChannelEventArgs(ChannelEventArgs args) : base(args) {
			Channel = (T)args.Channel;
		}
		public T Channel {
			get;
		}
	}
	public partial class MessageEventArgs : BubbleEventArgs {
		internal MessageEventArgs(Message message) {
			Message = message;
		}
		public Message Message {
			get;
		}
		public MessageEventArgs<T> Convert<T>() where T : Message {
			return new MessageEventArgs<T>(this);
		}
	}
	/// <summary>
	/// Aliases <see cref="MessageEventArgs"/> for types derived from <see cref="Messages.Message"/>.
	/// </summary>
	/// <typeparam name="T">The more specific type of the Message.</typeparam>
	public class MessageEventArgs<T> : AliasEventArgs<MessageEventArgs> where T : Message {
		internal MessageEventArgs(MessageEventArgs args) : base(args) {
			Message = (T)args.Message;
		}
		public T Message {
			get;
		}
	}
	public partial class PinEventArgs : BubbleEventArgs {
		internal PinEventArgs(PinnedMessageData data) {
			Data = data;
		}
		public PinnedMessageData Data {
			get;
		}
	}
}