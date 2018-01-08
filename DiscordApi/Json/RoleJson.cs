using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ReSharper disable All

namespace Echo.Discord.Api.Json {
	#pragma warning disable 0649
	internal class RoleJson : JsonId {
		public string name;
		public uint color;
		public bool hoist;
		public int position;
		public PermissionMask permissions;
		public bool managed;
		public bool mentionable;
	}
}
