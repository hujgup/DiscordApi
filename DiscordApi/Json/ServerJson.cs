using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Echo.Discord.Api.Servers;

// ReSharper disable All

namespace Echo.Discord.Api.Json {
	#pragma warning disable 0649
	internal class ServerJson : JsonId {
		public string name;
		public string icon;
		public string splash;
		public ulong owner_id;
		public string region;
		public ulong? afk_channel_id;
		public int afk_timeout;
		public bool embed_enabled;
		public ulong? embed_channel_id;
		public VerificationLevel verification_level;
		public NotificationLevel default_message_notifications;
		public NsfwFilterLevel explicit_content_filter;
		public RoleJson[] roles;
		public EmojiJson[] emojis;
		public string[] features;
		public MfaLevel mfa_level;
		public bool widget_enabled;
		public ulong? widget_channel_id;
		public string joined_at;
		public bool? large;
		public bool? unavailable;
		public int? member_count;
		public VoiceStateJson[] voice_states;
		public ServerUserJson[] members;
		public ChannelJson[] channels;
		public PresenceUpdateJson[] presences;
	}
}
