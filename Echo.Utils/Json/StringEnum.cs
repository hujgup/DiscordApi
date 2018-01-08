using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Echo.Utils.Json {
	public static class StringEnum {
		public static string GetValue(Enum value) {
			Type t = value.GetType();
			MemberInfo i = t.GetMember(value.ToString())[0];
			var a = (EnumMemberAttribute)i.GetCustomAttribute(typeof(EnumMemberAttribute), false);
			return a.Value;
			//return ((JsonProperty)value.GetType().GetMember(value.ToString())[0].GetCustomAttributes(typeof(JsonProperty), false)[0]).ToString();
		}
	}
}
