using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Echo.Utils {
	public partial class AliasRef<T> : IRef<T> {
		private readonly Func<T> _get;
		private readonly Action<T> _set;
		public AliasRef(Func<T> getter, Action<T> setter) {
			_get = getter;
			_set = setter;
		}
		public T Value {
			get => _get();
			set => _set(value);
		}
	}
}
