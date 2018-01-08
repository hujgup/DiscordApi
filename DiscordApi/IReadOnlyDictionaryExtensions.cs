using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Echo.Discord.Api {
	public static class IReadOnlyDictionaryExtensions {
		public static bool ContainsUser(this IReadOnlyDictionary<ulong, User> dic, LazyUser value) {
			return dic.ContainsKey(value.Id);
		}
	}
}
