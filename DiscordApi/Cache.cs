using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Echo.Discord.Api.Channels;

namespace Echo.Discord.Api {
	[SuppressMessage("ReSharper", "InconsistentNaming")]
	internal interface ICache<T> where T : IIdentifiable {
		T this[ulong id] {
			get;
		}
		TSync Mutex<TSync>(Func<TSync> f);
		Task<TSync> MutexAsync<TSync>(Func<Task<TSync>> f);
		void Mutex(Action f);
		Task MutexAsync(Func<Task> f);
		bool Contains(ulong id);
		bool Contains(T item);
		void Add(T item);
		void Remove(ulong id);
		void Remove(T item);
	}

	internal class BaseCache<T> : ICache<T> where T : IIdentifiable {
		private readonly SemaphoreSlim _sync;
		protected readonly Dictionary<ulong, T> _cache;
		public BaseCache() {
			_cache = new Dictionary<ulong, T>();
			_sync = new SemaphoreSlim(1);
		}
		public T this[ulong id] {
			get => _cache[id];
		}
		public TSync Mutex<TSync>(Func<TSync> f) {
			_sync.Wait();
			try {
				TSync res = f();
				return res;
			} finally {
				_sync.Release();
			}
		}
		public async Task<TSync> MutexAsync<TSync>(Func<Task<TSync>> f) {
			return await Mutex(f);
		}
		public void Mutex(Action f) {
			_sync.Wait();
			try {
				f();
			} finally {
				_sync.Release();
			}
		}
		public async Task MutexAsync(Func<Task> f) {
			await Mutex(f);
		}
		public bool Contains(ulong id) {
			return _cache.ContainsKey(id);
		}
		public bool Contains(T item) {
			return Contains(item.Id);
		}
		public virtual void Add(T item) {
			_cache.Add(item.Id, item);
		}
		public virtual void Remove(ulong id) {
			_cache.Remove(id);
		}
		public void Remove(T item) {
			Remove(item.Id);
		}
	}

	internal class AliasCache<TBase, TAlias> : ICache<TAlias> where TBase : IIdentifiable where TAlias : IIdentifiable {
		private readonly ICache<TBase> _cache;
		public AliasCache(ICache<TBase> cache) {
			_cache = cache;
		}
		public TAlias this[ulong id] {
			get => (TAlias)(object)_cache[id];
		}
		public TSync Mutex<TSync>(Func<TSync> f) {
			return _cache.Mutex(f);
		}
		public async Task<TSync> MutexAsync<TSync>(Func<Task<TSync>> f) {
			return await _cache.MutexAsync(f);
		}
		public void Mutex(Action f) {
			_cache.Mutex(f);
		}
		public async Task MutexAsync(Func<Task> f) {
			await _cache.MutexAsync(f);
		}
		public bool Contains(ulong id) {
			return _cache.Contains(id);
		}
		public bool Contains(TAlias item) {
			return _cache.Contains((TBase)(object)item);
		}
		public void Add(TAlias item) {
			_cache.Add((TBase)(object)item);
		}
		public void Remove(ulong id) {
			_cache.Remove(id);
		}
		public void Remove(TAlias item) {
			_cache.Remove((TBase)(object)item);
		}
	}

	internal class ChannelAliasCache<T> : AliasCache<T, IChannel> where T : IChannel {
		public ChannelAliasCache(ICache<T> cache) : base(cache) {
		}
	}
}
