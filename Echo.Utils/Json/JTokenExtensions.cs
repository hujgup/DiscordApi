using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Echo.Utils.Json {
	public static class JTokenExtensions {
		public static JsonArrayReader GetReader(this JArray arr) {
			return new JsonArrayReader(arr);
		}
		public static JsonObjectReader GetReader(this JObject obj) {
			return new JsonObjectReader(obj);
		}
	}
}
