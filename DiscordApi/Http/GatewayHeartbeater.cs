using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Newtonsoft.Json.Linq;
using Echo.Utils.Json;
using Echo.Utils.Async;
using Timer = System.Timers.Timer;

namespace Echo.Discord.Api.Http {
	internal class GatewayHeartbeater : IDisposable {
		private readonly ResponseListener _listener;
		private readonly SemaphoreSlim _seqMutex;
		private readonly Func<long?> _seqGetter;
		private int _aborts;
		private bool _fire;
		private readonly SemaphoreSlim _ackMutex;
		private bool _heardAck;
		private Timer _heartbeat;
		private readonly Func<bool, bool, Task> _connect;
		public GatewayHeartbeater(ResponseListener listener, SemaphoreSlim seqMutex, Func<long?> seqGetter, Func<bool, bool, Task> connect) {
			// TODO: Send heartbeat back when gateway requests a heartbeat (listen for GatewayOpCode.Heartbeat)
			_listener = listener;
			_seqMutex = seqMutex;
			_seqGetter = seqGetter;
			_aborts = 0;
			_fire = false;
			_ackMutex = new SemaphoreSlim(1);
			_heardAck = true;
			_connect = connect;
		}
		public event EventHandler<bool> Started;
		private async Task HeartbeatAsync() {
			await _ackMutex.WaitAsync();
			bool needRelease = true;
			try {
				if (_heardAck) {
					_heardAck = false;
				} else {
					await _connect(true, true);
					_heardAck = false;
					bool oldFire = _fire;
					_fire = false;
					_ackMutex.Release();
					needRelease = false;
					if (oldFire) {
						Started?.Invoke(this, false);
					}
				}
			} finally {
				if (needRelease) {
					_ackMutex.Release();
				}
			}
			await _seqMutex.WaitAsync();
			JObject obj;
			try {
				obj = SendGatewayData.Create(GatewayOpCode.Heartbeat, _seqGetter());
			} finally {
				_seqMutex.Release();
			}
			await _listener.SendAsync(obj, GatewayOpCode.HeartbeatAck);
			await _ackMutex.WaitAsync();
			try {
				_heardAck = true;
			} finally {
				_ackMutex.Release();
			}
		}
		public void Start(ReceiveGatewayData helloData, bool reconnect) {
			Thread starter = DiscordEnvironment.CreateSubthread(() => {
				if (reconnect) {
					_heartbeat.Stop();
					_heartbeat.Dispose();
				}
				_fire = true;
				//Task initBeat = HeartbeatAsync();
				_heartbeat = new Timer(((GatewayHelloData)helloData.Data).HeartbeatInterval.TotalMilliseconds) {
					AutoReset = true,
					SynchronizingObject = null
				};
				_heartbeat.Elapsed += (sender, args) => HeartbeatAsync().Await();
				_heartbeat.Start();
				//initBeat.Await();
				_ackMutex.Wait();
				if (_aborts > 0) {
					_fire = false;
					_aborts--;
					_ackMutex.Release();
				} else {
					bool oldFire = _fire;
					_fire = false;
					_ackMutex.Release();
					if (oldFire) {
						Started?.Invoke(this, true);
					}
				}
			});
			starter.Start();
		}
		public async Task AbortAsync() {
			await _ackMutex.WaitAsync();
			try {
				_aborts++;
			} finally {
				_ackMutex.Release();
			}
		}
		public void Dispose() {
			if (_heartbeat != null) {
				_heartbeat.Stop();
				_heartbeat.Dispose();
				_heartbeat = null;
			}
		}
	}
}
