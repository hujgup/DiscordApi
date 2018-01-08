using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using Echo.Discord.Api.Channels;

// ReSharper disable All

namespace Echo.Discord.Api.Json {
	#pragma warning disable 0649
	internal class ChannelJson : JsonId {
		public ChannelType type;
		public ulong? guild_id;
		public int? position;
		public OverwriteJson[] permission_overwrites;
		public string name;
		public string topic;
		public bool? nsfw;
		public ulong? last_message_id;
		public int? bitrate;
		public int? user_limit;
		public UserJson[] recipients;
		public string icon;
		public ulong? owner_id;
		public ulong? application_id;
		public ulong? parent_id;
	}
}
