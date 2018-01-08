using System;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using WebSocketSharp;
using Echo.Utils.Async;
using WebSocket = WebSocketSharp.WebSocket;
using WebSocketState = WebSocketSharp.WebSocketState;

namespace Echo.Utils.Sockets {
	public class Socket : ICloseable<ushort> {
		private WebSocket _socket;
		private readonly SemaphoreSlim _sendMutex;
		private readonly bool _disposed;
		private readonly SemaphoreSlim _dspMutex;
		private readonly BlockingQueue<SocketData> _in;
		public Socket() {
			_sendMutex = new SemaphoreSlim(1);
			_disposed = false;
			_dspMutex = new SemaphoreSlim(1);
			_in = new BlockingQueue<SocketData>();
		}
		public WebSocketState State {
			get => _socket.ReadyState;
		}
		public void Connect(string uri) {
			_socket = new WebSocket(uri);
			_socket.Connect();
			_socket.OnMessage += (sender, e) => {
				if (e.IsBinary) {
					_in.Enqueue(SocketData.CreateBinary(e.RawData));
				} else if (e.IsText) {
					_in.Enqueue(SocketData.CreateText(e.Data));
				}
			};
			_socket.OnClose += (sender, e) => {
				_in.Enqueue(SocketData.CreateClose(e.Code));
			};
		}
		public async Task<SocketData> ReadAsync() {
			await _dspMutex.WaitAsync();
			if (_disposed) {
				_dspMutex.Release();
				throw new ObjectDisposedException(nameof(Socket));
			} else {
				_dspMutex.Release();
				return _in.Dequeue();
			}
		}
		public SocketData Read() {
			return ReadAsync().Await();
		}
		public async Task WriteAsync(byte[] binary) {
			await _dspMutex.WaitAsync();
			if (_disposed) {
				_dspMutex.Release();
				throw new ObjectDisposedException(nameof(Socket));
			} else {
				await _sendMutex.WaitAsync();
				_dspMutex.Release();
				var s = new Semaphore(0, 1);
				_socket.SendAsync(binary, arg => {
					s.Release();
				});
				s.WaitOne();
				_sendMutex.Release();
			}
		}
		public async Task WriteAsync(SocketData data) {
			await WriteAsync(data.Binary);
		}
		public void Write(byte[] binary) {
			WriteAsync(binary).Await();
		}
		public void Write(SocketData data) {
			WriteAsync(data).Await();
		}
		public void Close(ushort code) {
			_socket.Close(code);
		}
	}
}
