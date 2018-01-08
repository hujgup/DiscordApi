using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;

namespace Echo.Utils.Json {
	public partial class JsonObjectReader {
		private readonly JObject _obj;
		internal JsonObjectReader(JObject obj) {
			_obj = obj;
		}
		[CanBeNull]
		private object GetVal(string key) {
			return ReadValue(key).Value;
		}
		private JArray GetArr(string key) {
			return (JArray)Read(key);
		}
		public bool Contains(string key) {
			return _obj.GetValue(key) != null;
		}
		public JToken Read(string key) {
			return _obj.GetValue(key);
		}
		public JValue ReadValue(string key) {
			return (JValue)Read(key);
		}
		public JsonArrayReader ReadArray(string key) {
			return new JsonArrayReader(GetArr(key));
		}
		public JsonObjectReader ReadObject(string key) {
			return new JsonObjectReader((JObject)Read(key));
		}
		public T ReadObject<T>(string key) {
			return ((JObject)Read(key)).ToObject<T>();
		}
		public JsonObjectReader[] ReadObjectArray(string key) {
			return ReadArray(key).AllObject();
		}
		public T[] ReadObjectArray<T>(string key) {
			return ReadArray(key).AllObject<T>();
		}
		[CanBeNull]
		public object ReadNull(string key) {
			object value = ReadValue(key).Value;
			if (value != null) {
				throw new NotNullException("Cannot read a non-null value as null.");
			}
			return null;
		}
		[ItemCanBeNull]
		public object[] ReadNullArray(string key) {
			return ReadArray(key).AllNull();
		}
		public ulong ReadSnowflake(string key) {
			return ulong.Parse(ReadString(key));
		}
		public ulong[] ReadSnowflakeArray(string key) {
			return ReadArray(key).AllSnowflake();
		}
		public ulong? ReadNullableSnowflake(string key) {
			string sf = ReadNullableString(key);
			return sf != null ? (ulong?)ulong.Parse(sf) : null;
		}
		public ulong?[] ReadNullableSnowflakeArray(string key) {
			return ReadArray(key).AllNullableSnowflake();
		}
		public T ToObject<T>() {
			return _obj.ToObject<T>();
		}
	}
}
