using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Echo.Utils;
using Echo.Utils.Async;
using Echo.Discord.Api.Http;
using JetBrains.Annotations;

namespace Echo.Discord.Api {
	public class CachedPromise<T> : Promise<Client, T> where T : IIdentifiable {
		private readonly ICache<T> _cache;
		private readonly Func<ulong, Client, Task<T>> _getter;
		private bool _hasValue;
		internal CachedPromise(ICache<T> cache, ulong valueId, Func<ulong, Client, Task<T>> networkGetter) {
			_cache = cache;
			ValueId = valueId;
			_getter = networkGetter;
			_hasValue = false;
		}
		public ulong ValueId {
			get;
		}
		public CachedPromise([CanBeNull] T result) : base(result) {
			_hasValue = true;
		}
		[ItemCanBeNull]
		private async Task<T> GetCachedValueAsync([CanBeNull] Client client, bool allowNetwork) {
			return _hasValue ? Value : await _cache.MutexAsync(async () => {
				T res;
				if (_cache.Contains(ValueId)) {
					res = _cache[ValueId];
				} else if (allowNetwork) {
					res = await _getter(ValueId, client);
				} else {
					throw new ItemNotCachedException("Cannot resolve promise because user invokation disallowed network connections and the promised item is not locally cached.");
				}
				_hasValue = true;
				return res;
			});
		}
		[ItemCanBeNull]
		protected override async Task<T> ResolveValueAsync([CanBeNull] Client client) {
			return await GetCachedValueAsync(client, true);
		}
		/// <summary>
		/// Gets the value this promise represents as long as it has already been cached.
		/// </summary>
		/// <returns></returns>
		/// <exception cref="ItemNotCachedException">Thrown iff the value is not cached.</exception>
		[CanBeNull]
		public T GetCachedValue() {
			Mutex.Wait();
			try {
				return GetCachedValueAsync(null, false).Await();
			} finally {
				Mutex.Release();
			}
		}
		public override string ToString() {
			return "Promise for " + ValueId;
		}
	}
}
