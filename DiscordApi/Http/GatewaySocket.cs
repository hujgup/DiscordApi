using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebSocketSharp;
using Echo.Utils;
using Echo.Utils.Async;
using Echo.Utils.Sockets;
using JetBrains.Annotations;

namespace Echo.Discord.Api.Http {
	internal class GatewaySocket : ICloseable<ushort> {
		private readonly Socket _socket;
		private readonly Client _client;
		public GatewaySocket(Client client) {
			_socket = new Socket();
			_client = client;
		}
		public WebSocketState State {
			get => _socket.State;
		}
		public void Connect(string uri) {
			_socket.Connect(uri);
		}
		public async Task ReconnectAsync(GatewayClient client, bool resumable) {
			await _socket.WriteAsync(SocketData.CreateClose(1001));
			await client.ConnectAsync(true, resumable);
		}
		public ReceiveGatewayData Read() {
			return ReadAsync().Await();
		}
		[ItemCanBeNull]
		public async Task<ReceiveGatewayData> ReadAsync() {
			// TODO: Read in sequence order and update sequence variable
			return ReceiveGatewayData.CreateJson(await _socket.ReadAsync());
		}
		public async Task WriteAsync(JToken json) {
			await _socket.WriteAsync(SocketData.CreateText(json.ToString(Formatting.None)));
		}
		public void Write(JToken json) {
			WriteAsync(json).Await();
		}
		public void Close(ushort code) {
			_socket.Close(code);
		}
	}
}
