using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Echo.Utils {
	public static class Validate {
		// ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Global
		[ContractAnnotation("obj: null => halt")]
		public static void NotNull([CanBeNull] object obj, [CanBeNull] string name) {
			if (obj is null) {
				throw new ArgumentNullException((name ?? "Variable") + " cannot be null.");
			}
		}
	}
}
