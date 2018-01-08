using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Echo.Utils;
using Echo.Utils.Async;
using Echo.Discord.Api.Json;
using Echo.Discord.Api.Http;
using Echo.Discord.Api.Messages;
using Echo.Discord.Api.Servers;
using JetBrains.Annotations;

namespace Echo.Discord.Api {
	public class Role : IEquatable<Role>, ICached, INamed, IMentionable, IAlias, IHasCachedServer {
		internal static readonly BaseCache<Role> _cache = new BaseCache<Role>();
		public const string EveryoneName = "@everyone";
		private Role(ulong serverId, RoleJson json) {
			ServerId = serverId;
			Server = new CachedPromise<Servers.Server>(Servers.Server._cache, ServerId, Servers.Server.GetAsync);
			Id = json.id;
			IsManaged = json.managed;
		}
		internal static Role CreateFromJson(ulong serverId, RoleJson json, [CanBeNull] object state) {
			return FactoryUtils.CreateFromJson(_cache, state, json2 => new Role(serverId, json2), Populate, json);
		}
		public static async Task<Role> GetAsync(ulong roleId, ulong serverId, [CanBeNull] Client client) {
			Server s = await Servers.Server.GetAsync(serverId, client);
			if (s.Roles.ContainsKey(roleId)) {
				return s.Roles[roleId];
			} else {
				throw new NoSuchRoleException("Role " + roleId + " either does not exist in server " + serverId + ", or said server does not exist.");
			}
		}
		public static async Task<Role> GetAsync(ulong roleId, Server server, [CanBeNull] Client client) {
			return await GetAsync(roleId, server.Id, client);
		}
		public static async Task<Role> GetAsync(ulong roleId, ulong serverId) {
			return await GetAsync(roleId, serverId, DiscordEnvironment.CurrentClient);
		}
		public static async Task<Role> GetAsync(ulong roleId, Server server) {
			return await GetAsync(roleId, server.Id);
		}
		public static Role Get(ulong roleId, ulong serverId, [CanBeNull] Client client) {
			return GetAsync(roleId, serverId, client).Await();
		}
		public static Role Get(ulong roleId, Server server, [CanBeNull] Client client) {
			return GetAsync(roleId, server, client).Await();
		}
		public static Role Get(ulong roleId, ulong serverId) {
			return GetAsync(roleId, serverId).Await();
		}
		public static Role Get(ulong roleId, Server server) {
			return GetAsync(roleId, server).Await();
		}
		[CanBeNull]
		public static Role GetCached(ulong roleId) {
			return FactoryUtils.GetCached(_cache, roleId);
		}
		public ulong ServerId {
			get;
		}
		public CachedPromise<Server> Server {
			get;
		}
		public ulong Id {
			get;
		}
		public bool IsManaged {
			get;
		}
		public string Name {
			get;
			private set;
		}
		public RoleColor Color {
			get;
			private set;
		}
		public bool IsUserBucket {
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
		public Ternary IsMentionable {
			get;
			private set;
		}
		public string MentionContent {
			get => MessageBuilder.MentionOpen + "@&" + Id + MessageBuilder.MentionClose;
		}
		string IMentionable.MentionFallbackName {
			get => Name;
		}
		private static void Populate(Role obj, RoleJson json, [CanBeNull] object state) {
			obj.Name = json.name;
			obj.Color = new RoleColor(json.color);
			obj.IsUserBucket = json.hoist;
			obj.Position = json.position;
			obj.Permissions = json.permissions;
			obj.IsMentionable = json.mentionable ? Ternary.Yes : Ternary.No;
		}
		public Task<bool> IsMentionableByAsync(ulong userId, ulong inServerId, [CanBeNull] Client client) {
			return (ServerId == inServerId && IsMentionable.IsTrue()).AsTask();
		}
		public async Task PullUpdateAsync([CanBeNull] Client client) {
			client = client ?? DiscordEnvironment.CurrentClient;
			FactoryUtils.ValidateInEnv(client);
			await (await Server.GetValueAsync(client)).PullUpdateAsync(client);
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
			return Name;
		}
		public bool Equals([CanBeNull] Role r) {
			return !(r is null) && ServerId == r.ServerId && Id == r.Id;
		}
		public override bool Equals([CanBeNull] object obj) {
			return obj is Role role && Equals(role);
		}
		public override int GetHashCode() {
			return unchecked((ServerId.GetHashCode()*397) ^ Id.GetHashCode());
		}
		public static bool operator ==([CanBeNull] Role a, [CanBeNull] Role b) {
			return a is null ? b is null : a.Equals(b);
		}
		public static bool operator !=([CanBeNull] Role a, [CanBeNull] Role b) {
			return !(a == b);
		}
	}
}
