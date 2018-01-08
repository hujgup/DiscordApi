using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Echo.Utils;
using Echo.Utils.Json;
using Echo.Utils.Async;
using Echo.Discord.Api.Channels;
using Echo.Discord.Api.Json;
using Echo.Discord.Api.Events;
using System.Diagnostics;
using Echo.Discord.Api.Servers;
using JetBrains.Annotations;

namespace Echo.Discord.Api.Http {
	internal static class GatewayEvent {
		public class Ready {
			public const string Id = "READY";
			public Ready(JObject data) {
				JsonObjectReader r = data.GetReader();
				ProtocolVersion = r.ReadInt32("v");
				ClientServerUser = User.CreateFromJson(r.ReadObject("user").ToObject<UserJson>(), null);
				// ReSharper disable once CoVariantArrayConversion (read-only)
				DmChannels = r.ReadArray("private_channels").AllObject<ChannelJson>().Select(x => DirectMessageTextChannel.CreateFromJson(x, null)).ToArray();
				ServersToCreate = r.ReadObjectArray<UaServerJson>("guilds").Where(x => x.unavailable).Select(x => x.id).ToList();
				SessionId = r.ReadString("session_id");
				Trace = r.ReadStringArray("_trace");
			}
			public int ProtocolVersion {
				get;
			}
			public User ClientServerUser {
				get;
			}
			public IDirectMessageChannel[] DmChannels {
				get;
			}
			public List<ulong> ServersToCreate {
				get;
			}
			public string SessionId {
				get;
			}
			public string[] Trace {
				get;
			}
			public static bool EventIsThis(string eventName) {
				return eventName == Id;
			}
			public static bool EventIsThis(ReceiveGatewayData data) {
				return data.EventName != null && EventIsThis(data.EventName);
			}
		}

		// TODO: Check docs for when this event is fired
		public class Resumed {
			public const string Id = "RESUMED";
			public Resumed(JObject data) {
				Trace = data.GetReader().ReadStringArray("_trace");
			}
			public string[] Trace {
				get;
			}
			public static bool EventIsThis(string eventName) {
				return eventName == Id;
			}
			public static bool EventIsThis(ReceiveGatewayData data) {
				return data.EventName != null && EventIsThis(data.EventName);
			}
		}

		public static class InvalidSession {
			public const string Id = "INVALID_SESSION";
			public static bool EventIsThis(string eventName) {
				return eventName == Id;
			}
			public static bool EventIsThis(ReceiveGatewayData data) {
				return data.EventName != null && EventIsThis(data.EventName);
			}
		}

		public class Channel {
			public const string CreateId = "CHANNEL_CREATE";
			public const string UpdateId = "CHANNEL_UPDATE";
			public const string DeleteId = "CHANNEL_DELETE";
			public Channel(JObject data) {
				Data = ChannelUtils.CreateFromJson(data.ToObject<ChannelJson>(), null);
			}
			[CanBeNull] // Iff JSON type was ServerVoice
			public IChannel Data {
				get;
			}
			public static bool EventIsThis(string eventName) {
				return eventName == CreateId || eventName == UpdateId || eventName == DeleteId;
			}
			public static bool EventIsThis(ReceiveGatewayData data) {
				return data.EventName != null && data.EventName != null && EventIsThis(data.EventName);
			}
		}

		public class ChannelPinsUpdate {
			public const string Id = "CHANNEL_PINS_UPDATE";
			public ChannelPinsUpdate(JObject data) {
				JsonObjectReader r = data.GetReader();
				ChannelId = r.ReadSnowflake("channel_id");
				string ts = r.ReadNullableString("list_pin_timestamp");
				LastPinTime = ts != null ? DateTime.Parse(ts) : DateTime.MinValue;
			}
			public ulong ChannelId {
				get;
			}
			/// <summary>
			/// The time that the last pin to the channel specified by <see cref="ChannelId"/> occured. Equal to <see cref="DateTime.MinValue"/> iff value in JSON was null.
			/// </summary>
			public DateTime LastPinTime {
				get;
			}
			public static bool EventIsThis(string eventName) {
				return eventName == Id;
			}
			public static bool EventIsThis(ReceiveGatewayData data) {
				return data.EventName != null && EventIsThis(data.EventName);
			}
		}

		// TODO: Below events (only do ones needed for bots + state update events, put TODO comments over others)
		// => Create event static on Server, Update event bubbles Server instance, Server static
		public class EvtServer {
			public const string CreateId = "GUILD_CREATE";
			public const string UpdateId = "GUILD_UPDATE";
			public EvtServer(JObject data) {
				Data = Server.CreateFromJson(data.ToObject<ServerJson>(), null);
			}
			public Server Data {
				get;
			}
			public static bool EventIsThis(string eventName) {
				return eventName == CreateId || eventName == UpdateId;
			}
			public static bool EventIsThis(ReceiveGatewayData data) {
				return data.EventName != null && EventIsThis(data.EventName);
			}
		}

