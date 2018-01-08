using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Echo.Utils.Json {
	public partial class RootJsonObjectBuilder : JsonObjectBuilder<JObject> {
		public static JArray MakePrimitiveArray(IEnumerable<string> value, bool nullable) {
			var res = new JArray();
			foreach (string item in value) {
				JValue v;
				if (item is null) {
					if (nullable) {
						v = JValue.CreateNull();
					} else {
						throw new UnexpectedNullException("Nullable was false but a null IEnumerable element was found.");
					}
				} else {
					v = new JValue(item);
				}
				res.Add(v);
			}
			return res;
		}
		public override JObject Build() {
			return _obj;
		}
	}
}
