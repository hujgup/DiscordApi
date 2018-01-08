using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Echo.Utils.Json;
using Echo.Discord.Api.Http;
using Echo.Discord.Api.Channels;
using Echo.Discord.Api.Json;
using Echo.Discord.Api.Events;
using JetBrains.Annotations;

namespace Echo.Discord.Api.Messages {
	public abstract partial class Message : IEquatable<Message>, ICached, IHasManyMentionedUsers, IHasManyMentionedRoles, IHasCachedChannel<TextChannel> {
		private static readonly BaseCache<Message> _cache = new BaseCache<Message>();
		internal static Message Create(JObject obj) {
			JsonObjectReader r = obj.GetReader();
			ulong id = r.ReadSnowflake("id");
			return _cache.Mutex(() => {
				Message res;
				if (_cache.Contains(id)) {
					res = _cache[id];
					res.UpdateInstance(r);
				} else {
					res = r.Contains("webhook_id") ? (Message)new WebhookMessage(r) : new StandardMessage(r);
				}
				return res;
			});
		}
		internal static Message Update(JObject obj) {
			JsonObjectReader r = obj.GetReader();
			ulong id = r.ReadSnowflake("id");
			return _cache.Mutex(() => {
				Message res;
				if (_cache.Contains(id)) {
					res = _cache[id];
					res.UpdateInstance(r);
				} else {
					// TODO: Create new message object when GET Message endpoint is done
					res = null;
				}
				return res;
			});
		}
		internal InternalMessageType MsgType {
			get;
			set;
		}
		public ulong Id {
			get;
		}
		public MessageType Type {
			get;
			protected set;
		}
		public ulong ChannelId {
			get;
			private set;
		}
		public CachedPromise<TextChannel> Channel {
			get;
			private set;
		}
		public string Content {
			get;
			private set;
		}
		public DateTime CreationTime {
			get;
			private set;
		}
		public bool IsEdit {
			get => LastEditTime != null;
		}
		public DateTime? LastEditTime {
			get;
			private set;
		}
		public bool IsTextToSpeech {
			get;
			private set;
		}
		public bool MentionedEveryone {
			get;
			private set;
		}
		public IReadOnlyDictionary<ulong, User> MentionedUsers {
			get;
			private set;
		}
		public IReadOnlyDictionary<ulong, Role> MentionedRoles {
			get;
			private set;
		}
		// TODO: public IReadOnlyList<Attachment> Attachments (when Attachment object is done)
		// TODO: public IReadOnlyList<Embed> EmbeddedContent (when Embed object is done)
		// TODO: public IReadOnlyList<Reaction> Reactions (when Reaction object is done)
		// TODO: Figure out when nonce is (https://discordapp.com/developers/docs/resources/channel#message-object)
		public bool IsPinned {
			get;
			private set;
		}
		internal static void OnPinAny(object sender, PinEventArgs e) {
			PinAny?.Invoke(sender, e);
		}
		internal static void OnUnpinAny(object sender, PinEventArgs e) {
			UnpinAny?.Invoke(sender, e);
		}
		public static event EventHandler<PinEventArgs> PinAny;
		public static event EventHandler<PinEventArgs> UnpinAny;
		internal void OnPin(object sender, PinEventArgs e) {
			Pin?.Invoke(sender, e);
		}
		internal void OnUnpin(object sender, PinEventArgs e) {
			Unpin?.Invoke(sender, e);
		}
		public event EventHandler<PinEventArgs> Pin;
		public event EventHandler<PinEventArgs> Unpin;
		public abstract override string ToString();
		public void Uncache() {
			FactoryUtils.Uncache(_cache, this);
		}
		public bool Equals([CanBeNull] Message msg) {
			return !(msg is null) && Id == msg.Id;
		}
		public override bool Equals([CanBeNull] object obj) {
			return obj is Message message && Equals(message);
		}
		public override int GetHashCode() {
			return Id.GetHashCode();
		}
		public static bool operator ==([CanBeNull] Message a, [CanBeNull] Message b) {
			return a is null ? b is null : a.Equals(b);
		}
		public static bool operator !=([CanBeNull] Message a, [CanBeNull] Message b) {
			return !(a == b);
		}
	}
}