		public static class ServerDelete {
			public const string Id = "GUILD_DELETE";
			public static bool EventIsThis(string eventName) {
				return eventName == Id;
			}
			public static bool EventIsThis(ReceiveGatewayData data) {
				return data.EventName != null && EventIsThis(data.EventName);
			}
		}

		public class ServerUser : EvtUser {
			public const string BanAddId = "GUILD_BAN_ADD";
			public const string BanRemoveId = "GUILD_BAN_REMOVE";
			public const string MemberAddId = "GUILD_MEMBER_ADD";
			public const string MemberRemoveId = "GUILD_MEMBER_REMOVE";
			public ServerUser(JObject data, bool seperate) {
				if (seperate) {
					JsonObjectReader r = data.GetReader();
					ServerId = r.ReadSnowflake("guild_id");
					Data = User.CreateFromJson(r.ReadObject<UserJson>("user"), ServerId);
				} else {
					var json = data.ToObject<ServerUserGatewayJson>();
					ServerId = json.guild_id;
					Data = User.CreateFromJson(json, ServerId);
				}
			}
			public ulong ServerId {
				get;
			}
			public static new bool EventIsThis(string eventName) {
				return eventName == BanAddId || eventName == BanRemoveId || eventName == MemberAddId;
			}
			public static new bool EventIsThis(ReceiveGatewayData data) {
				return data.EventName != null && EventIsThis(data.EventName);
			}
		}

		public class ServerUserSet {
			public const string Id = "GUILD_MEMBERS_CHUNK";
			public ServerUserSet(JObject data) {
				JsonObjectReader r = data.GetReader();
				ServerId = r.ReadSnowflake("guild_id");
				Users = r.ReadObjectArray<ServerUserJson>("members").Select(x => {
					User res = User.CreateFromJson(x.user, ServerId);
					LazyUser.PushServerData(ServerId, x);
					return res;
				}).ToArray();
			}
			public User[] Users {
				get;
			}
			public ulong ServerId {
				get;
			}
			public static bool EventIsThis(string eventName) {
				return eventName == Id;
			}
			public static bool EventIsThis(ReceiveGatewayData data) {
				return data.EventName != null && EventIsThis(data.EventName);
			}
		}

		public class ServerUserUpdate : ServerUser {
			public new const string Id = "GUILD_MEMBER_UPDATE";
			public ServerUserUpdate(JObject data) : base(data, true) {
				JsonObjectReader r = data.GetReader();
				UserRoles = r.ReadSnowflakeArray("roles");
				Nickname = r.ReadString("nick");
			}
			public ulong[] UserRoles {
				get;
			}
			public string Nickname {
				get;
			}
			public static new bool EventIsThis(string eventName) {
				return eventName == Id;
			}
			public static new bool EventIsThis(ReceiveGatewayData data) {
				return data.EventName != null && EventIsThis(data.EventName);
			}
		}

		public class ServerEmojiUpdate {
			public const string Id = "GUILD_EMOJIS_UPDATE";
			public ServerEmojiUpdate(JObject data) {
				JsonObjectReader r = data.GetReader();
				ServerId = r.ReadSnowflake("guild_id");
				Emojis = r.ReadObjectArray<EmojiJson>("emojis").Select(x => ServerEmoji.CreateFromJson(ServerId, x, null)).ToArray();
			}
			public Emoji[] Emojis {
				get;
			}
			public ulong ServerId {
				get;
			}
			public static bool EventIsThis(string eventName) {
				return eventName == Id;
			}
			public static bool EventIsThis(ReceiveGatewayData data) {
				return data.EventName != null && EventIsThis(data.EventName);
			}
		}

		public class RawServer {
			public const string Id = "GUILD_INTEGRATIONS_UPDATE";
			public RawServer(JObject data) {
				ServerId = data.ToObject<JsonId>().id;
			}
			public ulong ServerId {
				get;
			}
			public static bool EventIsThis(string eventName) {
				return eventName == Id;
			}
			public static bool EventIsThis(ReceiveGatewayData data) {
				return data.EventName != null && EventIsThis(data.EventName);
			}
		}

