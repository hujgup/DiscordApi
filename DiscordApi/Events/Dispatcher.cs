using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Echo.Utils;
using Echo.Utils.Async;
using Echo.Discord.Api.Http;
using Echo.Discord.Api.Channels;
using Echo.Discord.Api.Servers;
using Echo.Discord.Api.Messages;

namespace Echo.Discord.Api.Events {
	internal partial class Dispatcher {
		private readonly GatewayClient _client;
		private readonly ResponseListener _listener;
		private Thread _invalidThread;
		public Dispatcher(GatewayClient client, ResponseListener listener) {
			_client = client;
			_listener = listener;
			SetUpAll();
		}
		private void SetUpAll() {
			SetUp();
			SetUpChannelEvents();
			SetUpMessagePinEvents();
			// TODO: Use templating to generate listeners
		}
		private void SetUpMessagePinEvents() {
			_listener.Listen(data => {
				var d = (GatewayEvent.ChannelPinsUpdate)data.Data;
				List<PinnedMessageData> args = PinEventArgs.Create(d.ChannelId, _client);
				foreach (PinnedMessageData d2 in args) {
					var b = new EventBubbler<PinEventArgs>();
					ServerTextChannel channel;
					Message msg;
					Server server;
					if (d2.Added.Count > 0) {
						msg = d2.Added[0];
						b.Append(msg.OnPin);
						channel = ServerTextChannel.GetCached(d.ChannelId);
						if (channel != null) {
							b.Append(channel.OnMessagePin);
							server = channel.Server.GetCachedValue();
							if (server != null) {
								b.Append(server.OnMessagePin);
							}
						}
						b.Append(Message.OnPinAny);
					} else {
						msg = d2.Removed[0];
						b.Append(msg.OnUnpin);
						channel = ServerTextChannel.GetCached(d.ChannelId);
						if (channel != null) {
							b.Append(channel.OnMessageUnpin);
							server = channel.Server.GetCachedValue();
							if (server != null) {
								b.Append(server.OnMessageUnpin);
							}
						}
						b.Append(Message.OnUnpinAny);
					}
					b.Invoke(_client, new PinEventArgs(d2));
				}
			}, ResponseListener.Unbounded, GatewayOpCode.Dispatch, GatewayEvent.ChannelPinsUpdate.Id);
		}
		public void SetUp() {
			_invalidThread = _listener.ListenAsync(async data => {
				await _client.ConnectAsync(true, (bool)data.Data);
			}, ResponseListener.Unbounded, GatewayOpCode.InvalidSession).Await();
		}
		public void TearDown() {
			_listener.Unlisten(_invalidThread);
		}
	}
}
