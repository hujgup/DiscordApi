using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Echo.Utils.Json;

namespace Echo.Discord.Api.Http {
	internal struct GatewayHelloData {
		private const string _heartbeatKey = "heartbeat_interval";
		private const string _traceKey = "_trace";
		public GatewayHelloData(JObject obj) : this() {
			var r = new JsonObjectReader(obj);
			HeartbeatInterval = TimeSpan.FromMilliseconds(r.ReadDouble(_heartbeatKey));
			ConnectedServers = r.ReadStringArray(_traceKey);
		}
		public TimeSpan HeartbeatInterval {
			get;
		}
		public string[] ConnectedServers {
			get;
		}
		public static explicit operator JObject(GatewayHelloData data) {
			return new RootJsonObjectBuilder()
				.Int64Property(_heartbeatKey, (long)data.HeartbeatInterval.TotalMilliseconds)
				.StringArrayProperty(_traceKey, data.ConnectedServers)
				.Build();
		}
	}
}
