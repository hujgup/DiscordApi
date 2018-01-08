using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Echo.Utils;
using Echo.Utils.Sockets;
using Echo.Utils.Async;

namespace Echo.Discord.Api.Http {
	internal class ResponseListener : ICloseable<ushort>, IDisposable {
		private class ListenData {
			public ListenData(GatewayOpCode[] codes, string[] events, int occurs) {
				if (occurs == 0) {
					throw new ArgumentOutOfRangeException(nameof(occurs), "Value cannot be 0.");
				}
				Codes = codes;
				Events = events;
				Waiter = new Semaphore(0, 1);
				Data = new Queue<ReceiveGatewayData>();
				Occurs = occurs;
			}
			public GatewayOpCode[] Codes {
				get;
			}
			public string[] Events {
				get;
			}
			public bool CheckEventName {
				get => Events.Length > 0;
			}
			public Semaphore Waiter {
				get;
			}
			public Queue<ReceiveGatewayData> Data {
				get;
			}
			public bool Bounded {
				get => Occurs >= 0;
			}
			public int Occurs {
				get;
				set;
			}
			public bool Fits(ReceiveGatewayData data) {
				return Codes.Contains(data.OpCode) && (data.EventName == null || Events.Length == 0 || Events.Contains(data.EventName));
			}
		}

		private class UnhandledEvent : ICloseable {
			private readonly System.Timers.Timer _tmr;
			private readonly List<UnhandledEvent> _uhList;
			private readonly StrongSemaphoreSlim _uhListMutex;
			private bool _active;
			public UnhandledEvent(ReceiveGatewayData data, List<UnhandledEvent> uhList, StrongSemaphoreSlim uhListMutex) {
				Data = data;
				_uhList = uhList;
				_uhListMutex = uhListMutex;
				_uhList.Add(this);
				// TODO: Customize unhandled event expiry time
				_tmr = new System.Timers.Timer(60000) {
					AutoReset = false
				};
				_tmr.Elapsed += Remove;
				_tmr.Start();
				_active = true;
			}
			public ReceiveGatewayData Data {
				get;
			}
			private void Remove(object sender, System.Timers.ElapsedEventArgs e) {
				_uhListMutex.Wait();
				try {
					if (_active) {
						_active = false;
						_tmr.Stop();
						_tmr.Close();
						_tmr.Dispose();
						_uhList.Remove(this);
					}
				} finally {
					_uhListMutex.Release();
				}
			}
			// Close method assumes you have _uhListMutex acquired 
			public void Close() {
				if (_active) {
					_active = false;
					_tmr.Stop();
					_tmr.Close();
					_tmr.Dispose();
					_uhList.Remove(this);
				}
			}
		}

