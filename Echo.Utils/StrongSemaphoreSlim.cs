using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Echo.Utils.Async;

namespace Echo.Utils {
	public class StrongSemaphoreSlim {
		private readonly SemaphoreSlim _mutex;
		private readonly ConcurrentQueue<TaskCompletionSource<bool>> _queue;
		public StrongSemaphoreSlim(int initialCount, int maxCount) {
			_mutex = new SemaphoreSlim(initialCount, maxCount);
			_queue = new ConcurrentQueue<TaskCompletionSource<bool>>();
		}
		public StrongSemaphoreSlim(int initialCount) {
			_mutex = new SemaphoreSlim(initialCount);
			_queue = new ConcurrentQueue<TaskCompletionSource<bool>>();
		}
		// ReSharper disable once InconsistentNaming
		public Task WaitAsync() {
			var source = new TaskCompletionSource<bool>();
			_queue.Enqueue(source);
			_mutex.WaitAsync().ContinueWith(task => {
				if (_queue.TryDequeue(out TaskCompletionSource<bool> popped)) {
					popped.SetResult(true);
				}
			});
			return source.Task;
		}
		public void Wait() {
			WaitAsync().Await();
		}
		public void Release(int releaseCount) {
			_mutex.Release(releaseCount);
		}
		public void Release() {
			_mutex.Release();
		}
	}
}
