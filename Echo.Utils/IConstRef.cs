using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Echo.Utils {
	public interface IConstRef<T> : IEquatable<IConstRef<T>>, IEquatable<T> {
		[CanBeNull]
		T Value {
			get;
		}
	}
}
