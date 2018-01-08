using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Echo.Utils {
	// ReSharper disable once InconsistentNaming
	public static class IReadOnlyDictionaryExtensions {
		public static bool ContainsValue<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dic, [CanBeNull] TValue value) {
			return dic.Values.Contains(value);
		}
	}
}
