using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Echo.Discord.Api {
	[JsonConverter(typeof(StringEnumConverter))]
	public enum UserStatus {
		[EnumMember(Value = "online")]
		Online,
		[EnumMember(Value = "idle")]
		Idle,
		[EnumMember(Value = "dnd")]
		DoNotDisturb,
		[EnumMember(Value = "offline")]
		Offline
	}
}
