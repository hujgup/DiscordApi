using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Echo.Utils;
using Echo.Utils.Async;
using Echo.Discord.Api.Json;
using Echo.Discord.Api.Http;
using Echo.Discord.Api.Servers;
using Echo.Discord.Api.Messages;
using JetBrains.Annotations;

namespace Echo.Discord.Api {
	public class ServerEmoji : LazyServerEmoji, ICached, IAlias, IHasCachedServer, IHasManyCachedUsableRoles, IHasCreator {
		private static readonly BaseCache<ServerEmoji> _cache = new BaseCache<ServerEmoji>();
		private ServerEmoji(ulong forServer, EmojiJson json, ulong ogId) : base(json.id, json.name) {
			if (json.id == 0) {
				throw new NoSuchEmojiException("No emoji with ID " + ogId + " exists on server " + forServer + ".");
			}
			ServerId = forServer;
			Server = new CachedPromise<Servers.Server>(Servers.Server._cache, ServerId, Servers.Server.GetAsync);
		}
		internal static ServerEmoji CreateFromJson(ulong serverId, EmojiJson json, [CanBeNull] object state) {
			return FactoryUtils.CreateFromJson(_cache, state, json2 => new ServerEmoji(serverId, json, 1), Populate, json);
		}
		public static async Task<ServerEmoji> GetAsync(ulong emojiId, ulong serverId, [CanBeNull] Client client) {
			return await FactoryUtils.GetAsync<ServerEmoji, EmojiJson, NoSuchEmojiException>(_cache, "Emoji", null, FactoryUtils.ResolveGetter<EmojiJson>(client, c => async id2 => await c.GetServerEmojiJsonAsync(id2, serverId)), json => new ServerEmoji(serverId, json, emojiId), Populate, emojiId);
		}
		public static async Task<ServerEmoji> GetAsync(ulong emojiId, Server server, [CanBeNull] Client client) {
			return await GetAsync(emojiId, server.Id, client);
		}
		public static async Task<ServerEmoji> GetAsync(ulong emojiId, ulong serverId) {
			return await GetAsync(emojiId, serverId, DiscordEnvironment.CurrentClient);
		}
		public static async Task<ServerEmoji> GetAsync(ulong emojiId, Server server) {
			return await GetAsync(emojiId, server.Id);
		}
		public static ServerEmoji Get(ulong emojiId, ulong serverId, [CanBeNull] Client client) {
			return GetAsync(emojiId, serverId, client).Await();
		}
		public static ServerEmoji Get(ulong emojiId, Server server, [CanBeNull] Client client) {
			return GetAsync(emojiId, server, client).Await();
		}
		public static ServerEmoji Get(ulong emojiId, ulong serverId) {
			return GetAsync(emojiId, serverId).Await();
		}
		public static ServerEmoji Get(ulong emojiId, Server server) {
			return GetAsync(emojiId, server).Await();
		}
		[CanBeNull]
		public static ServerEmoji GetCached(ulong emojiId) {
			return FactoryUtils.GetCached(_cache, emojiId);
		}
		private static void Populate(ServerEmoji obj, EmojiJson json, [CanBeNull] object state) {
			Emoji.Populate(obj, json);
			obj.UsableRoles = json.roles.ToIdDic(Role._cache, async (id, c) => await Role.GetAsync(id, obj.ServerId, c));
			obj.HasCreator = json.user != null;
			if (obj.HasCreator) {
				// ReSharper disable once PossibleNullReferenceException (checked already)
				obj.CreatorId = json.user.id;
				obj.Creator = User.CreateFromJson(json.user, obj.ServerId);
			} else {
				obj.CreatorId = 0;
			}
			obj.MustWrapInColons = json.require_colons;
			obj.IsManaged = json.managed;
		}
		public ulong ServerId {
			get;
		}
		public CachedPromise<Server> Server {
			get;
		}
		public IReadOnlyDictionary<ulong, CachedPromise<Role>> UsableRoles {
			get;
			private set;
		}
		public bool HasCreator {
			get;
			private set;
		}
		public ulong CreatorId {
			get;
			private set;
		}
		public User Creator {
			get;
			private set;
		}
		public bool MustWrapInColons {
			get;
			private set;
		}
		public bool IsManaged {
			get;
			private set;
		}
		private void ValidateHasCreator() {
			if (!HasCreator) {
				throw new NoSuchUserException("Cannot get an emoji's creator if the creator is unknown or does not exist.");
			}
		}
		public override Task<ServerEmoji> ToServerEmojiAsync(ulong serverId, [CanBeNull] Client client) {
			return this.AsTask();
		}
		public override Task<ServerEmoji> ToServerEmojiAsync(Server server, [CanBeNull] Client client) {
			return this.AsTask();
		}
		public override Task<ServerEmoji> ToServerEmojiAsync(ulong serverId) {
			return this.AsTask();
		}
		public override Task<ServerEmoji> ToServerEmojiAsync(Server server) {
			return this.AsTask();
		}
		public override ServerEmoji ToServerEmoji(ulong serverId, [CanBeNull] Client client) {
			return this;
		}
		public override ServerEmoji ToServerEmoji(Server server, [CanBeNull] Client client) {
			return this;
		}
		public override ServerEmoji ToServerEmoji(ulong serverId) {
			return this;
		}
		public override ServerEmoji ToServerEmoji(Server server) {
			return this;
		}
		public override ServerEmoji CachedServerEmoji() {
			return this;
		}
		public async Task PullUpdateAsync([CanBeNull] Client client) {
			client = client ?? DiscordEnvironment.CurrentClient;
			FactoryUtils.ValidateInEnv(client);
			await FactoryUtils.UpdateAsync(this, null, FactoryUtils.ResolveGetter<EmojiJson>(client, c2 => id => c2.GetServerEmojiJsonAsync(id, ServerId)), Populate);
		}
		public void PullUpdate([CanBeNull] Client client) {
			PullUpdateAsync(client).Await();
		}
		public async Task PullUpdateAsync() {
			await PullUpdateAsync(null);
		}
		public void PullUpdate() {
			PullUpdateAsync().Await();
		}
		public void Uncache() {
			FactoryUtils.Uncache(_cache, this);
		}
		public override string ToString() {
			string res = Name;
			if (MustWrapInColons) {
				res = ":" + res + ":";
			}
			return res;
		}
	}
}
