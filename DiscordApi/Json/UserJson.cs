using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ReSharper disable All

namespace Echo.Discord.Api.Json {
	#pragma warning disable 0649
	internal class UserJson : JsonId {
		public string username;
		public string discriminator;
		public string avatar;
		public bool? bot;
		public bool? mfa_enabled;
		public bool? verified;
		public string email;
	}
}
