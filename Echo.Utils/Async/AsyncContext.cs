using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Echo.Utils.Async {
	public static class AsyncContext {
		public static T Switch<T>(Func<Task<T>> runner) {
			return runner().Await();
		}
		public static void Switch(Func<Task> runner) {
			runner().Await();
		}
	}
}
