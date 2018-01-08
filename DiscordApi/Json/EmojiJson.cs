using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ReSharper disable All

namespace Echo.Discord.Api.Json {
	#pragma warning disable 0649
	internal class EmojiJson : JsonId {
		public string name;
		public ulong[] roles;
		public UserJson user;
		public bool require_colons;
		public bool managed;
	}
}
