using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Echo.Utils {
	public partial class SharedRef<T> : IRef<T> {
		public SharedRef([CanBeNull] T value) {
			Value = value;
		}
		public T Value {
			get;
			set;
		}
	}
}
