using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Echo.Utils.Async {
	public static class TaskExtensions {
		public static readonly string ExceptionMessage = "Async task threw an exception (HNmpKo7s5D8=HNmpKo7s5D8=).";
		public static T Await<T>(this Task<T> task) {
			Await((Task)task);
			return task.Result;
		}
		public static void Await(this Task task) {
			try {
				task.Start();
			} catch (InvalidOperationException) {
				// Was already started
			}
			try {
				task.Wait();
			} catch (AggregateException e) {
				// Throw correct exception type so we don't need to use if (e is SomeException) in catch blocks
				Debug.WriteLine(e.ToString());
				throw (Exception)Activator.CreateInstance(e.GetBaseException().GetType(), ExceptionMessage, e);
			}
		}
		public static Task<T> AsTask<T>(this T item) {
			return Task.Run(() => item);
		}
	}
}