		public const int Unbounded = -1;
		private GatewaySocket _socket;
		private readonly SemaphoreSlim _socketMutex;
		private readonly BlockingQueue<ReceiveGatewayData> _dataQueue;
		private readonly List<UnhandledEvent> _unhandled;
		private readonly StrongSemaphoreSlim _listMutex;
		private readonly List<ListenData> _listeners;
		private Thread _tListen;
		private Thread _tResolve;
		private readonly List<Thread> _persistentListeners;
		private readonly SemaphoreSlim _plMutex;
		public ResponseListener(GatewaySocket socket) {
			_socket = socket;
			_socketMutex = new SemaphoreSlim(1);
			_dataQueue = new BlockingQueue<ReceiveGatewayData>();
			_unhandled = new List<UnhandledEvent>();
			_listMutex = new StrongSemaphoreSlim(1, 1);
			_listeners = new List<ListenData>();
			_persistentListeners = new List<Thread>();
			_plMutex = new SemaphoreSlim(1);
			StartListenerThreads();
		}
		private void StartListenerThreads() {
			_tListen = DiscordEnvironment.CreateSubthread(() => {
				AsyncContext.Switch(async () => {
					bool open = true;
					while (open) {
						ReceiveGatewayData data = await _socket.ReadAsync();
						if (data == null) {
							// TODO: Handle socket close
							DiscordDebug.WriteLine("Socket close occured (ResponseListener).");
							open = false;
						} else {
							_dataQueue.Enqueue(data);
						}
					}
				});
			});
			_tListen.Start();
			_tResolve = DiscordEnvironment.CreateSubthread(() => {
				AsyncContext.Switch(async () => {
					while (true) {
						// ReSharper disable AssignNullToNotNullAttribute
						ReceiveGatewayData data = _dataQueue.Dequeue();
						await _listMutex.WaitAsync();
						bool listNeedRelease = true;
						try {
							bool handled = false;
							foreach (ListenData lData in _listeners) {
								if (lData.Fits(data)) {
									if (lData.Bounded) {
										lData.Occurs--;
										if (lData.Occurs == 0) {
											_listeners.Remove(lData);
										}
									}
									_listMutex.Release();
									listNeedRelease = false;
									lData.Data.Enqueue(data);
									lData.Waiter.Release();
									handled = true;
									break;
								}
							}
							if (!handled) {
								_unhandled.Add(new UnhandledEvent(data, _unhandled, _listMutex));
							}
						} finally {
							if (listNeedRelease) {
								_listMutex.Release();
							}
						}
						// ReSharper restore AssignNullToNotNullAttribute
					}
				});
			});
			_tResolve.Start();
		}
		internal void UpdateSocket(GatewaySocket socket) {
			_socketMutex.Wait();
			try {
				Dispose();
				_socket = socket;
				StartListenerThreads();
			} finally {
				_socketMutex.Release();
			}
		}
		public async Task<ReceiveGatewayData> SendAsync(JObject data, GatewayOpCode[] responseCodes, params string[] responseEventNames) {
			await _listMutex.WaitAsync();
			ListenData lData;
			try {
				lData = new ListenData(responseCodes, responseEventNames, 1);
				_listeners.Add(lData);
			} finally {
				_listMutex.Release();
			}
			await _socketMutex.WaitAsync();
			try {
				await _socket.WriteAsync(data);
			} finally {
				_socketMutex.Release();
			}
			lData.Waiter.WaitOne();
			await _listMutex.WaitAsync();
			try {
				_listeners.Remove(lData);
			} finally {
				_listMutex.Release();
			}
			return lData.Data.Dequeue();
		}
		public async Task<ReceiveGatewayData> SendAsync(JObject data, GatewayOpCode responseCode, params string[] responseEventNames) {
			return await SendAsync(data, new[] { responseCode }, responseEventNames);
		}
		public ReceiveGatewayData Send(JObject data, GatewayOpCode[] responseCodes, params string[] responseEventNames) {
			return SendAsync(data, responseCodes, responseEventNames).Await();
		}
		public ReceiveGatewayData Send(JObject data, GatewayOpCode responseCode, params string[] responseEventNames) {
			return SendAsync(data, responseCode, responseEventNames).Await();
		}
		public async Task<Thread> ListenAsync(Func<ReceiveGatewayData, Task> listener, int occurs, GatewayOpCode[] responseCodes, params string[] responseEventNames) {
			Thread listenThread = DiscordEnvironment.CreateSubthread(() => {
				AsyncContext.Switch(async () => {
					var data = new ListenData(responseCodes, responseEventNames, occurs);
					{
						var uhDataList = new List<ReceiveGatewayData>();
						await _listMutex.WaitAsync();
						try {
							foreach (UnhandledEvent uhData in _unhandled) {
								if (data.Fits(uhData.Data)) {
									uhDataList.Add(uhData.Data);
									uhData.Close();
									data.Occurs--;
									if (data.Occurs == 0) {
										break;
									}
								}
							}
							if (data.Occurs > 0) {
								_listeners.Add(data);
							}
						} finally {
							_listMutex.Release();
						}
						foreach (ReceiveGatewayData d in uhDataList) {
							await listener(d);
						}
					}
					while (!data.Bounded || data.Occurs > 0) {
						data.Waiter.WaitOne();
						await listener(data.Data.Dequeue());
					}
				});
			});
			await _plMutex.WaitAsync();
			try {
				_persistentListeners.Add(listenThread);
			} finally {
				_plMutex.Release();
			}
			listenThread.Start();
			return listenThread;
		}
		public async Task<Thread> ListenAsync(Func<ReceiveGatewayData, Task> listener, int occurs, GatewayOpCode responseCode, params string[] responseEventNames) {
			return await ListenAsync(listener, occurs, new[] { responseCode }, responseEventNames);
		}
		public Thread Listen(Action<ReceiveGatewayData> listener, int occurs, GatewayOpCode[] responseCodes, params string[] responseEventNames) {
			return ListenAsync(data => {
				listener(data);
				return Task.CompletedTask;
			}, occurs, responseCodes, responseEventNames).Await();
		}
		public Thread Listen(Action<ReceiveGatewayData> listener, int occurs, GatewayOpCode responseCode, params string[] responseEventNames) {
			return Listen(listener, occurs, new[] { responseCode }, responseEventNames);
		}
		public void Unlisten(Thread listenThread) {
			_plMutex.Wait();
			try {
				listenThread.Abort();
				_persistentListeners.Remove(listenThread);
			} finally {
				_plMutex.Release();
			}
		}
		public void Unlisten() {
			_plMutex.Wait();
			try {
				foreach (Thread t in _persistentListeners) {
					t.Abort();
				}
			} finally {
				_plMutex.Release();
			}
		}
		public void Close(ushort code) {
			_socket.Close(code);
		}
		public void Dispose() {
			Unlisten();
			_tListen.Abort();
			_tResolve.Abort();
		}
	}
}
