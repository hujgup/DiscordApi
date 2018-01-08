using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Echo.Utils.Async;
using Echo.Discord.Api.Http;
using Echo.Discord.Api.Servers;
using JetBrains.Annotations;

namespace Echo.Discord.Api.Messages {
	// ReSharper disable once InconsistentNaming (extension method provider)
	public static class IMentionableExtensions {
		public static async Task<bool> IsMentionableByAsync(this IMentionable obj, ulong userId, Server inServer, [CanBeNull] Client client) {
			return await obj.IsMentionableByAsync(userId, inServer.Id, client);
		}
		public static async Task<bool> IsMentionableByAsync(this IMentionable obj, LazyUser user, ulong inServerId, [CanBeNull] Client client) {
			return await obj.IsMentionableByAsync(user.Id, inServerId, client);
		}
		public static async Task<bool> IsMentionableByAsync(this IMentionable obj, LazyUser user, Server inServer, [CanBeNull] Client client) {
			return await obj.IsMentionableByAsync(user.Id, inServer.Id, client);
		}
		public static async Task<bool> IsMentionableByAsync(this IMentionable obj, ulong userId, ulong inServerId) {
			return await obj.IsMentionableByAsync(userId, inServerId, null);
		}
		public static async Task<bool> IsMentionableByAsync(this IMentionable obj, ulong userId, Server inServer) {
			return await obj.IsMentionableByAsync(userId, inServer.Id, null);
		}
		public static async Task<bool> IsMentionableByAsync(this IMentionable obj, LazyUser user, ulong inServerId) {
			return await obj.IsMentionableByAsync(user.Id, inServerId, null);
		}
		public static async Task<bool> IsMentionableByAsync(this IMentionable obj, LazyUser user, Server inServer) {
			return await obj.IsMentionableByAsync(user.Id, inServer.Id, null);
		}
		public static bool IsMentionableBy(this IMentionable obj, ulong userId, ulong inServerId, [CanBeNull] Client client) {
			return obj.IsMentionableByAsync(userId, inServerId, client).Await();
		}
		public static bool IsMentionableBy(this IMentionable obj, ulong userId, Server inServer, [CanBeNull] Client client) {
			return obj.IsMentionableByAsync(userId, inServer, client).Await();
		}
		public static bool IsMentionableBy(this IMentionable obj, LazyUser user, ulong inServerId, [CanBeNull] Client client) {
			return obj.IsMentionableByAsync(user, inServerId, client).Await();
		}
		public static bool IsMentionableBy(this IMentionable obj, LazyUser user, Server inServer, [CanBeNull] Client client) {
			return obj.IsMentionableByAsync(user, inServer, client).Await();
		}
		public static bool IsMentionableBy(this IMentionable obj, ulong userId, ulong inServerId) {
			return obj.IsMentionableByAsync(userId, inServerId).Await();
		}
		public static bool IsMentionableBy(this IMentionable obj, ulong userId, Server inServer) {
			return obj.IsMentionableByAsync(userId, inServer).Await();
		}
		public static bool IsMentionableBy(this IMentionable obj, LazyUser user, ulong inServerId) {
			return obj.IsMentionableByAsync(user, inServerId).Await();
		}
		public static bool IsMentionableBy(this IMentionable obj, LazyUser user, Server inServer) {
			return obj.IsMentionableByAsync(user, inServer).Await();
		}
	}
}
