using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using JetBrains.Annotations;

namespace Echo.Utils.Json {
	public partial class JsonArrayReader {
		public bool[] NextBooleanArray() {
			return NextArray().AllBoolean();
		}
		public bool NextBoolean() {
			return Convert.ToBoolean(NextValue());
		}
		public bool[] AllBoolean() {
			return _arr.Children().Select(x => Convert.ToBoolean(((JValue)x).Value)).ToArray();
		}
		public bool[] RestBoolean() {
			var res = new List<bool>();
			while (_arrI.MoveNext()) {
				// ReSharper disable once PossibleNullReferenceException
				res.Add(Convert.ToBoolean(((JValue)_arrI.Current).Value));
			}
			return res.ToArray();
		}
		public bool? NextNullableBoolean() {
			object value = NextValue();
			return value != null ? (bool?)Convert.ToBoolean(value) : null;
		}
		public bool?[] NextNullableBooleanArray() {
			return NextArray().AllNullableBoolean();
		}
		public bool?[] AllNullableBoolean() {
			return _arr.Children().Select(x => ((JValue)x).Value != null ? (bool?)Convert.ToBoolean(((JValue)x).Value) : null).ToArray();
		}
		public bool?[] RestNullableBoolean() {
			var res = new List<bool?>();
			while (_arrI.MoveNext()) {
				// ReSharper disable PossibleNullReferenceException
				res.Add(((JValue)_arrI.Current).Value != null ? (bool?)Convert.ToBoolean(((JValue)_arrI.Current).Value) : null);
				// ReSharper restore PossibleNullReferenceException
			}
			return res.ToArray();
		}
		public byte[] NextByteArray() {
			return NextArray().AllByte();
		}
		public byte NextByte() {
			return Convert.ToByte(NextValue());
		}
		public byte[] AllByte() {
			return _arr.Children().Select(x => Convert.ToByte(((JValue)x).Value)).ToArray();
		}
		public byte[] RestByte() {
			var res = new List<byte>();
			while (_arrI.MoveNext()) {
				// ReSharper disable once PossibleNullReferenceException
				res.Add(Convert.ToByte(((JValue)_arrI.Current).Value));
			}
			return res.ToArray();
		}
		public byte? NextNullableByte() {
			object value = NextValue();
			return value != null ? (byte?)Convert.ToByte(value) : null;
		}
		public byte?[] NextNullableByteArray() {
			return NextArray().AllNullableByte();
		}
		public byte?[] AllNullableByte() {
			return _arr.Children().Select(x => ((JValue)x).Value != null ? (byte?)Convert.ToByte(((JValue)x).Value) : null).ToArray();
		}
		public byte?[] RestNullableByte() {
			var res = new List<byte?>();
			while (_arrI.MoveNext()) {
				// ReSharper disable PossibleNullReferenceException
				res.Add(((JValue)_arrI.Current).Value != null ? (byte?)Convert.ToByte(((JValue)_arrI.Current).Value) : null);
				// ReSharper restore PossibleNullReferenceException
			}
			return res.ToArray();
		}
		public sbyte[] NextSByteArray() {
			return NextArray().AllSByte();
		}
		public sbyte NextSByte() {
			return Convert.ToSByte(NextValue());
		}
		public sbyte[] AllSByte() {
			return _arr.Children().Select(x => Convert.ToSByte(((JValue)x).Value)).ToArray();
		}
		public sbyte[] RestSByte() {
			var res = new List<sbyte>();
			while (_arrI.MoveNext()) {
				// ReSharper disable once PossibleNullReferenceException
				res.Add(Convert.ToSByte(((JValue)_arrI.Current).Value));
			}
			return res.ToArray();
		}
		public sbyte? NextNullableSByte() {
			object value = NextValue();
			return value != null ? (sbyte?)Convert.ToSByte(value) : null;
		}
		public sbyte?[] NextNullableSByteArray() {
			return NextArray().AllNullableSByte();
		}
		public sbyte?[] AllNullableSByte() {
			return _arr.Children().Select(x => ((JValue)x).Value != null ? (sbyte?)Convert.ToSByte(((JValue)x).Value) : null).ToArray();
		}
		public sbyte?[] RestNullableSByte() {
			var res = new List<sbyte?>();
			while (_arrI.MoveNext()) {
				// ReSharper disable PossibleNullReferenceException
				res.Add(((JValue)_arrI.Current).Value != null ? (sbyte?)Convert.ToSByte(((JValue)_arrI.Current).Value) : null);
				// ReSharper restore PossibleNullReferenceException
			}
			return res.ToArray();
		}
		public short[] NextInt16Array() {
			return NextArray().AllInt16();
		}
		public short NextInt16() {
			return Convert.ToInt16(NextValue());
		}
		public short[] AllInt16() {
			return _arr.Children().Select(x => Convert.ToInt16(((JValue)x).Value)).ToArray();
		}
		public short[] RestInt16() {
			var res = new List<short>();
			while (_arrI.MoveNext()) {
				// ReSharper disable once PossibleNullReferenceException
				res.Add(Convert.ToInt16(((JValue)_arrI.Current).Value));
			}
			return res.ToArray();
		}
		public short? NextNullableInt16() {
			object value = NextValue();
			return value != null ? (short?)Convert.ToInt16(value) : null;
		}
		public short?[] NextNullableInt16Array() {
			return NextArray().AllNullableInt16();
		}
		public short?[] AllNullableInt16() {
			return _arr.Children().Select(x => ((JValue)x).Value != null ? (short?)Convert.ToInt16(((JValue)x).Value) : null).ToArray();
		}
		public short?[] RestNullableInt16() {
			var res = new List<short?>();
			while (_arrI.MoveNext()) {
				// ReSharper disable PossibleNullReferenceException
				res.Add(((JValue)_arrI.Current).Value != null ? (short?)Convert.ToInt16(((JValue)_arrI.Current).Value) : null);
				// ReSharper restore PossibleNullReferenceException
			}
			return res.ToArray();
		}
		public ushort[] NextUInt16Array() {
			return NextArray().AllUInt16();
		}
		public ushort NextUInt16() {
			return Convert.ToUInt16(NextValue());
		}
		public ushort[] AllUInt16() {
			return _arr.Children().Select(x => Convert.ToUInt16(((JValue)x).Value)).ToArray();
		}
		public ushort[] RestUInt16() {
			var res = new List<ushort>();
			while (_arrI.MoveNext()) {
				// ReSharper disable once PossibleNullReferenceException
				res.Add(Convert.ToUInt16(((JValue)_arrI.Current).Value));
			}
			return res.ToArray();
		}
		public ushort? NextNullableUInt16() {
			object value = NextValue();
			return value != null ? (ushort?)Convert.ToUInt16(value) : null;
		}
		public ushort?[] NextNullableUInt16Array() {
			return NextArray().AllNullableUInt16();
		}
		public ushort?[] AllNullableUInt16() {
			return _arr.Children().Select(x => ((JValue)x).Value != null ? (ushort?)Convert.ToUInt16(((JValue)x).Value) : null).ToArray();
		}
		public ushort?[] RestNullableUInt16() {
			var res = new List<ushort?>();
			while (_arrI.MoveNext()) {
				// ReSharper disable PossibleNullReferenceException
				res.Add(((JValue)_arrI.Current).Value != null ? (ushort?)Convert.ToUInt16(((JValue)_arrI.Current).Value) : null);
				// ReSharper restore PossibleNullReferenceException
			}
			return res.ToArray();
		}
		public int[] NextInt32Array() {
			return NextArray().AllInt32();
		}
		public int NextInt32() {
			return Convert.ToInt32(NextValue());
		}
		public int[] AllInt32() {
			return _arr.Children().Select(x => Convert.ToInt32(((JValue)x).Value)).ToArray();
		}
		public int[] RestInt32() {
			var res = new List<int>();
			while (_arrI.MoveNext()) {
				// ReSharper disable once PossibleNullReferenceException
				res.Add(Convert.ToInt32(((JValue)_arrI.Current).Value));
			}
			return res.ToArray();
		}
		public int? NextNullableInt32() {
			object value = NextValue();
			return value != null ? (int?)Convert.ToInt32(value) : null;
		}
		public int?[] NextNullableInt32Array() {
			return NextArray().AllNullableInt32();
		}
		public int?[] AllNullableInt32() {
			return _arr.Children().Select(x => ((JValue)x).Value != null ? (int?)Convert.ToInt32(((JValue)x).Value) : null).ToArray();
		}
		public int?[] RestNullableInt32() {
			var res = new List<int?>();
			while (_arrI.MoveNext()) {
				// ReSharper disable PossibleNullReferenceException
				res.Add(((JValue)_arrI.Current).Value != null ? (int?)Convert.ToInt32(((JValue)_arrI.Current).Value) : null);
				// ReSharper restore PossibleNullReferenceException
			}
			return res.ToArray();
		}
		public uint[] NextUInt32Array() {
			return NextArray().AllUInt32();
		}
		public uint NextUInt32() {
			return Convert.ToUInt32(NextValue());
		}
		public uint[] AllUInt32() {
			return _arr.Children().Select(x => Convert.ToUInt32(((JValue)x).Value)).ToArray();
		}
		public uint[] RestUInt32() {
			var res = new List<uint>();
			while (_arrI.MoveNext()) {
				// ReSharper disable once PossibleNullReferenceException
				res.Add(Convert.ToUInt32(((JValue)_arrI.Current).Value));
			}
			return res.ToArray();
		}
		public uint? NextNullableUInt32() {
			object value = NextValue();
			return value != null ? (uint?)Convert.ToUInt32(value) : null;
		}
		public uint?[] NextNullableUInt32Array() {
			return NextArray().AllNullableUInt32();
		}
		public uint?[] AllNullableUInt32() {
			return _arr.Children().Select(x => ((JValue)x).Value != null ? (uint?)Convert.ToUInt32(((JValue)x).Value) : null).ToArray();
		}
		public uint?[] RestNullableUInt32() {
			var res = new List<uint?>();
			while (_arrI.MoveNext()) {
				// ReSharper disable PossibleNullReferenceException
				res.Add(((JValue)_arrI.Current).Value != null ? (uint?)Convert.ToUInt32(((JValue)_arrI.Current).Value) : null);
				// ReSharper restore PossibleNullReferenceException
			}
			return res.ToArray();
		}
		public long[] NextInt64Array() {
			return NextArray().AllInt64();
		}
		public long NextInt64() {
			return Convert.ToInt64(NextValue());
		}
		public long[] AllInt64() {
			return _arr.Children().Select(x => Convert.ToInt64(((JValue)x).Value)).ToArray();
		}
		public long[] RestInt64() {
			var res = new List<long>();
			while (_arrI.MoveNext()) {
				// ReSharper disable once PossibleNullReferenceException
				res.Add(Convert.ToInt64(((JValue)_arrI.Current).Value));
			}
			return res.ToArray();
		}
		public long? NextNullableInt64() {
			object value = NextValue();
			return value != null ? (long?)Convert.ToInt64(value) : null;
		}
		public long?[] NextNullableInt64Array() {
			return NextArray().AllNullableInt64();
		}
		public long?[] AllNullableInt64() {
			return _arr.Children().Select(x => ((JValue)x).Value != null ? (long?)Convert.ToInt64(((JValue)x).Value) : null).ToArray();
		}
		public long?[] RestNullableInt64() {
			var res = new List<long?>();
			while (_arrI.MoveNext()) {
				// ReSharper disable PossibleNullReferenceException
				res.Add(((JValue)_arrI.Current).Value != null ? (long?)Convert.ToInt64(((JValue)_arrI.Current).Value) : null);
				// ReSharper restore PossibleNullReferenceException
			}
			return res.ToArray();
		}
		public ulong[] NextUInt64Array() {
			return NextArray().AllUInt64();
		}
		public ulong NextUInt64() {
			return Convert.ToUInt64(NextValue());
		}
		public ulong[] AllUInt64() {
			return _arr.Children().Select(x => Convert.ToUInt64(((JValue)x).Value)).ToArray();
		}
		public ulong[] RestUInt64() {
			var res = new List<ulong>();
			while (_arrI.MoveNext()) {
				// ReSharper disable once PossibleNullReferenceException
				res.Add(Convert.ToUInt64(((JValue)_arrI.Current).Value));
			}
			return res.ToArray();
		}
		public ulong? NextNullableUInt64() {
			object value = NextValue();
			return value != null ? (ulong?)Convert.ToUInt64(value) : null;
		}
		public ulong?[] NextNullableUInt64Array() {
			return NextArray().AllNullableUInt64();
		}
		public ulong?[] AllNullableUInt64() {
			return _arr.Children().Select(x => ((JValue)x).Value != null ? (ulong?)Convert.ToUInt64(((JValue)x).Value) : null).ToArray();
		}
		public ulong?[] RestNullableUInt64() {
			var res = new List<ulong?>();
			while (_arrI.MoveNext()) {
				// ReSharper disable PossibleNullReferenceException
				res.Add(((JValue)_arrI.Current).Value != null ? (ulong?)Convert.ToUInt64(((JValue)_arrI.Current).Value) : null);
				// ReSharper restore PossibleNullReferenceException
			}
			return res.ToArray();
		}
		public float[] NextSingleArray() {
			return NextArray().AllSingle();
		}
		public float NextSingle() {
			return Convert.ToSingle(NextValue());
		}
		public float[] AllSingle() {
			return _arr.Children().Select(x => Convert.ToSingle(((JValue)x).Value)).ToArray();
		}
		public float[] RestSingle() {
			var res = new List<float>();
			while (_arrI.MoveNext()) {
				// ReSharper disable once PossibleNullReferenceException
				res.Add(Convert.ToSingle(((JValue)_arrI.Current).Value));
			}
			return res.ToArray();
		}
		public float? NextNullableSingle() {
			object value = NextValue();
			return value != null ? (float?)Convert.ToSingle(value) : null;
		}
		public float?[] NextNullableSingleArray() {
			return NextArray().AllNullableSingle();
		}
		public float?[] AllNullableSingle() {
			return _arr.Children().Select(x => ((JValue)x).Value != null ? (float?)Convert.ToSingle(((JValue)x).Value) : null).ToArray();
		}
		public float?[] RestNullableSingle() {
			var res = new List<float?>();
			while (_arrI.MoveNext()) {
				// ReSharper disable PossibleNullReferenceException
				res.Add(((JValue)_arrI.Current).Value != null ? (float?)Convert.ToSingle(((JValue)_arrI.Current).Value) : null);
				// ReSharper restore PossibleNullReferenceException
			}
			return res.ToArray();
		}
		public double[] NextDoubleArray() {
			return NextArray().AllDouble();
		}
		public double NextDouble() {
			return Convert.ToDouble(NextValue());
		}
		public double[] AllDouble() {
			return _arr.Children().Select(x => Convert.ToDouble(((JValue)x).Value)).ToArray();
		}
		public double[] RestDouble() {
			var res = new List<double>();
			while (_arrI.MoveNext()) {
				// ReSharper disable once PossibleNullReferenceException
				res.Add(Convert.ToDouble(((JValue)_arrI.Current).Value));
			}
			return res.ToArray();
		}
		public double? NextNullableDouble() {
			object value = NextValue();
			return value != null ? (double?)Convert.ToDouble(value) : null;
		}
		public double?[] NextNullableDoubleArray() {
			return NextArray().AllNullableDouble();
		}
		public double?[] AllNullableDouble() {
			return _arr.Children().Select(x => ((JValue)x).Value != null ? (double?)Convert.ToDouble(((JValue)x).Value) : null).ToArray();
		}
		public double?[] RestNullableDouble() {
			var res = new List<double?>();
			while (_arrI.MoveNext()) {
				// ReSharper disable PossibleNullReferenceException
				res.Add(((JValue)_arrI.Current).Value != null ? (double?)Convert.ToDouble(((JValue)_arrI.Current).Value) : null);
				// ReSharper restore PossibleNullReferenceException
			}
			return res.ToArray();
		}
		public decimal[] NextDecimalArray() {
			return NextArray().AllDecimal();
		}
		public decimal NextDecimal() {
			return Convert.ToDecimal(NextValue());
		}
		public decimal[] AllDecimal() {
			return _arr.Children().Select(x => Convert.ToDecimal(((JValue)x).Value)).ToArray();
		}
		public decimal[] RestDecimal() {
			var res = new List<decimal>();
			while (_arrI.MoveNext()) {
				// ReSharper disable once PossibleNullReferenceException
				res.Add(Convert.ToDecimal(((JValue)_arrI.Current).Value));
			}
			return res.ToArray();
		}
		public decimal? NextNullableDecimal() {
			object value = NextValue();
			return value != null ? (decimal?)Convert.ToDecimal(value) : null;
		}
		public decimal?[] NextNullableDecimalArray() {
			return NextArray().AllNullableDecimal();
		}
		public decimal?[] AllNullableDecimal() {
			return _arr.Children().Select(x => ((JValue)x).Value != null ? (decimal?)Convert.ToDecimal(((JValue)x).Value) : null).ToArray();
		}
		public decimal?[] RestNullableDecimal() {
			var res = new List<decimal?>();
			while (_arrI.MoveNext()) {
				// ReSharper disable PossibleNullReferenceException
				res.Add(((JValue)_arrI.Current).Value != null ? (decimal?)Convert.ToDecimal(((JValue)_arrI.Current).Value) : null);
				// ReSharper restore PossibleNullReferenceException
			}
			return res.ToArray();
		}
		public char[] NextCharArray() {
			return NextArray().AllChar();
		}
		public char NextChar() {
			return Convert.ToChar(NextValue());
		}
		public char[] AllChar() {
			return _arr.Children().Select(x => Convert.ToChar(((JValue)x).Value)).ToArray();
		}
		public char[] RestChar() {
			var res = new List<char>();
			while (_arrI.MoveNext()) {
				// ReSharper disable once PossibleNullReferenceException
				res.Add(Convert.ToChar(((JValue)_arrI.Current).Value));
			}
			return res.ToArray();
		}
		public char? NextNullableChar() {
			object value = NextValue();
			return value != null ? (char?)Convert.ToChar(value) : null;
		}
		public char?[] NextNullableCharArray() {
			return NextArray().AllNullableChar();
		}
		public char?[] AllNullableChar() {
			return _arr.Children().Select(x => ((JValue)x).Value != null ? (char?)Convert.ToChar(((JValue)x).Value) : null).ToArray();
		}
		public char?[] RestNullableChar() {
			var res = new List<char?>();
			while (_arrI.MoveNext()) {
				// ReSharper disable PossibleNullReferenceException
				res.Add(((JValue)_arrI.Current).Value != null ? (char?)Convert.ToChar(((JValue)_arrI.Current).Value) : null);
				// ReSharper restore PossibleNullReferenceException
			}
			return res.ToArray();
		}
		public string[] NextStringArray() {
			return NextArray().AllString();
		}
		public string NextString() {
			object value = NextValue();
			if (value == null) {
				throw new UnexpectedNullException("String value cannot be null.");
			}
			return Convert.ToString(value);
		}
		public string[] AllString() {
			return _arr.Children().Select(x => {
				if (((JValue)x).Value == null) {
					throw new UnexpectedNullException("String value cannot be null.");
				}
				return Convert.ToString(((JValue)x).Value);
			}).ToArray();
		}
		public string[] RestString() {
			var res = new List<string>();
			while (_arrI.MoveNext()) {
				// ReSharper disable PossibleNullReferenceException
				if (((JValue)_arrI.Current).Value == null) {
					throw new UnexpectedNullException("String value cannot be null.");
				}
				res.Add(Convert.ToString(((JValue)_arrI.Current).Value));
				// ReSharper restore PossibleNullReferenceException
			}
			return res.ToArray();
		}
		[CanBeNull]
		public string NextNullableString() {
			object value = NextValue();
			return value != null ? Convert.ToString(value) : null;
		}
		[ItemCanBeNull]
		public string[] NextNullableStringArray() {
			return NextArray().AllNullableString();
		}
		[ItemCanBeNull]
		public string[] AllNullableString() {
			return _arr.Children().Select(x => ((JValue)x).Value != null ? Convert.ToString(((JValue)x).Value) : null).ToArray();
		}
		[ItemCanBeNull]
		public string[] RestNullableString() {
			var res = new List<string>();
			while (_arrI.MoveNext()) {
				// ReSharper disable PossibleNullReferenceException
				res.Add(((JValue)_arrI.Current).Value != null ? Convert.ToString(((JValue)_arrI.Current).Value) : null);
				// ReSharper restore PossibleNullReferenceException
			}
			return res.ToArray();
		}
	}

