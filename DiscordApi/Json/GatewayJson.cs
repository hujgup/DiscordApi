using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Echo.Discord.Api.Http;

// ReSharper disable All

namespace Echo.Discord.Api.Json {
	#pragma warning disable 0649
	/// <summary>
	/// JSON contents of a received socket message.
	/// </summary>
	internal class GatewayJson {
		/// <summary>
		/// Message op code.
		/// </summary>
		public GatewayOpCode op;
		/// <summary>
		/// Data component of the message. Srt iff <see cref="op"/> is not <see cref="GatewayOpCode.HeartbeatAck"/>.
		/// <para />
		/// Type is <see cref="JObject" /> for JSON objects, <see cref="JArray" /> for JSON arrays, and <see cref="JValue" /> for JSON primitives.
		/// </summary>
		public object d;
		/// <summary>
		/// Sequence number. Set iff <see cref="op"/> is <see cref="GatewayOpCode.Dispatch"/>.
		/// </summary>
		public int? s;
		/// <summary>
		/// Event name. Set iff <see cref="op"/> is <see cref="GatewayOpCode.Dispatch"/>.
		/// </summary>
		public string t;
	}
}