		public class ServerRole {
			public const string CreateId = "GUILD_ROLE_CREATE";
			public const string UpdateId = "GUILD_ROLE_UPDATE";
			public ServerRole(JObject data) {
				JsonObjectReader r = data.GetReader();
				ServerId = r.ReadSnowflake("server_id");
				Role = Role.CreateFromJson(ServerId, r.ReadObject<RoleJson>("role"), null);
			}
			public Role Role {
				get;
			}
			public ulong ServerId {
				get;
			}
			public static bool EventIsThis(string eventName) {
				return eventName == CreateId || eventName == UpdateId;
			}
			public static bool EventIsThis(ReceiveGatewayData data) {
				return data.EventName != null && EventIsThis(data.EventName);
			}
		}

		public class ServerRoleDelete {
			public const string Id = "GUILD_ROLE_DELETE";
			public ServerRoleDelete(JObject data) {
				JsonObjectReader r = data.GetReader();
				RoleId = r.ReadSnowflake("role_id");
				ServerId = r.ReadSnowflake("guild_id");
			}
			public ulong RoleId {
				get;
			}
			public ulong ServerId {
				get;
			}
			public static bool EventIsThis(string eventName) {
				return eventName == Id;
			}
			public static bool EventIsThis(ReceiveGatewayData data) {
				return data.EventName != null && EventIsThis(data.EventName);
			}
		}

		public class Message {
			public const string CreateId = "MESSAGE_CREATE";
			public const string UpdateId = "MESSAGE_UPDATE";
			public Message(JObject data, bool isPartial) {
				Data = isPartial ? Messages.Message.Update(data) : Messages.Message.Create(data);
			}
			public Messages.Message Data {
				get;
			}
			public static bool EventIsThis(string eventName) {
				return eventName == CreateId || eventName == UpdateId;
			}
			public static bool EventIsThis(ReceiveGatewayData data) {
				return data.EventName != null && EventIsThis(data.EventName);
			}
		}

		public class RawMessage {
			public const string DeleteId = "MESSAGE_DELETE";
			public const string BulkDeleteId = "MESSAGE_DELETE_BULK";
			public RawMessage(JObject data, bool isSet) {
				JsonObjectReader r = data.GetReader();
				ChannelId = r.ReadSnowflake("channel_id");
				Messages = isSet ? r.ReadSnowflakeArray("ids") : new ulong[] { r.ReadSnowflake("id") };
			}
			public ulong[] Messages {
				get;
			}
			public ulong ChannelId {
				get;
			}
			public static bool EventIsThis(string eventName) {
				return eventName == DeleteId || eventName == BulkDeleteId;
			}
			public static bool EventIsThis(ReceiveGatewayData data) {
				return data.EventName != null && EventIsThis(data.EventName);
			}
		}

		public class Reaction {
			public const string AddId = "MESSAGE_REACTION_ADD";
			public const string RemoveId = "MESSAGE_REACTION_REMOVE";
			public Reaction(JObject data) {
				JsonObjectReader r = data.GetReader();
				ReactorId = r.ReadSnowflake("user_id");
				MessageId = r.ReadSnowflake("message_id");
				ChannelId = r.ReadSnowflake("channel_id");
				JsonObjectReader re = r.ReadObject("emoji");
				ulong? emojiId = re.ReadNullableSnowflake("id");
				string name = re.ReadString("name");
				Emoji = emojiId != null ? (Emoji)new LazyServerEmoji((ulong)emojiId, name) : new GlobalEmoji(name);
			}
			public ulong ReactorId {
				get;
			}
			public ulong MessageId {
				get;
			}
			public ulong ChannelId {
				get;
			}
			public Emoji Emoji {
				get;
			}
			public static bool EventIsThis(string eventName) {
				return eventName == AddId || eventName == RemoveId;
			}
			public static bool EventIsThis(ReceiveGatewayData data) {
				return data.EventName != null && EventIsThis(data.EventName);
			}
		}

		public class ReactionRemoveAll {
			public const string Id = "MESSAGE_REACTION_REMOVE_ALL";
			public ReactionRemoveAll(JObject data) {
				JsonObjectReader r = data.GetReader();
				MessageId = r.ReadSnowflake("message_id");
				ChannelId = r.ReadSnowflake("channel_id");
			}
			public ulong MessageId {
				get;
			}
			public ulong ChannelId {
				get;
			}
			public static bool EventIsThis(string eventName) {
				return eventName == Id;
			}
			public static bool EventIsThis(ReceiveGatewayData data) {
				return data.EventName != null && EventIsThis(data.EventName);
			}
		}

		public class Webhook {
			public const string Id = "WEBHOOKS_UPDATE";
			public Webhook(JObject data) {
				JsonObjectReader r = data.GetReader();
				ServerId = r.ReadSnowflake("guild_id");
				ChannelId = r.ReadSnowflake("channel_id");
			}
			public ulong ServerId {
				get;
			}
			public ulong ChannelId {
				get;
			}
			public static bool EventIsThis(string eventName) {
				return eventName == Id;
			}
			public static bool EventIsThis(ReceiveGatewayData data) {
				return data.EventName != null && EventIsThis(data.EventName);
			}
		}

