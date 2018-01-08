using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Echo.Utils {
	public static class TernaryUtils {
		public static Ternary FromNullable(bool? value) {
			switch (value) {
				case null:
					return Ternary.Maybe;
				case true:
					return Ternary.Yes;
				case false:
					return Ternary.No;
				default:
					throw new ArgumentOutOfRangeException(nameof(value), "Nullable bool can only be true, false, or null. If this has been thrown, something is very very wrong.");
			}
		}
	}
}
