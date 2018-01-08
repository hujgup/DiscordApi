using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;

namespace Echo.Utils.Json {
	public abstract partial class JsonObjectBuilder<T> {
		private class ObjectPropJsonArrayBuilder : JsonArrayBuilder<JsonObjectBuilder<T>> {
			private string _key;
			private JsonObjectBuilder<T> _obj;
			public ObjectPropJsonArrayBuilder(string key, JsonObjectBuilder<T> obj) {
				_key = key;
				_obj = obj;
			}
			public override JsonObjectBuilder<T> Build() {
				return _obj.ArrayProperty(_key, _arr);
			}
		}

		private class JsonObjectPropJsonObjectBuilder : JsonObjectBuilder<JsonObjectBuilder<T>> {
			private string _key;
			private JsonObjectBuilder<T> _bObj;
			public JsonObjectPropJsonObjectBuilder(string key, JsonObjectBuilder<T> obj) {
				_key = key;
				_bObj = obj;
			}
			public override JsonObjectBuilder<T> Build() {
				return _bObj.ObjectProperty(_key, _obj);
			}
		}

		protected JObject _obj;
		public JsonObjectBuilder() {
			_obj = new JObject();
		}
		public abstract T Build();
		public JsonObjectBuilder<T> StringProperty(string key, string value, bool nullable) {
			return value is null ? NullProperty(key) : DynamicProperty(key, value);
		}
		public JsonObjectBuilder<T> StringProperty(string key, string value) {
			return StringProperty(key, value, false);
		}
		public JsonObjectBuilder<T> StringArrayProperty(string key, IEnumerable<string> value, bool nullable) {
			return ArrayProperty(key, RootJsonObjectBuilder.MakePrimitiveArray(value, nullable));
		}
		public JsonObjectBuilder<T> StringArrayProperty(string key, IEnumerable<string> value) {
			return StringArrayProperty(key, value, false);
		}
		public JsonObjectBuilder<T> NullProperty(string key) {
			return DynamicProperty(key, JValue.CreateNull());
		}
		public JsonObjectBuilder<T> DynamicProperty(string key, [CanBeNull] JToken value) {
			_obj.Add(key, value ?? JValue.CreateNull());
			return this;
		}
		public JsonObjectBuilder<T> DynamicProperty(string key, object value) {
			return DynamicProperty(key, JToken.FromObject(value));
		}
		public JsonObjectBuilder<T> PrimitiveProperty(string key, JValue value) {
			return DynamicProperty(key, value);
		}
		public JsonArrayBuilder<JsonObjectBuilder<T>> ArrayProperty(string key) {
			return new ObjectPropJsonArrayBuilder(key, this);
		}
		public JsonObjectBuilder<T> ArrayProperty(string key, JArray arr) {
			return DynamicProperty(key, arr);
		}
		public JsonObjectBuilder<JsonObjectBuilder<T>> ObjectProperty(string key) {
			return new JsonObjectPropJsonObjectBuilder(key, this);
		}
		public JsonObjectBuilder<T> ObjectProperty(string key, JObject obj) {
			return DynamicProperty(key, obj);
		}
	}
}
