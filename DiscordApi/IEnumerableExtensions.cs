using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Echo.Discord.Api.Http;
using JetBrains.Annotations;

namespace Echo.Discord.Api {
	// ReSharper disable once InconsistentNaming (extension method provider)
	public static class IEnumerableExtensions {
		private static List<T> GetAllCachedActual<T>(this IEnumerable<CachedPromise<T>> promiseList, T exceptValue, bool throwExcept) where T : IIdentifiable {
			return promiseList.Select(x => {
				try {
					return x.GetCachedValue();
				} catch (ItemNotCachedException e) {
					if (throwExcept) {
						throw e;
						// ReSharper disable once RedundantIfElseBlock
					} else {
						return exceptValue;
					}
				}
			}).ToList();
		}
		internal static IReadOnlyDictionary<ulong, T> ToIdDic<T>([CanBeNull] this IEnumerable<T> list) where T : IIdentifiable {
			if (list == null) {
				throw new ArgumentNullException(nameof(list), "ToIdDic this arg cannot be null.");
			}
			return list.Where(x => x != null).ToDictionary(x => x.Id);
		}
		internal static IReadOnlyDictionary<ulong, CachedPromise<T>> ToIdDic<T>([CanBeNull] this IEnumerable<ulong> list, ICache<T> cache, Func<ulong, Client, Task<T>> networkGetter) where T : IIdentifiable {
			if (list == null) {
				throw new ArgumentNullException(nameof(list), "ToIdDic this arg cannot be null.");
			}
			return list.ToDictionary(x => x, x => new CachedPromise<T>(cache, x, networkGetter));
		}
		public static List<T> GetAllCached<T>(this IEnumerable<CachedPromise<T>> promiseList, T exceptValue) where T : IIdentifiable {
			return GetAllCachedActual(promiseList, exceptValue, false);
		}
		public static List<T> GetAllCached<T>(this IEnumerable<CachedPromise<T>> promiseList) where T : IIdentifiable {
			return GetAllCachedActual(promiseList, default, true);
		}
	}
}
