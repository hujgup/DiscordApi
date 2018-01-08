using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ReSharper disable All

namespace Echo.Discord.Api.Json {
	#pragma warning disable 0649
	internal class VoiceStateJson {
		public ulong? guild_id;
		public ulong channel_id;
		public ulong user_id;
		public string session_id;
		public bool deaf;
		public bool mute;
		public bool self_deaf;
		public bool self_mute;
		public bool suppress;
	}
}
