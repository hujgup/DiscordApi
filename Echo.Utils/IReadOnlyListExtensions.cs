using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Echo.Utils.Async;
using JetBrains.Annotations;

namespace Echo.Utils {
	public static class ReadOnlyListExtensions {
		public static int IndexOf<T>([ItemCanBeNull] this IReadOnlyList<T> list, [CanBeNull] T item) {
			int res = -1;
			int i = 0;
			foreach (T x in list) {
				if ((x == null && item == null) || (x != null && x.Equals(item))) {
					res = i;
					break;
				} else {
					i++;
				}
			}
			return res;
		}
	}
}
