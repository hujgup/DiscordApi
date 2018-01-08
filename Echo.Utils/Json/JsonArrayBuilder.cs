using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;

namespace Echo.Utils.Json {
	public abstract partial class JsonArrayBuilder<T> {
		// ReSharper disable once PossibleInfiniteInheritance
		private class ArrayItemJsonArrayBuilder : JsonArrayBuilder<JsonArrayBuilder<T>> {
			private readonly JsonArrayBuilder<T> _arrObj;
			public ArrayItemJsonArrayBuilder(JsonArrayBuilder<T> arr) : base() {
				_arrObj = arr;
			}
			public override JsonArrayBuilder<T> Build() {
				return _arrObj.AddArray(_arr);
			}
		}

		private class ArrayItemJsonObjectBuilder : JsonObjectBuilder<JsonArrayBuilder<T>> {
			private JsonArrayBuilder<T> _arr;
			public ArrayItemJsonObjectBuilder(JsonArrayBuilder<T> arr) : base() {
				_arr = arr;
			}
			public override JsonArrayBuilder<T> Build() {
				return _arr.AddObject(_obj);
			}
		}

		protected JArray _arr;
		protected JsonArrayBuilder() {
			_arr = new JArray();
		}
		public abstract T Build();
		public JsonArrayBuilder<T> AddString([CanBeNull] string value, bool nullable) {
			return value is null ? AddNull() : AddDynamic(value);
		}
		public JsonArrayBuilder<T> AddString(string value) {
			return AddString(value, false);
		}
		public JsonArrayBuilder<T> AddStringArray([ItemCanBeNull] IEnumerable<string> value, bool nullable) {
			return AddArray(RootJsonObjectBuilder.MakePrimitiveArray(value, nullable));
		}
		public JsonArrayBuilder<T> AddStringArray(IEnumerable<string> value) {
			return AddStringArray(value, false);
		}
		public JsonArrayBuilder<T> AddNull() {
			_arr.Add(JValue.CreateNull());
			return this;
		}
		public JsonArrayBuilder<T> AddDynamic(JToken value) {
			_arr.Add(value);
			return this;
		}
		public JsonArrayBuilder<T> AddDynamic([CanBeNull] object value) {
			return AddDynamic(value is null ? JValue.CreateNull() : JToken.FromObject(value));
		}
		public JsonArrayBuilder<T> AddPrimitive(JValue value) {
			return AddDynamic(value);
		}
		public JsonArrayBuilder<JsonArrayBuilder<T>> AddArray() {
			return new ArrayItemJsonArrayBuilder(this);
		}
		public JsonArrayBuilder<T> AddArray(JArray arr) {
			return AddDynamic(arr);
		}
		public JsonObjectBuilder<JsonArrayBuilder<T>> AddObject() {
			return new ArrayItemJsonObjectBuilder(this);
		}
		public JsonArrayBuilder<T> AddObject(JObject obj) {
			return AddDynamic(obj);
		}
	}
}
