using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Echo.Utils.Json {
	public class RootJsonArrayBuilder : JsonArrayBuilder<JArray> {
		public override JArray Build() {
			return _arr;
		}
	}
}
