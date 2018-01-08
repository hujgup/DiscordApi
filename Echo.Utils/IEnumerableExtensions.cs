using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Echo.Utils.Async;

namespace Echo.Utils {
	// ReSharper disable once InconsistentNaming
	public static class IEnumerableExtensions {
		public static async Task<List<T>> GetAllAsync<T>(this IEnumerable<IPromise<T>> promiseList) {
			int count = promiseList.Count();
			var res = new List<T>(count);
			var tasks = new Task<T>[count];
			int i = 0;
			foreach (IPromise<T> promise1 in promiseList) {
				var promise = (Promise<T>)promise1;
				tasks[i++] = promise.GetValueAsync();
			}
			i = 0;
			foreach (Task<T> task in tasks) {
				res[i] = await task;
			}
			return res;
		}
		public static List<T> GetAll<T>(this IEnumerable<IPromise<T>> promiseList) {
			return GetAllAsync(promiseList).Await();
		}
		public static async Task<List<TOut>> GetAllAsync<TData, TOut>(this IEnumerable<Promise<TData, TOut>> promiseList, TData data) {
			int count = promiseList.Count();
			var res = new List<TOut>(count);
			var tasks = new Task<TOut>[count];
			int i = 0;
			foreach (Promise<TData, TOut> promise in promiseList) {
				tasks[i++] = promise.GetValueAsync(data);
			}
			i = 0;
			foreach (Task<TOut> task in tasks) {
				res[i] = await task;
			}
			return res;
		}
		public static List<TOut> GetAll<TData, TOut>(this IEnumerable<Promise<TData, TOut>> promiseList, TData data) {
			return GetAllAsync(promiseList, data).Await();
		}
	}
}
