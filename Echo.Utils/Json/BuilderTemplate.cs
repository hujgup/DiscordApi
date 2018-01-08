using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Echo.Utils.Json {
	public partial class RootJsonObjectBuilder {
		public static JArray MakeNullablePrimitiveArray<TArr>(IEnumerable<TArr?> value) where TArr : struct {
			var res = new JArray();
			foreach (TArr? item in value) {
				res.Add(item == null ? JValue.CreateNull() : new JValue(item));
			}
			return res;
		}
		public static JArray MakePrimitiveArray<TArr>(IEnumerable<TArr> value) where TArr : struct {
			var res = new JArray();
			foreach (TArr item in value) {
				res.Add(new JValue(item));
			}
			return res;
		}
	}

	public abstract partial class JsonObjectBuilder<T> {
		public JsonObjectBuilder<T> BooleanProperty(string key, bool value) {
			return DynamicProperty(key, value);
		}
		public JsonObjectBuilder<T> BooleanProperty(string key, bool? value) {
			return value is null ? NullProperty(key) : BooleanProperty(key, (bool)value);
		}
		public JsonObjectBuilder<T> BooleanArrayProperty(string key, IEnumerable<bool> value) {
			return ArrayProperty(key, RootJsonObjectBuilder.MakePrimitiveArray(value));
		}
		public JsonObjectBuilder<T> BooleanArrayProperty(string key, IEnumerable<bool?> value) {
			return ArrayProperty(key, RootJsonObjectBuilder.MakeNullablePrimitiveArray(value));
		}
		public JsonObjectBuilder<T> ByteProperty(string key, byte value) {
			return DynamicProperty(key, value);
		}
		public JsonObjectBuilder<T> ByteProperty(string key, byte? value) {
			return value is null ? NullProperty(key) : ByteProperty(key, (byte)value);
		}
		public JsonObjectBuilder<T> ByteArrayProperty(string key, IEnumerable<byte> value) {
			return ArrayProperty(key, RootJsonObjectBuilder.MakePrimitiveArray(value));
		}
		public JsonObjectBuilder<T> ByteArrayProperty(string key, IEnumerable<byte?> value) {
			return ArrayProperty(key, RootJsonObjectBuilder.MakeNullablePrimitiveArray(value));
		}
		public JsonObjectBuilder<T> SByteProperty(string key, sbyte value) {
			return DynamicProperty(key, value);
		}
		public JsonObjectBuilder<T> SByteProperty(string key, sbyte? value) {
			return value is null ? NullProperty(key) : SByteProperty(key, (sbyte)value);
		}
		public JsonObjectBuilder<T> SByteArrayProperty(string key, IEnumerable<sbyte> value) {
			return ArrayProperty(key, RootJsonObjectBuilder.MakePrimitiveArray(value));
		}
		public JsonObjectBuilder<T> SByteArrayProperty(string key, IEnumerable<sbyte?> value) {
			return ArrayProperty(key, RootJsonObjectBuilder.MakeNullablePrimitiveArray(value));
		}
		public JsonObjectBuilder<T> Int16Property(string key, short value) {
			return DynamicProperty(key, value);
		}
		public JsonObjectBuilder<T> Int16Property(string key, short? value) {
			return value is null ? NullProperty(key) : Int16Property(key, (short)value);
		}
		public JsonObjectBuilder<T> Int16ArrayProperty(string key, IEnumerable<short> value) {
			return ArrayProperty(key, RootJsonObjectBuilder.MakePrimitiveArray(value));
		}
		public JsonObjectBuilder<T> Int16ArrayProperty(string key, IEnumerable<short?> value) {
			return ArrayProperty(key, RootJsonObjectBuilder.MakeNullablePrimitiveArray(value));
		}
		public JsonObjectBuilder<T> UInt16Property(string key, ushort value) {
			return DynamicProperty(key, value);
		}
		public JsonObjectBuilder<T> UInt16Property(string key, ushort? value) {
			return value is null ? NullProperty(key) : UInt16Property(key, (ushort)value);
		}
		public JsonObjectBuilder<T> UInt16ArrayProperty(string key, IEnumerable<ushort> value) {
			return ArrayProperty(key, RootJsonObjectBuilder.MakePrimitiveArray(value));
		}
		public JsonObjectBuilder<T> UInt16ArrayProperty(string key, IEnumerable<ushort?> value) {
			return ArrayProperty(key, RootJsonObjectBuilder.MakeNullablePrimitiveArray(value));
		}
		public JsonObjectBuilder<T> Int32Property(string key, int value) {
			return DynamicProperty(key, value);
		}
		public JsonObjectBuilder<T> Int32Property(string key, int? value) {
			return value is null ? NullProperty(key) : Int32Property(key, (int)value);
		}
		public JsonObjectBuilder<T> Int32ArrayProperty(string key, IEnumerable<int> value) {
			return ArrayProperty(key, RootJsonObjectBuilder.MakePrimitiveArray(value));
		}
		public JsonObjectBuilder<T> Int32ArrayProperty(string key, IEnumerable<int?> value) {
			return ArrayProperty(key, RootJsonObjectBuilder.MakeNullablePrimitiveArray(value));
		}
		public JsonObjectBuilder<T> UInt32Property(string key, uint value) {
			return DynamicProperty(key, value);
		}
		public JsonObjectBuilder<T> UInt32Property(string key, uint? value) {
			return value is null ? NullProperty(key) : UInt32Property(key, (uint)value);
		}
		public JsonObjectBuilder<T> UInt32ArrayProperty(string key, IEnumerable<uint> value) {
			return ArrayProperty(key, RootJsonObjectBuilder.MakePrimitiveArray(value));
		}
		public JsonObjectBuilder<T> UInt32ArrayProperty(string key, IEnumerable<uint?> value) {
			return ArrayProperty(key, RootJsonObjectBuilder.MakeNullablePrimitiveArray(value));
		}
		public JsonObjectBuilder<T> Int64Property(string key, long value) {
			return DynamicProperty(key, value);
		}
		public JsonObjectBuilder<T> Int64Property(string key, long? value) {
			return value is null ? NullProperty(key) : Int64Property(key, (long)value);
		}
		public JsonObjectBuilder<T> Int64ArrayProperty(string key, IEnumerable<long> value) {
			return ArrayProperty(key, RootJsonObjectBuilder.MakePrimitiveArray(value));
		}
		public JsonObjectBuilder<T> Int64ArrayProperty(string key, IEnumerable<long?> value) {
			return ArrayProperty(key, RootJsonObjectBuilder.MakeNullablePrimitiveArray(value));
		}
		public JsonObjectBuilder<T> UInt64Property(string key, ulong value) {
			return DynamicProperty(key, value);
		}
		public JsonObjectBuilder<T> UInt64Property(string key, ulong? value) {
			return value is null ? NullProperty(key) : UInt64Property(key, (ulong)value);
		}
		public JsonObjectBuilder<T> UInt64ArrayProperty(string key, IEnumerable<ulong> value) {
			return ArrayProperty(key, RootJsonObjectBuilder.MakePrimitiveArray(value));
		}
		public JsonObjectBuilder<T> UInt64ArrayProperty(string key, IEnumerable<ulong?> value) {
			return ArrayProperty(key, RootJsonObjectBuilder.MakeNullablePrimitiveArray(value));
		}
		public JsonObjectBuilder<T> SingleProperty(string key, float value) {
			return DynamicProperty(key, value);
		}
		public JsonObjectBuilder<T> SingleProperty(string key, float? value) {
			return value is null ? NullProperty(key) : SingleProperty(key, (float)value);
		}
		public JsonObjectBuilder<T> SingleArrayProperty(string key, IEnumerable<float> value) {
			return ArrayProperty(key, RootJsonObjectBuilder.MakePrimitiveArray(value));
		}
		public JsonObjectBuilder<T> SingleArrayProperty(string key, IEnumerable<float?> value) {
			return ArrayProperty(key, RootJsonObjectBuilder.MakeNullablePrimitiveArray(value));
		}
		public JsonObjectBuilder<T> DoubleProperty(string key, double value) {
			return DynamicProperty(key, value);
		}
		public JsonObjectBuilder<T> DoubleProperty(string key, double? value) {
			return value is null ? NullProperty(key) : DoubleProperty(key, (double)value);
		}
		public JsonObjectBuilder<T> DoubleArrayProperty(string key, IEnumerable<double> value) {
			return ArrayProperty(key, RootJsonObjectBuilder.MakePrimitiveArray(value));
		}
		public JsonObjectBuilder<T> DoubleArrayProperty(string key, IEnumerable<double?> value) {
			return ArrayProperty(key, RootJsonObjectBuilder.MakeNullablePrimitiveArray(value));
		}
		public JsonObjectBuilder<T> DecimalProperty(string key, decimal value) {
			return DynamicProperty(key, value);
		}
		public JsonObjectBuilder<T> DecimalProperty(string key, decimal? value) {
			return value is null ? NullProperty(key) : DecimalProperty(key, (decimal)value);
		}
		public JsonObjectBuilder<T> DecimalArrayProperty(string key, IEnumerable<decimal> value) {
			return ArrayProperty(key, RootJsonObjectBuilder.MakePrimitiveArray(value));
		}
		public JsonObjectBuilder<T> DecimalArrayProperty(string key, IEnumerable<decimal?> value) {
			return ArrayProperty(key, RootJsonObjectBuilder.MakeNullablePrimitiveArray(value));
		}
		public JsonObjectBuilder<T> CharProperty(string key, char value) {
			return DynamicProperty(key, value);
		}
		public JsonObjectBuilder<T> CharProperty(string key, char? value) {
			return value is null ? NullProperty(key) : CharProperty(key, (char)value);
		}
		public JsonObjectBuilder<T> CharArrayProperty(string key, IEnumerable<char> value) {
			return ArrayProperty(key, RootJsonObjectBuilder.MakePrimitiveArray(value));
		}
		public JsonObjectBuilder<T> CharArrayProperty(string key, IEnumerable<char?> value) {
			return ArrayProperty(key, RootJsonObjectBuilder.MakeNullablePrimitiveArray(value));
		}
	}

	public abstract partial class JsonArrayBuilder<T> {
		public JsonArrayBuilder<T> AddBoolean(bool value) {
			return AddDynamic(value);
		}
		public JsonArrayBuilder<T> AddBoolean(bool? value) {
			return value is null ? AddNull() : AddBoolean((bool)value);
		}
		public JsonArrayBuilder<T> AddBooleanArray(IEnumerable<bool> value) {
			return AddArray(RootJsonObjectBuilder.MakePrimitiveArray(value));
		}
		public JsonArrayBuilder<T> AddBooleanArray(IEnumerable<bool?> value) {
			return AddArray(RootJsonObjectBuilder.MakeNullablePrimitiveArray(value));
		}
		public JsonArrayBuilder<T> AddByte(byte value) {
			return AddDynamic(value);
		}
		public JsonArrayBuilder<T> AddByte(byte? value) {
			return value is null ? AddNull() : AddByte((byte)value);
		}
		public JsonArrayBuilder<T> AddByteArray(IEnumerable<byte> value) {
			return AddArray(RootJsonObjectBuilder.MakePrimitiveArray(value));
		}
		public JsonArrayBuilder<T> AddByteArray(IEnumerable<byte?> value) {
			return AddArray(RootJsonObjectBuilder.MakeNullablePrimitiveArray(value));
		}
		public JsonArrayBuilder<T> AddSByte(sbyte value) {
			return AddDynamic(value);
		}
		public JsonArrayBuilder<T> AddSByte(sbyte? value) {
			return value is null ? AddNull() : AddSByte((sbyte)value);
		}
		public JsonArrayBuilder<T> AddSByteArray(IEnumerable<sbyte> value) {
			return AddArray(RootJsonObjectBuilder.MakePrimitiveArray(value));
		}
		public JsonArrayBuilder<T> AddSByteArray(IEnumerable<sbyte?> value) {
			return AddArray(RootJsonObjectBuilder.MakeNullablePrimitiveArray(value));
		}
		public JsonArrayBuilder<T> AddInt16(short value) {
			return AddDynamic(value);
		}
		public JsonArrayBuilder<T> AddInt16(short? value) {
			return value is null ? AddNull() : AddInt16((short)value);
		}
		public JsonArrayBuilder<T> AddInt16Array(IEnumerable<short> value) {
			return AddArray(RootJsonObjectBuilder.MakePrimitiveArray(value));
		}
		public JsonArrayBuilder<T> AddInt16Array(IEnumerable<short?> value) {
			return AddArray(RootJsonObjectBuilder.MakeNullablePrimitiveArray(value));
		}
		public JsonArrayBuilder<T> AddUInt16(ushort value) {
			return AddDynamic(value);
		}
		public JsonArrayBuilder<T> AddUInt16(ushort? value) {
			return value is null ? AddNull() : AddUInt16((ushort)value);
		}
		public JsonArrayBuilder<T> AddUInt16Array(IEnumerable<ushort> value) {
			return AddArray(RootJsonObjectBuilder.MakePrimitiveArray(value));
		}
		public JsonArrayBuilder<T> AddUInt16Array(IEnumerable<ushort?> value) {
			return AddArray(RootJsonObjectBuilder.MakeNullablePrimitiveArray(value));
		}
		public JsonArrayBuilder<T> AddInt32(int value) {
			return AddDynamic(value);
		}
		public JsonArrayBuilder<T> AddInt32(int? value) {
			return value is null ? AddNull() : AddInt32((int)value);
		}
		public JsonArrayBuilder<T> AddInt32Array(IEnumerable<int> value) {
			return AddArray(RootJsonObjectBuilder.MakePrimitiveArray(value));
		}
		public JsonArrayBuilder<T> AddInt32Array(IEnumerable<int?> value) {
			return AddArray(RootJsonObjectBuilder.MakeNullablePrimitiveArray(value));
		}
		public JsonArrayBuilder<T> AddUInt32(uint value) {
			return AddDynamic(value);
		}
		public JsonArrayBuilder<T> AddUInt32(uint? value) {
			return value is null ? AddNull() : AddUInt32((uint)value);
		}
		public JsonArrayBuilder<T> AddUInt32Array(IEnumerable<uint> value) {
			return AddArray(RootJsonObjectBuilder.MakePrimitiveArray(value));
		}
		public JsonArrayBuilder<T> AddUInt32Array(IEnumerable<uint?> value) {
			return AddArray(RootJsonObjectBuilder.MakeNullablePrimitiveArray(value));
		}
		public JsonArrayBuilder<T> AddInt64(long value) {
			return AddDynamic(value);
		}
		public JsonArrayBuilder<T> AddInt64(long? value) {
			return value is null ? AddNull() : AddInt64((long)value);
		}
		public JsonArrayBuilder<T> AddInt64Array(IEnumerable<long> value) {
			return AddArray(RootJsonObjectBuilder.MakePrimitiveArray(value));
		}
		public JsonArrayBuilder<T> AddInt64Array(IEnumerable<long?> value) {
			return AddArray(RootJsonObjectBuilder.MakeNullablePrimitiveArray(value));
		}
		public JsonArrayBuilder<T> AddUInt64(ulong value) {
			return AddDynamic(value);
		}
		public JsonArrayBuilder<T> AddUInt64(ulong? value) {
			return value is null ? AddNull() : AddUInt64((ulong)value);
		}
		public JsonArrayBuilder<T> AddUInt64Array(IEnumerable<ulong> value) {
			return AddArray(RootJsonObjectBuilder.MakePrimitiveArray(value));
		}
		public JsonArrayBuilder<T> AddUInt64Array(IEnumerable<ulong?> value) {
			return AddArray(RootJsonObjectBuilder.MakeNullablePrimitiveArray(value));
		}
		public JsonArrayBuilder<T> AddSingle(float value) {
			return AddDynamic(value);
		}
		public JsonArrayBuilder<T> AddSingle(float? value) {
			return value is null ? AddNull() : AddSingle((float)value);
		}
		public JsonArrayBuilder<T> AddSingleArray(IEnumerable<float> value) {
			return AddArray(RootJsonObjectBuilder.MakePrimitiveArray(value));
		}
		public JsonArrayBuilder<T> AddSingleArray(IEnumerable<float?> value) {
			return AddArray(RootJsonObjectBuilder.MakeNullablePrimitiveArray(value));
		}
		public JsonArrayBuilder<T> AddDouble(double value) {
			return AddDynamic(value);
		}
		public JsonArrayBuilder<T> AddDouble(double? value) {
			return value is null ? AddNull() : AddDouble((double)value);
		}
		public JsonArrayBuilder<T> AddDoubleArray(IEnumerable<double> value) {
			return AddArray(RootJsonObjectBuilder.MakePrimitiveArray(value));
		}
		public JsonArrayBuilder<T> AddDoubleArray(IEnumerable<double?> value) {
			return AddArray(RootJsonObjectBuilder.MakeNullablePrimitiveArray(value));
		}
		public JsonArrayBuilder<T> AddDecimal(decimal value) {
			return AddDynamic(value);
		}
		public JsonArrayBuilder<T> AddDecimal(decimal? value) {
			return value is null ? AddNull() : AddDecimal((decimal)value);
		}
		public JsonArrayBuilder<T> AddDecimalArray(IEnumerable<decimal> value) {
			return AddArray(RootJsonObjectBuilder.MakePrimitiveArray(value));
		}
		public JsonArrayBuilder<T> AddDecimalArray(IEnumerable<decimal?> value) {
			return AddArray(RootJsonObjectBuilder.MakeNullablePrimitiveArray(value));
		}
		public JsonArrayBuilder<T> AddChar(char value) {
			return AddDynamic(value);
		}
		public JsonArrayBuilder<T> AddChar(char? value) {
			return value is null ? AddNull() : AddChar((char)value);
		}
		public JsonArrayBuilder<T> AddCharArray(IEnumerable<char> value) {
			return AddArray(RootJsonObjectBuilder.MakePrimitiveArray(value));
		}
		public JsonArrayBuilder<T> AddCharArray(IEnumerable<char?> value) {
			return AddArray(RootJsonObjectBuilder.MakeNullablePrimitiveArray(value));
		}
	}
}