using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Echo.Utils.Async;
using Echo.Discord.Api.Json;
using Echo.Discord.Api.Servers;
using Echo.Discord.Api.Http;
using Echo.Discord.Api.Events;
using Echo.Utils;
using JetBrains.Annotations;

namespace Echo.Discord.Api.Channels {
	public partial class ChannelCategory : IServerChannel {
		internal static readonly BaseCache<ChannelCategory> _cache = new BaseCache<ChannelCategory>();
		internal ChannelCategory(ulong id, ulong ogId) {
			if (id == 0) {
				throw new NoSuchChannelException("No channel with ID " + ogId + " exists.");
			}
			Id = id;
		}
		internal static ChannelCategory CreateFromJson(ChannelJson json, [CanBeNull] object state) {
			return (ChannelCategory)ChannelUtils.CreateFromJson(json, state) ?? throw new ArgumentOutOfRangeException(nameof(json), "Incorrect JSON type.");
		}
		public static async Task<ChannelCategory> GetAsync(ulong categoryId, [CanBeNull] Client client) {
			return await FactoryUtils.GetChannelAsync(_cache, "ChannelCategory", null, json => new ChannelCategory(json.id, categoryId), Populate, categoryId, client, ChannelType.ServerCategory);
		}
		public static async Task<ChannelCategory> GetAsync(ulong categoryId) {
			return await GetAsync(categoryId, DiscordEnvironment.CurrentClient);
		}
		public static ChannelCategory Get(ulong categoryId, [CanBeNull] Client client) {
			return GetAsync(categoryId, client).Await();
		}
		public static ChannelCategory Get(ulong categoryId) {
			return GetAsync(categoryId).Await();
		}
		[CanBeNull]
		public static ChannelCategory GetCached(ulong categoryId) {
			return FactoryUtils.GetCached(_cache, categoryId);
		}
		public ulong Id {
			get;
		}
		public string Name {
			get;
			private set;
		}
		public ChannelType Type {
			get;
			private set;
		}
		public int Position {
			get;
			private set;
		}
		public PermissionMask Permissions {
			get;
			private set;
		}
		public ulong ServerId {
			get;
			private set;
		}
		public CachedPromise<Server> Server {
			get;
			private set;
		}
		internal static void Populate(ChannelCategory obj, ChannelJson json, [CanBeNull] object state) {
			obj.Name = json.name;
			obj.Type = json.type;
			obj.Position = (int)json.position;
			obj.Permissions = PermissionUtils.GetMask(json.permission_overwrites);
			obj.ServerId = state != null ? (ulong)state : (ulong)json.guild_id;
			obj.Server = new CachedPromise<Server>(Servers.Server._cache, obj.ServerId, Servers.Server.GetAsync);
		}
		public async Task PullUpdateAsync([CanBeNull] Client client) {
			client = client ?? DiscordEnvironment.CurrentClient;
			FactoryUtils.ValidateInEnv(client);
			await FactoryUtils.UpdateAsync(this, null, client.GetChannelJsonAsync, Populate);
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
			ChannelUtils.Uncache(_cache, this);
		}
		public override string ToString() {
			return Name;
		}
	}
}
