using System;
using System.Text;
using System.Net.WebSockets;

namespace Echo.Utils.Sockets {
	public class SocketData {
		private static readonly Encoding _enc = Encoding.UTF8;
		private SocketData(byte[] binary, WebSocketMessageType messageType, ushort code) {
			Binary = binary;
			MessageType = messageType;
			CloseCode = code;
		}
		private SocketData(byte[] binary, WebSocketMessageType messageType) : this(binary, messageType, 0) {
		}
		protected SocketData() {
		}
		protected static T CreateBinary<T>(byte[] binBuffer, int binOffset, int binCount) where T : SocketData {
			var binary = new byte[binCount];
			Array.Copy(binBuffer, binOffset, binary, 0, binCount);
			return CreateBinary<T>(binary);
		}
		protected static T CreateBinary<T>(byte[] binary) where T : SocketData {
			var res = Activator.CreateInstance<T>();
			res.Binary = binary;
			res.MessageType = WebSocketMessageType.Binary;
			return res;
		}
		protected static T CreateText<T>(string text) where T : SocketData {
			byte[] buffer = _enc.GetBytes(text);
			var res = Activator.CreateInstance<T>();
			res.Binary = buffer;
			res.MessageType = WebSocketMessageType.Text;
			return res;
		}
		protected static T CreateClose<T>(ushort code) where T : SocketData {
			var res = Activator.CreateInstance<T>();
			res.Binary = new byte[0];
			res.MessageType = WebSocketMessageType.Close;
			res.CloseCode = code;
			return res;
		}
		public static SocketData CreateBinary(byte[] binBuffer, int binOffset, int binCount) {
			var binary = new byte[binCount];
			Array.Copy(binBuffer, binOffset, binary, 0, binCount);
			return CreateBinary(binary);
		}
		public static SocketData CreateBinary(byte[] binary) {
			return new SocketData(binary, WebSocketMessageType.Binary);
		}
		public static SocketData CreateText(string text) {
			byte[] buffer = _enc.GetBytes(text);
			return new SocketData(buffer, WebSocketMessageType.Text);
		}
		public static SocketData CreateClose(ushort code) {
			return new SocketData(new byte[0], WebSocketMessageType.Close, code);
		}
		public static SocketData Create(byte[] binBuffer, int binOffset, int binCount, WebSocketMessageType messageType) {
			var binary = new byte[binCount];
			Array.Copy(binBuffer, binOffset, binary, 0, binCount);
			return Create(binary, messageType);
		}
		public static SocketData Create(byte[] binary, WebSocketMessageType messageType) {
			return new SocketData(binary, messageType);
		}
		public byte[] Binary {
			get;
			protected set;
		}
		public WebSocketMessageType MessageType {
			get;
			protected set;
		}
		public ushort CloseCode {
			get;
			protected set;
		}
		public string GetText() {
			if (MessageType != WebSocketMessageType.Text) {
				throw new InvalidOperationException("Cannot convert a non-textual message into text.");
			} else {
				return _enc.GetString(Binary);
			}
		}
	}
}
