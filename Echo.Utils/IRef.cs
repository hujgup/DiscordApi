using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Echo.Utils {
	public interface IRef<T> : IConstRef<T> {
		[CanBeNull]
		new T Value {
			get;
			set;
		}
	}
}
