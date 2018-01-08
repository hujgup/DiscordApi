using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Echo.Utils {
	/// <summary>
	/// Implemented by classes where resources cannot be automatically disposed in an object finalizer.
	/// </summary>
	public interface ICloseable {
		void Close();
	}

	/// <summary>
	/// Implemented by classes where resources cannot be automatically disposed in an object finalizer.
	/// </summary>
	/// <typeparam name="T">An argument required for closing to happen.</typeparam>
	public interface ICloseable<in T> {
		void Close(T arg);
	}
}
