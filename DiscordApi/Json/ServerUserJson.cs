using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ReSharper disable All

namespace Echo.Discord.Api.Json {
	#pragma warning disable 0649
	internal class ServerUserJson {
		public UserJson user;
		public string nick;
		public ulong[] roles;
		public string joined_at;
		public bool deaf;
		public bool mute;
	}
}
