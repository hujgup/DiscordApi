using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Echo.Utils {
	public class BlockingQueue<T> {
		private readonly Queue<T> _q;
		private readonly Semaphore _s;
		private readonly SemaphoreSlim _a;
		public BlockingQueue(IEnumerable<T> collection) {
			_q = new Queue<T>(collection);
			_s = new Semaphore(_q.Count, int.MaxValue);
			_a = new SemaphoreSlim(1);
		}
		public BlockingQueue(int capacity) {
			_q = new Queue<T>(capacity);
			_s = new Semaphore(0, int.MaxValue);
			_a = new SemaphoreSlim(1);
		}
		public BlockingQueue() {
			_q = new Queue<T>();
			_s = new Semaphore(0, int.MaxValue);
			_a = new SemaphoreSlim(1);
		}
		public int Count {
			get => _q.Count;
		}
		public void Enqueue([CanBeNull] T item) {
			_a.Wait();
			try {
				_q.Enqueue(item);
				_s.Release();
			} finally {
				_a.Release();
			}
		}
		[CanBeNull]
		public T Dequeue() {
			_s.WaitOne();
			_a.Wait();
			try {
				return _q.Dequeue();
			} finally {
				_a.Release();
			}
		}
	}
}
