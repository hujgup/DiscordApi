using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;

namespace Echo.Utils.Json {
	public partial class JsonArrayReader : IEnumerable<JToken>, IEnumerator<JToken> {
		private readonly JArray _arr;
		private readonly IEnumerator<JToken> _arrI;
		internal JsonArrayReader(JArray arr) {
			_arr = arr;
			_arrI = arr.Children().GetEnumerator();
		}
		public int Count {
			get => _arr.Count;
		}
		[NotNull]
		JToken IEnumerator<JToken>.Current {
			// ReSharper disable once AssignNullToNotNullAttribute
			get => _arrI.Current;
		}
		[NotNull]
		object IEnumerator.Current {
			// ReSharper disable once AssignNullToNotNullAttribute
			get => _arrI.Current;
		}
		[CanBeNull]
		public JToken Next() {
			bool hasNext = _arrI.MoveNext();
			return hasNext ? _arrI.Current : null;
		}
		[CanBeNull]
		public JValue NextValue() {
			return (JValue)Next();
		}
		public JsonArrayReader NextArray() {
			// ReSharper disable once AssignNullToNotNullAttribute
			return new JsonArrayReader((JArray)Next());
		}
		public JsonArrayReader[] NextNestedArray() {
			return NextArray().AllArray();
		}
		public JsonArrayReader[] AllArray() {
			return _arr.Children().Select(x => new JsonArrayReader((JArray)x)).ToArray();
		}
		public JsonArrayReader[] RestArray() {
			var res = new List<JsonArrayReader>();
			while (_arrI.MoveNext()) {
				// ReSharper disable once AssignNullToNotNullAttribute
				res.Add(new JsonArrayReader((JArray)_arrI.Current));
			}
			return res.ToArray();
		}
		public JsonArrayReader NextNullableArray() {
			JToken next = Next();
			// ReSharper disable AssignNullToNotNullAttribute
			return next is JValue value && value.Value == null ? null : new JsonArrayReader((JArray)next);
			// ReSharper restore AssignNullToNotNullAttribute
		}
		public JsonArrayReader[] NextNullableNestedArray() {
			return NextArray().AllNullableArray();
		}
		public JsonArrayReader[] AllNullableArray() {
			return _arr.Children().Select(x => x is JValue && ((JValue)x).Value == null ? null : new JsonArrayReader((JArray)x)).ToArray();
		}
		public JsonArrayReader[] RestNullableArray() {
			var res = new List<JsonArrayReader>();
			while (_arrI.MoveNext()) {
				// ReSharper disable once AssignNullToNotNullAttribute
				res.Add(_arrI.Current is JValue value && value.Value == null ? null : new JsonArrayReader((JArray)_arrI.Current));
			}
			return res.ToArray();
		}
		[CanBeNull]
		public object NextNull() {
			// ReSharper disable once PossibleNullReferenceException
			object value = NextValue().Value;
			if (value != null) {
				throw new NotNullException("Cannot read a non-null value as null.");
			}
			return null;
		}
		[ItemCanBeNull]
		public object[] NextNullArray() {
			return NextArray().AllNull();
		}
		[ItemCanBeNull]
		public object[] AllNull() {
			return _arr.Children().Select<JToken, object>(x => {
				object value = ((JValue)x).Value;
				if (value != null) {
					throw new NotNullException("Cannot read a non-null value as null.");
				}
				return null;
			}).ToArray();
		}
		[ItemCanBeNull]
		public object[] RestNull() {
			var res = new List<object>();
			while (_arrI.MoveNext()) {
				// ReSharper disable once PossibleNullReferenceException
				object value = ((JValue)_arrI.Current).Value;
				if (value != null) {
					throw new NotNullException("Cannot read a non-null value as null.");
				}
				res.Add(null);
			}
			return res.ToArray();
		}
		public ulong NextSnowflake() {
			return ulong.Parse(NextString());
		}
		public ulong[] NextSnowflakeArray() {
			return NextArray().AllSnowflake();
		}
		public ulong[] AllSnowflake() {
			return _arr.Children().Select(x => ulong.Parse((string)((JValue)x).Value)).ToArray();
		}
		public ulong[] RestSnowflake() {
			var res = new List<ulong>();
			while (_arrI.MoveNext()) {
				// ReSharper disable once PossibleNullReferenceException
				res.Add(ulong.Parse((string)((JValue)_arrI.Current).Value));
			}
			return res.ToArray();
		}
		public ulong? NextNullableSnowflake() {
			string sf = NextNullableString();
			return sf != null ? (ulong?)ulong.Parse(sf) : null;
		}
		public ulong?[] NextNullableSnowflakeArray() {
			return NextArray().AllNullableSnowflake();
		}
		public ulong?[] AllNullableSnowflake() {
			return _arr.Children().Select(x => ((JValue)x).Value != null ? (ulong?)ulong.Parse((string)((JValue)x).Value) : null).ToArray();
		}
		public ulong?[] RestNullableSnowflake() {
			var res = new List<ulong?>();
			while (_arrI.MoveNext()) {
				// ReSharper disable PossibleNullReferenceException
				res.Add(((JValue)_arrI.Current).Value != null ? (ulong?)ulong.Parse((string)((JValue)_arrI.Current).Value) : null);
				// ReSharper restore PossibleNullReferenceException
			}
			return res.ToArray();
		}
		public JsonObjectReader NextObject() {
			// ReSharper disable once AssignNullToNotNullAttribute
			return new JsonObjectReader((JObject)Next());
		}
		public JsonObjectReader[] NextObjectArray() {
			return NextArray().AllObject();
		}
		public JsonObjectReader[] AllObject() {
			return _arr.Children().Select(x => new JsonObjectReader((JObject)x)).ToArray();
		}
		public JsonObjectReader[] RestObject() {
			var res = new List<JsonObjectReader>();
			while (_arrI.MoveNext()) {
				// ReSharper disable once AssignNullToNotNullAttribute
				res.Add(new JsonObjectReader((JObject)_arrI.Current));
			}
			return res.ToArray();
		}
		[CanBeNull]
		public JsonObjectReader NextNullableObject() {
			JToken next = Next();
			// ReSharper disable AssignNullToNotNullAttribute
			return next is JValue value && value.Value == null ? null : new JsonObjectReader((JObject)next);
			// ReSharper restore AssignNullToNotNullAttribute
		}
		[ItemCanBeNull]
		public JsonObjectReader[] NextNullableObjectArray() {
			return NextArray().AllNullableObject();
		}
		[ItemCanBeNull]
		public JsonObjectReader[] AllNullableObject() {
			return _arr.Children().Select(x => x is JValue && ((JValue)x).Value == null ? null : new JsonObjectReader((JObject)x)).ToArray();
		}
		[ItemCanBeNull]
		public JsonObjectReader[] RestNullableObject() {
			var res = new List<JsonObjectReader>();
			while (_arrI.MoveNext()) {
				// ReSharper disable AssignNullToNotNullAttribute
				res.Add(_arrI.Current is JValue value && value.Value == null ? null : new JsonObjectReader((JObject)_arrI.Current));
				// ReSharper restore AssignNullToNotNullAttribute
			}
			return res.ToArray();
		}
		public T NextObject<T>() {
			// ReSharper disable once PossibleNullReferenceException
			return Next().ToObject<T>();
		}
		public T[] NextObjectArray<T>() {
			return NextArray().AllObject<T>();
		}
		public T[] AllObject<T>() {
			return _arr.Children().Select(x => x.ToObject<T>()).ToArray();
		}
		public T[] RestObject<T>() {
			var res = new List<T>();
			while (_arrI.MoveNext()) {
				// ReSharper disable once PossibleNullReferenceException
				res.Add(_arrI.Current.ToObject<T>());
			}
			return res.ToArray();
		}
		[CanBeNull]
		public T NextNullableObject<T>() {
			JToken next = Next();
			// ReSharper disable PossibleNullReferenceException
			return next is JValue value && value.Value == null ? default(T) : next.ToObject<T>();
			// ReSharper restore PossibleNullReferenceException
		}
		[ItemCanBeNull]
		public T[] NextNullableObjectArray<T>() {
			return NextArray().AllNullableObject<T>();
		}
		[ItemCanBeNull]
		public T[] AllNullableObject<T>() {
			return _arr.Children().Select(x => x is JValue && ((JValue)x).Value == null ? default(T) : x.ToObject<T>()).ToArray();
		}
		[ItemCanBeNull]
		public T[] RestNullableObject<T>() {
			var res = new List<T>();
			while (_arrI.MoveNext()) {
				res.Add(_arrI.Current is JValue && ((JValue)_arrI.Current).Value == null ? default(T) : _arrI.Current.ToObject<T>());
			}
			return res.ToArray();
		}
		public void Reset() {
			_arrI.Reset();
		}
		public IEnumerator<JToken> GetEnumerator() {
			return this;
		}
		void IDisposable.Dispose() {
			_arrI.Dispose();
		}
		bool IEnumerator.MoveNext() {
			return _arrI.MoveNext();
		}
		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}
	}
}
