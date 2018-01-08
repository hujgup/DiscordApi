using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Echo.Utils.Async;
using JetBrains.Annotations;

namespace Echo.Utils {
	public class Promise<TData, TOut> : IPromise<TOut> {
		[CanBeNull]
		private readonly Func<TData, Task<TOut>> _resolver;
		private bool _hasValue;
		protected Promise() {
			_hasValue = false;
			Mutex = new SemaphoreSlim(1);
		}
		public Promise(Func<TData, Task<TOut>> resolver) : this() {
			_resolver = resolver;
		}
		public Promise(Func<TData, TOut> resolver) : this(arg => resolver(arg).AsTask()) {}
		public Promise(TOut result) : this() {
			Value = result;
			_hasValue = true;
		}
		protected TOut Value {
			get;
			private set;
		}
		protected SemaphoreSlim Mutex {
			get;
		}
		protected virtual async Task<TOut> ResolveValueAsync([CanBeNull] TData data) {
			// ReSharper disable once PossibleNullReferenceException (only case where _resolver is null is also case where _hasValue is true, this this method is never called)
			TOut res = await _resolver(data);
			_hasValue = true;
			return res;
		}
		public async Task<TOut> GetValueAsync([CanBeNull] TData data) {
			await Mutex.WaitAsync();
			try {
				if (!_hasValue) {
					Value = await ResolveValueAsync(data);
				}
				return Value;
			} finally {
				Mutex.Release();
			}
		}
		public async Task<TOut> GetValueAsync() {
			return await GetValueAsync(default(TData));
		}
		public TOut GetValue([CanBeNull] TData data) {
			return GetValueAsync(data).Await();
		}
		public TOut GetValue() {
			return GetValueAsync().Await();
		}
	}

	public class Promise<T> : IPromise<T> {
		private bool _hasValue;
		[CanBeNull]
		private readonly Func<Task<T>> _resolver;
		private readonly SemaphoreSlim _mutex;
		protected Promise() {
			_hasValue = false;
			_mutex = new SemaphoreSlim(1);
		}
		public Promise(Func<Task<T>> resolver) : this() {
			_resolver = resolver;
		}
		public Promise(Func<T> resolver) : this(() => resolver().AsTask()) { }
		public Promise(T result) : this() {
			Value = result;
			_hasValue = true;
		}
		protected T Value {
			get;
			private set;
		}
		protected virtual async Task<T> ResolveValueAsync() {
			// ReSharper disable once PossibleNullReferenceException (same logic as previous class)
			T res = await _resolver();
			_hasValue = true;
			return res;
		}
		public async Task<T> GetValueAsync() {
			await _mutex.WaitAsync();
			try {
				if (!_hasValue) {
					Value = await ResolveValueAsync();
				}
				return Value;
			} finally {
				_mutex.Release();
			}
		}
		public T GetValue() {
			return GetValueAsync().Await();
		}
	}

	public interface IPromise<T> {
		// ReSharper disable once InconsistentNaming
		Task<T> GetValueAsync();
		T GetValue();
	}
}
