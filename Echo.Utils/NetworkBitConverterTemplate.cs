using System;

namespace Echo.Utils {
	public static partial class NetworkBitConverter {
		public static byte[] GetBytes(bool value) {
			return OrderOut(BitConverter.GetBytes(value));
		}
		public static bool ToBoolean(byte[] value, int startIndex) {
			return BitConverter.ToBoolean(OrderIn(value, startIndex), startIndex);
		}
		public static byte[] GetBytes(short value) {
			return OrderOut(BitConverter.GetBytes(value));
		}
		public static short ToInt16(byte[] value, int startIndex) {
			return BitConverter.ToInt16(OrderIn(value, startIndex), startIndex);
		}
		public static byte[] GetBytes(ushort value) {
			return OrderOut(BitConverter.GetBytes(value));
		}
		public static ushort ToUInt16(byte[] value, int startIndex) {
			return BitConverter.ToUInt16(OrderIn(value, startIndex), startIndex);
		}
		public static byte[] GetBytes(int value) {
			return OrderOut(BitConverter.GetBytes(value));
		}
		public static int ToInt32(byte[] value, int startIndex) {
			return BitConverter.ToInt32(OrderIn(value, startIndex), startIndex);
		}
		public static byte[] GetBytes(uint value) {
			return OrderOut(BitConverter.GetBytes(value));
		}
		public static uint ToUInt32(byte[] value, int startIndex) {
			return BitConverter.ToUInt32(OrderIn(value, startIndex), startIndex);
		}
		public static byte[] GetBytes(long value) {
			return OrderOut(BitConverter.GetBytes(value));
		}
		public static long ToInt64(byte[] value, int startIndex) {
			return BitConverter.ToInt64(OrderIn(value, startIndex), startIndex);
		}
		public static byte[] GetBytes(ulong value) {
			return OrderOut(BitConverter.GetBytes(value));
		}
		public static ulong ToUInt64(byte[] value, int startIndex) {
			return BitConverter.ToUInt64(OrderIn(value, startIndex), startIndex);
		}
		public static byte[] GetBytes(char value) {
			return OrderOut(BitConverter.GetBytes(value));
		}
		public static char ToChar(byte[] value, int startIndex) {
			return BitConverter.ToChar(OrderIn(value, startIndex), startIndex);
		}
		public static byte[] GetBytes(float value) {
			return OrderOut(BitConverter.GetBytes(value));
		}
		public static float ToSingle(byte[] value, int startIndex) {
			return BitConverter.ToSingle(OrderIn(value, startIndex), startIndex);
		}
		public static byte[] GetBytes(double value) {
			return OrderOut(BitConverter.GetBytes(value));
		}
		public static double ToDouble(byte[] value, int startIndex) {
			return BitConverter.ToDouble(OrderIn(value, startIndex), startIndex);
		}
	}
}