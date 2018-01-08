using System;
using System.Linq;
using Echo.Utils.Json;
using Echo.Discord.Api.Json;
using Echo.Discord.Api.Http;
using Echo.Discord.Api.Channels;

namespace Echo.Discord.Api.Messages {
	public abstract partial class Message {
	// TODO: Make channel concrete instead of a promise, and thus fix the cast error
		protected Message(JsonObjectReader r) {
			Id = r.ReadSnowflake("id");
			ChannelId = r.ReadSnowflake("channel_id");
			Channel = new CachedPromise<TextChannel>(new AliasCache<IChannel, TextChannel>(ChannelUtils._globalCache), ChannelId, TextChannel.GetAsync);
			Content = r.ReadString("content");
			CreationTime = DateTime.Parse(r.ReadString("timestamp"));
			string letStr = r.ReadNullableString("edited_timestamp");if (letStr != null) { LastEditTime = (DateTime?)DateTime.Parse(letStr); }
			IsTextToSpeech = r.ReadBoolean("tts");
			MentionedEveryone = r.ReadBoolean("mention_everyone");
			MentionedUsers = r.ReadObjectArray<UserJson>("mentions").Select(x => User.CreateFromJson(x, null)).ToIdDic();
			MentionedRoles = r.ReadObjectArray<RoleJson>("mention_roles").Select(x => Role.CreateFromJson(((IServerChannel)Channel.GetCachedValue()).ServerId, x, null)).ToIdDic();
			IsPinned = r.ReadBoolean("pinned");
			MsgType = (InternalMessageType)r.ReadInt32("type");
			_cache.Add(this);
		}
		internal virtual void UpdateInstance(JsonObjectReader r) {
			ulong cid = r.ReadSnowflake("channel_id");
			if (cid != ChannelId) {
				ChannelId = cid;
				Channel = new CachedPromise<TextChannel>(new AliasCache<IChannel, TextChannel>(ChannelUtils._globalCache), ChannelId, TextChannel.GetAsync);
			}
			// author and webhook_id should be dealt with in this method's overloads
			if (r.Contains("content")) {
				Content = r.ReadString("content");
			}
			if (r.Contains("timestamp")) {
				CreationTime = DateTime.Parse(r.ReadString("timestamp"));
			}
			if (r.Contains("edited_timestamp")) {
				string letStr = r.ReadNullableString("edited_timestamp");if (letStr != null) { LastEditTime = (DateTime?)DateTime.Parse(letStr); }
			}
			if (r.Contains("tts")) {
				IsTextToSpeech = r.ReadBoolean("tts");
			}
			if (r.Contains("mention_everyone")) {
				MentionedEveryone = r.ReadBoolean("mention_everyone");
			}
			if (r.Contains("mentions")) {
				MentionedUsers = r.ReadObjectArray<UserJson>("mentions").Select(x => User.CreateFromJson(x, null)).ToIdDic();
			}
			if (r.Contains("mention_roles")) {
				MentionedRoles = r.ReadObjectArray<RoleJson>("mention_roles").Select(x => Role.CreateFromJson(((IServerChannel)Channel.GetCachedValue()).ServerId, x, null)).ToIdDic();
			}
			if (r.Contains("pinned")) {
				IsPinned = r.ReadBoolean("pinned");
			}
			if (r.Contains("type")) {
				MsgType = (InternalMessageType)r.ReadInt32("type");
			}
		}
	}
}