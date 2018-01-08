using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Echo.Utils.Async;

namespace Echo.Utils {
	public class StrongSemaphore {
		private readonly Semaphore _mutex;
		private readonly ConcurrentQueue<TaskCompletionSource<bool>> _queue;
		public StrongSemaphore(int initialCount, int maxCount) {
			_mutex = new Semaphore(initialCount, maxCount);
			_queue = new ConcurrentQueue<TaskCompletionSource<bool>>();
		}
		public void WaitOne() {
			var source = new TaskCompletionSource<bool>();
			_queue.Enqueue(source);
			_mutex.WaitOne();
			if (_queue.TryDequeue(out TaskCompletionSource<bool> popped)) {
				popped.SetResult(true);
			}
		}
		public void Release(int releaseCount) {
			_mutex.Release(releaseCount);
		}
		public void Release() {
			_mutex.Release();
		}
	}
}
