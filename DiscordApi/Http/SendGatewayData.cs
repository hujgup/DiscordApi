using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Echo.Utils.Sockets;
using Echo.Utils.Json;
using Echo.Discord.Api.Json;
using JetBrains.Annotations;

namespace Echo.Discord.Api.Http {
	internal static class SendGatewayData {
		public static JObject Create(GatewayOpCode opCode, [CanBeNull] JToken data) {
			return new RootJsonObjectBuilder()
			       .Int32Property("op", (int)opCode)
			       .DynamicProperty("d", data)
			       .Build();
		}
	}
}
