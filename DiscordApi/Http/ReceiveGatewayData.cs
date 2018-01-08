using System;
using System.Collections.Generic;
using System.Diagnostics;
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
	internal class ReceiveGatewayData : SocketData {
		protected ReceiveGatewayData() {
		}
		[CanBeNull]
		public static ReceiveGatewayData CreateJson(SocketData data) {
			ReceiveGatewayData res;
			if (data.MessageType == WebSocketMessageType.Close) {
				res = null;
			} else {
				res = new ReceiveGatewayData();
				var j = JsonConvert.DeserializeObject<GatewayJson>(data.GetText());
				res.OpCode = j.op;
				// ReSharper disable once SwitchStatementMissingSomeCases (default case exists)
				switch (res.OpCode) {
					case GatewayOpCode.Dispatch:
						res.Sequence = (int)j.s;
						res.EventName = j.t;
						res.Data = GatewayEvent.Create(j.t, j.d);
						break;
					case GatewayOpCode.Heartbeat:
						res.Data = ((JValue)j.d).Value;
						break;
					case GatewayOpCode.Hello:
						res.Data = new GatewayHelloData((JObject)j.d);
						break;
					case GatewayOpCode.InvalidSession:
						res.Data = (bool)((JValue)j.d).Value;
						break;
					case GatewayOpCode.Reconnect:
					case GatewayOpCode.HeartbeatAck:
						res.Data = null;
						break;
					default:
						throw new UnknownOpCodeException("Gateway op code " + (int)j.op + " is either invalid in a recieve context or is undefined.");
				}
			}
			return res;
		}
		public static new ReceiveGatewayData CreateClose(ushort code) {
			return SocketData.CreateClose<ReceiveGatewayData>(code);
		}
		public GatewayOpCode OpCode {
			get;
			private set;
		}
		public int Sequence {
			get;
			private set;
		}
		[CanBeNull]
		public string EventName {
			get;
			private set;
		}
		public object Data {
			get;
			private set;
		}
		public override string ToString() {
			string res = "[[[\nOp " + OpCode + " (" + (int)OpCode + ")";
			if (EventName != null) {
				res += "\nSeq " + Sequence + "\nEvt " + EventName;
			}
			res += "\nData " + DiscordDebug.JsonString(Data) + "\n]]]";
			return res;
		}
	}
}