	public partial class JsonObjectReader {
		public bool ReadBoolean(string key) {
			return Convert.ToBoolean(GetVal(key));
		}
		public bool[] ReadBooleanArray(string key) {
			return ReadArray(key).AllBoolean();
		}
		public bool? ReadNullableBoolean(string key) {
			object value = GetVal(key);
			return value != null ? (bool?)Convert.ToBoolean(value) : null;
		}
		public bool?[] ReadNullableBooleanArray(string key) {
			return ReadArray(key).AllNullableBoolean();
		}
		public byte ReadByte(string key) {
			return Convert.ToByte(GetVal(key));
		}
		public byte[] ReadByteArray(string key) {
			return ReadArray(key).AllByte();
		}
		public byte? ReadNullableByte(string key) {
			object value = GetVal(key);
			return value != null ? (byte?)Convert.ToByte(value) : null;
		}
		public byte?[] ReadNullableByteArray(string key) {
			return ReadArray(key).AllNullableByte();
		}
		public sbyte ReadSByte(string key) {
			return Convert.ToSByte(GetVal(key));
		}
		public sbyte[] ReadSByteArray(string key) {
			return ReadArray(key).AllSByte();
		}
		public sbyte? ReadNullableSByte(string key) {
			object value = GetVal(key);
			return value != null ? (sbyte?)Convert.ToSByte(value) : null;
		}
		public sbyte?[] ReadNullableSByteArray(string key) {
			return ReadArray(key).AllNullableSByte();
		}
		public short ReadInt16(string key) {
			return Convert.ToInt16(GetVal(key));
		}
		public short[] ReadInt16Array(string key) {
			return ReadArray(key).AllInt16();
		}
		public short? ReadNullableInt16(string key) {
			object value = GetVal(key);
			return value != null ? (short?)Convert.ToInt16(value) : null;
		}
		public short?[] ReadNullableInt16Array(string key) {
			return ReadArray(key).AllNullableInt16();
		}
		public ushort ReadUInt16(string key) {
			return Convert.ToUInt16(GetVal(key));
		}
		public ushort[] ReadUInt16Array(string key) {
			return ReadArray(key).AllUInt16();
		}
		public ushort? ReadNullableUInt16(string key) {
			object value = GetVal(key);
			return value != null ? (ushort?)Convert.ToUInt16(value) : null;
		}
		public ushort?[] ReadNullableUInt16Array(string key) {
			return ReadArray(key).AllNullableUInt16();
		}
		public int ReadInt32(string key) {
			return Convert.ToInt32(GetVal(key));
		}
		public int[] ReadInt32Array(string key) {
			return ReadArray(key).AllInt32();
		}
		public int? ReadNullableInt32(string key) {
			object value = GetVal(key);
			return value != null ? (int?)Convert.ToInt32(value) : null;
		}
		public int?[] ReadNullableInt32Array(string key) {
			return ReadArray(key).AllNullableInt32();
		}
		public uint ReadUInt32(string key) {
			return Convert.ToUInt32(GetVal(key));
		}
		public uint[] ReadUInt32Array(string key) {
			return ReadArray(key).AllUInt32();
		}
		public uint? ReadNullableUInt32(string key) {
			object value = GetVal(key);
			return value != null ? (uint?)Convert.ToUInt32(value) : null;
		}
		public uint?[] ReadNullableUInt32Array(string key) {
			return ReadArray(key).AllNullableUInt32();
		}
		public long ReadInt64(string key) {
			return Convert.ToInt64(GetVal(key));
		}
		public long[] ReadInt64Array(string key) {
			return ReadArray(key).AllInt64();
		}
		public long? ReadNullableInt64(string key) {
			object value = GetVal(key);
			return value != null ? (long?)Convert.ToInt64(value) : null;
		}
		public long?[] ReadNullableInt64Array(string key) {
			return ReadArray(key).AllNullableInt64();
		}
		public ulong ReadUInt64(string key) {
			return Convert.ToUInt64(GetVal(key));
		}
		public ulong[] ReadUInt64Array(string key) {
			return ReadArray(key).AllUInt64();
		}
		public ulong? ReadNullableUInt64(string key) {
			object value = GetVal(key);
			return value != null ? (ulong?)Convert.ToUInt64(value) : null;
		}
		public ulong?[] ReadNullableUInt64Array(string key) {
			return ReadArray(key).AllNullableUInt64();
		}
		public float ReadSingle(string key) {
			return Convert.ToSingle(GetVal(key));
		}
		public float[] ReadSingleArray(string key) {
			return ReadArray(key).AllSingle();
		}
		public float? ReadNullableSingle(string key) {
			object value = GetVal(key);
			return value != null ? (float?)Convert.ToSingle(value) : null;
		}
		public float?[] ReadNullableSingleArray(string key) {
			return ReadArray(key).AllNullableSingle();
		}
		public double ReadDouble(string key) {
			return Convert.ToDouble(GetVal(key));
		}
		public double[] ReadDoubleArray(string key) {
			return ReadArray(key).AllDouble();
		}
		public double? ReadNullableDouble(string key) {
			object value = GetVal(key);
			return value != null ? (double?)Convert.ToDouble(value) : null;
		}
		public double?[] ReadNullableDoubleArray(string key) {
			return ReadArray(key).AllNullableDouble();
		}
		public decimal ReadDecimal(string key) {
			return Convert.ToDecimal(GetVal(key));
		}
		public decimal[] ReadDecimalArray(string key) {
			return ReadArray(key).AllDecimal();
		}
		public decimal? ReadNullableDecimal(string key) {
			object value = GetVal(key);
			return value != null ? (decimal?)Convert.ToDecimal(value) : null;
		}
		public decimal?[] ReadNullableDecimalArray(string key) {
			return ReadArray(key).AllNullableDecimal();
		}
		public char ReadChar(string key) {
			return Convert.ToChar(GetVal(key));
		}
		public char[] ReadCharArray(string key) {
			return ReadArray(key).AllChar();
		}
		public char? ReadNullableChar(string key) {
			object value = GetVal(key);
			return value != null ? (char?)Convert.ToChar(value) : null;
		}
		public char?[] ReadNullableCharArray(string key) {
			return ReadArray(key).AllNullableChar();
		}
		public string ReadString(string key) {
			object value = GetVal(key);
			if (value == null) {
				throw new UnexpectedNullException("String value cannot be null.");
			}
			return Convert.ToString(value);
		}
		public string[] ReadStringArray(string key) {
			return ReadArray(key).AllString();
		}
		[CanBeNull]
		public string ReadNullableString(string key) {
			object value = GetVal(key);
			return value != null ? Convert.ToString(value) : null;
		}
		[ItemCanBeNull]
		public string[] ReadNullableStringArray(string key) {
			return ReadArray(key).AllNullableString();
		}
	}
}