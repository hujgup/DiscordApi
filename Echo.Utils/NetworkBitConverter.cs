using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Echo.Utils {
	public static partial class NetworkBitConverter {
		private static byte[] OrderOut(byte[] bytes) {
			if (BitConverter.IsLittleEndian) {
				Array.Reverse(bytes);
			}
			return bytes;
		}
		private static byte[] OrderIn(byte[] bytes, int startIndex, int length) {
			if (BitConverter.IsLittleEndian) {
				Array.Reverse(bytes, startIndex, length);
			}
			return bytes;
		}
		private static byte[] OrderIn(byte[] bytes, int startIndex) {
			return OrderIn(bytes, startIndex, bytes.Length - startIndex);
		}
		public static string ToString(byte[] bytes, int startIndex, int length) {
			return BitConverter.ToString(OrderIn(bytes, startIndex, length), startIndex, length);
		}
		public static string ToString(byte[] bytes, int startIndex) {
			return BitConverter.ToString(OrderIn(bytes, startIndex), startIndex);
		}
		public static string ToString(byte[] bytes) {
			return BitConverter.ToString(OrderIn(bytes, 0));
		}
	}
}
