using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using WebSocketSharp;
using Echo.Utils;
using Echo.Utils.Async;
using Echo.Utils.Json;
using Echo.Discord.Api.Events;
using Echo.Discord.Api.Servers;
using JetBrains.Annotations;

namespace Echo.Discord.Api.Http {
	public class GatewayClient : Client, IDisposable {
		#pragma warning disable 0649
		[UsedImplicitly]
		private class GetGatewayJson {
			public string url;
		}
		#pragma warning restore 0649

		private const int _maxConnectAttempts = 8;
		private readonly SemaphoreSlim _connectMutex;
		private ResponseListener _listener;
		private Dispatcher _dispatcher;
		private string _gatewayUrl;
		private readonly SemaphoreSlim _seqMutex;
		private long? _lastSequence;
		private GatewayHeartbeater _heartbeater;
		private readonly int _shardId;
		private readonly int _shardCount;
		private string _sessionId;
		private ICollection<ulong> _awaitServers;
		private readonly SemaphoreSlim _asMutex;
		private Semaphore _asDone;
		internal GatewayClient(Token token, int shardId, int shardCount) : base(token) {
			_connectMutex = new SemaphoreSlim(1);
			_seqMutex = new SemaphoreSlim(1);
			_lastSequence = null;
			_shardId = shardId;
			_shardCount = shardCount;
			_asMutex = new SemaphoreSlim(1);
			Events = new EventListener();
		}
		internal long? Sequence {
			get {
				_seqMutex.Wait();
				try {
					return _lastSequence;
				} finally {
					_seqMutex.Release();
				}
			}
			set {
				_seqMutex.Wait();
				try {
					_lastSequence = value;
				} finally {
					_seqMutex.Release();
				}
			}
		}
		public EventListener Events {
			get;
		}
		public bool FirePinEvents {
			get;
			set;
		}
		private readonly SemaphoreSlim _gatewayBucket = new SemaphoreSlim(1);
		private async Task<string> GetGatewayUrlAsync() {
			_gatewayUrl = _gatewayUrl ?? ((await GetResponseAsync<GetGatewayJson>(_gatewayBucket, HttpMethod.Get, _baseUrl + "gateway")).url + "?v=6&encoding=json");
			return _gatewayUrl;
		}
		private GatewayIdentifyData MakeIdentify() {
			return new GatewayIdentifyData(this, false, 250, _shardId, _shardCount, new GatewayUpdateStatus(UserStatus.Online, false, null, null));
		}
		private JObject MakeResume() {
			_seqMutex.Wait();
			try {
				return new RootJsonObjectBuilder()
				       .StringProperty("token", Token)
				       .StringProperty("session_id", _sessionId)
				       .Int64Property("seq", _lastSequence)
				       .Build();
			} finally {
				_seqMutex.Release();
			}
		}
		private void GuildCreate(ReceiveGatewayData data) {
			var d = (GatewayEvent.EvtServer)data.Data;
			_asMutex.Wait();
			try {
				DiscordDebug.WriteLine("GUILD_CREATE " + d.Data.Id + " (" + (_awaitServers.Count - 1) + " remaining)");
				_awaitServers.Remove(d.Data.Id);
				if (_awaitServers.Count == 0) {
					_asDone.Release();
				}
			} finally {
				_asMutex.Release();
			}
		}
		private async Task<Server[]> IdentAsync(bool reconnect, bool resumable) {
			bool ident = true;
			if (reconnect && resumable) {
				ReceiveGatewayData resumed = await _listener.SendAsync(SendGatewayData.Create(GatewayOpCode.Resume, MakeResume()), new[] {
					GatewayOpCode.Dispatch,
					GatewayOpCode.InvalidSession
				}, GatewayEvent.Resumed.Id);
				ident = resumed.OpCode == GatewayOpCode.InvalidSession;
				if (ident) {
					// Per specs, client must wait 1-5 seconds before sending a fresh Identify payload (https://discordapp.com/developers/docs/topics/gateway#resuming)
					await Task.Delay(2000);
				}
			}
			await _asMutex.WaitAsync();
			bool needRelease = true;
			Server[] res = null;
			try {
				while (ident) {
					DiscordDebug.WriteLine("Identing...");
					ReceiveGatewayData ready = await _listener.SendAsync(SendGatewayData.Create(GatewayOpCode.Identify, (JObject)MakeIdentify()), new[] {
						GatewayOpCode.Dispatch,
						GatewayOpCode.InvalidSession
					}, GatewayEvent.Ready.Id);
					ident = ready.OpCode == GatewayOpCode.InvalidSession;
					if (ident) {
						DiscordDebug.WriteLine("Ident rate-limited, waiting and trying again...");
						// Per specs, the only case in which this occurs is when you hit the 5-second Identify rate limit (https://discordapp.com/developers/docs/topics/gateway#identifying)
						await Task.Delay(5000);
					} else {
						DiscordDebug.WriteLine("Ident successful, copying session data...");
						var readyData = (GatewayEvent.Ready)ready.Data;
						_sessionId = readyData.SessionId;
						DiscordDebug.WriteLine("Session data copied.");
						DiscordDebug.WriteLine("Awaiting server create events (expecting [" + string.Join(", ", readyData.ServersToCreate) + "] (" + readyData.ServersToCreate.Count + "))...");
						_awaitServers = readyData.ServersToCreate;
						_listener.Listen(GuildCreate, _awaitServers.Count, GatewayOpCode.Dispatch, GatewayEvent.EvtServer.CreateId);
						_asDone = new Semaphore(0, 1);
						_asMutex.Release();
						needRelease = false;
						res = readyData.ServersToCreate.Select(Server.GetCached).ToArray();
						_asDone.WaitOne();
						DiscordDebug.WriteLine("All expected server create events have occured.");
					}
					DiscordDebug.WriteLine("Ident done.");
				}
			} finally {
				if (needRelease) {
					_asMutex.Release();
				}
			}
			DiscordDebug.WriteLine("Setting up event listener...");
			_dispatcher = new Dispatcher(this, _listener);
			DiscordDebug.WriteLine("Event listener set up.");
			DiscordDebug.WriteLine("Socket ready to use");
			// ReSharper disable once AssignNullToNotNullAttribute
			return res;
		}
		[ItemCanBeNull]
		private async Task<Server[]> HandshakeAsync(bool reconnect, bool resumable, GatewaySocket socket, IRef<bool> loop) {
			DiscordDebug.WriteLine("Getting Hello data...");
			ReceiveGatewayData data = await socket.ReadAsync();
			DiscordDebug.WriteLine("Got Hello data.");
			if (reconnect) {
				_listener.UpdateSocket(socket);
			} else {
				DiscordDebug.WriteLine("Initializing response listener and heartbeater...");
				_listener = new ResponseListener(socket);
				_heartbeater = new GatewayHeartbeater(_listener, _seqMutex, () => _lastSequence, ConnectAsync);
				DiscordDebug.WriteLine("Response listener and heartbeater initialized.");
			}
			// Need to handle cases where heartbeat timer elapses, thus the event/semaphore model
			DiscordDebug.WriteLine("Waiting for initial heartbeat ack...");
			var signal = new Semaphore(0, 1);
			bool success = false;
			void EvtListener(object sender, bool e) {
				success = e;
				signal.Release();
			}
			_heartbeater.Started += EvtListener;
			_heartbeater.Start(data, reconnect);
			signal.WaitOne();
			_heartbeater.Started -= EvtListener;
			if (success) {
				DiscordDebug.WriteLine("Heartbeat ack heard.");
				return await IdentAsync(reconnect, resumable);
				// ReSharper disable once RedundantIfElseBlock
			} else {
				DiscordDebug.WriteLine("Heartbeat ack not heard, trying again...");
				// Reconnect immediately
				await _heartbeater.AbortAsync();
				_heartbeater.Dispose();
				_listener.Dispose();
				loop.Value = true;
				return null;
			}
		}
		internal async Task<Server[]> ConnectAsync(bool reconnect, bool resumable) {
			if (reconnect) {
				_dispatcher?.TearDown();
			}
			var loop = new SharedRef<bool>(false);
			int loops = 0;
			DiscordDebug.WriteLine("Starting connection appempt...");
			Server[] res = null;
			do {
				if (loops >= _maxConnectAttempts) {
					throw new GatewayConnectException("Gateway failed to connect " + _maxConnectAttempts + " times, and the connection attempt was aborted.");
				}
				loop.Value = false;
				await _connectMutex.WaitAsync();
				if (_listener == null) {
					string url = await GetGatewayUrlAsync();
					DiscordDebug.WriteLine("Opening socket...");
					var socket = new GatewaySocket(this);
					socket.Connect(url);
					if (socket.State == WebSocketState.Open) {
						DiscordDebug.WriteLine("Socket opened.");
						res = await HandshakeAsync(reconnect, resumable, socket, loop);
					} else {
						DiscordDebug.WriteLine("Socket opening failed, trying again...");
						loop.Value = true;
					}
				}
				_connectMutex.Release();
				loops++;
			} while (loop.Value);
			DiscordDebug.WriteLine("Connection attempt done.");
			// ReSharper disable once AssignNullToNotNullAttribute (null value is default to get compiler to shut up - whenever res is null, the loop should continue)
			return res;
		}
		public async Task<Server[]> ConnectAsync() {
			return await ConnectAsync(false, false);
		}
		public Server[] Connect() {
			return ConnectAsync().Await();
		}
		public override string ToString() {
			return "GatewayClient: " + Token;
		}
		/// <summary>
		/// Disconnects this client from Discord. This method must be called when you are finished with this object.
		/// </summary>
		public void Disconnect() {
			_heartbeater.Dispose();
			_dispatcher.TearDown();
			_listener.Dispose();
			_heartbeater = null;
			_listener = null;
			_dispatcher = null;
			// TODO: Disconnect
		}
		public void Dispose() {
			if (_listener != null) {
				Disconnect();
			}
		}
	}
}