		public class EvtUser {
			public const string Id = "USER_UPDATE";
			protected EvtUser() {
			}
			public EvtUser(JObject data) {
				Data = User.CreateFromJson(data.ToObject<UserJson>(), null);
			}
			public User Data {
				get;
				protected set;
			}
			public static bool EventIsThis(string eventName) {
				return eventName == Id;
			}
			public static bool EventIsThis(ReceiveGatewayData data) {
				return data.EventName != null && EventIsThis(data.EventName);
			}
		}

		// TODO: On presence support, https://discordapp.com/developers/docs/topics/gateway#presence
		// TODO: Figure out where Typing Stop is, them implement lookup methods on TextChannel and LazyUser (https://discordapp.com/developers/docs/topics/gateway#typing-start)
		// TODO: On voice support, https://discordapp.com/developers/docs/topics/gateway#voice
		/// <summary>
		/// Represents a valid gateway event that the API doesn't handle.
		/// </summary>
		public class Unimplemented {
			public const string PresenceUpdateId = "PRESENCE_UPDATE";
			public const string TypingStartId = "TYPING_START";
			public const string VoiceStateUpdateId = "VOICE_STATE_UPDATE";
			public const string VoiceServerUpdateId = "VOICE_SERVER_UPDATE";
			public static readonly Unimplemented Instance = new Unimplemented();
			private Unimplemented() {
			}
			public static bool EventIsThis(string eventName) {
				return eventName == PresenceUpdateId || eventName == TypingStartId || eventName == VoiceStateUpdateId || eventName == VoiceServerUpdateId;
			}
			public static bool EventIsThis(ReceiveGatewayData data) {
				return data.EventName != null && EventIsThis(data.EventName);
			}
		}

		public static object Create(string eventName, object data) {
			object res;
			if (Ready.EventIsThis(eventName)) {
				res = new Ready((JObject)data);
			} else if (Resumed.EventIsThis(eventName)) {
				res = new Resumed((JObject)data);
			} else if (InvalidSession.EventIsThis(eventName)) {
				res = (bool)((JValue)data).Value;
			} else if (Channel.EventIsThis(eventName)) {
				res = new Channel((JObject)data);
			} else if (ChannelPinsUpdate.EventIsThis(eventName)) {
				res = new ChannelPinsUpdate((JObject)data);
			} else if (EvtServer.EventIsThis(eventName)) {
				res = new EvtServer((JObject)data);
			} else if (ServerDelete.EventIsThis(eventName)) {
				res = ((JObject)data).ToObject<UaServerJson>().id;
			} else if (ServerUser.EventIsThis(eventName)) {
				res = new ServerUser((JObject)data, eventName == ServerUser.MemberRemoveId);
			} else if (ServerUserSet.EventIsThis(eventName)) {
				res = new ServerUserSet((JObject)data);
			} else if (ServerUserUpdate.EventIsThis(eventName)) {
				res = new ServerUserUpdate((JObject)data);
			} else if (ServerEmojiUpdate.EventIsThis(eventName)) {
				res = new ServerEmojiUpdate((JObject)data);
			} else if (RawServer.EventIsThis(eventName)) {
				res = new RawServer((JObject)data);
			} else if (ServerRole.EventIsThis(eventName)) {
				res = new ServerRole((JObject)data);
			} else if (ServerRoleDelete.EventIsThis(eventName)) {
				res = new ServerRoleDelete((JObject)data);
			} else if (Message.EventIsThis(eventName)) {
				res = new Message((JObject)data, eventName == Message.UpdateId);
			} else if (RawMessage.EventIsThis(eventName)) {
				res = new RawMessage((JObject)data, eventName == RawMessage.BulkDeleteId);
			} else if (Reaction.EventIsThis(eventName)) {
				res = new Reaction((JObject)data);
			} else if (ReactionRemoveAll.EventIsThis(eventName)) {
				res = new ReactionRemoveAll((JObject)data);
			} else if (Webhook.EventIsThis(eventName)) {
				res = new Webhook((JObject)data);
			} else if (EvtUser.EventIsThis(eventName)) {
				res = new EvtUser((JObject)data);
			} else if (Unimplemented.EventIsThis(eventName)) {
				res = Unimplemented.Instance;
			} else {
				throw new UnknownEventException("Gateway event \"" + eventName + "\" is unknown.");
			}
			DiscordDebug.WriteLine("Heard event: " + eventName + ", handling w/ object " + res.GetType().Name);
			return res;
		}
	}
}
