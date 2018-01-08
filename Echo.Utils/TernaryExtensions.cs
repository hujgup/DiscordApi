using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Echo.Utils {
	public static class TernaryExtensions {
		public static bool IsTrue(this Ternary t) {
			return t == Ternary.Yes;
		}
		public static bool IsFalse(this Ternary t) {
			return t == Ternary.No;
		}
		public static bool? ToNullable(this Ternary t) {
			return t != Ternary.Maybe ? t.IsTrue() : (bool?)null;
		}
	}
}
