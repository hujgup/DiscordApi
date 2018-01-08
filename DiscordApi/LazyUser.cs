using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Echo.Utils;
using Echo.Utils.Async;
using Echo.Discord.Api.Http;
using Echo.Discord.Api.Servers;
using Echo.Discord.Api.Json;
using Echo.Discord.Api.Messages;
using JetBrains.Annotations;

namespace Echo.Discord.Api {
	/// <summary>
	/// Provides access to server-specific user methods without having to initialize a <see cref="User"/> object.
	/// </summary>
	public class LazyUser : IEquatable<LazyUser>, IIdentifiable, IServerNicknamed, IMentionable {
		public class ServerData : IHasManyCachedRoles, IHasCachedRelatedUser, IHasCachedServer, INicknamed {
			internal ServerData(ulong forUser, ulong inServer, ServerUserJson json) {
				RelatedUserId = forUser;
				RelatedUser = new CachedPromise<User>(User._cache, forUser, User.GetAsync);
				LazyRelatedUser = LazyUser.Get(forUser);
				ServerId = inServer;
				Server = new CachedPromise<Servers.Server>(Servers.Server._cache, inServer, Servers.Server.GetAsync);
				Nickname = json.nick;
				Roles = json.roles.Select(id => new CachedPromise<Role>(Role._cache, id, async (id2, client) => await Role.GetAsync(id2, inServer, client))).ToDictionary(cp => cp.ValueId);
				UserJoinTime = DateTime.Parse(json.joined_at);
				IsDeafened = json.deaf;
				IsMuted = json.mute;
			}
			public ulong RelatedUserId {
				get;
			}
			public CachedPromise<User> RelatedUser {
				get;
			}
			public LazyUser LazyRelatedUser {
				get;
			}
			public ulong ServerId {
				get;
			}
			public CachedPromise<Server> Server {
				get;
			}
			public bool HasNickname {
				get => Nickname != null;
			}
			[CanBeNull]
			public string Nickname {
				get;
			}
			public IReadOnlyDictionary<ulong, CachedPromise<Role>> Roles {
				get;
			}
			public DateTime UserJoinTime {
				get;
			}
			public bool IsDeafened {
				get;
			}
			public bool IsMuted {
				get;
			}
		}

		private static readonly Dictionary<ulong, Dictionary<ulong, ServerData>> _serverData = new Dictionary<ulong, Dictionary<ulong, ServerData>>();
		private static readonly SemaphoreSlim _sdSync = new SemaphoreSlim(1);
		protected LazyUser(ulong id) {
			Id = id;
			SdSync(() => {
				if (!_serverData.ContainsKey(id)) {
					_serverData.Add(id, new Dictionary<ulong, ServerData>());
				}
			});
		}
		public static LazyUser Get(ulong userId) {
			return User._cache.Mutex(() => User._cache.Contains(userId) ? User._cache[userId] : new LazyUser(userId));
		}
		private static async Task SdSyncAsync(Func<Task> act) {
			await _sdSync.WaitAsync();
			await act();
			_sdSync.Release();
		}
		private static void SdSync(Action act) {
			_sdSync.Wait();
			act();
			_sdSync.Release();
		}
		private static async Task<T> SdSyncAsync<T>(Func<Task<T>> act) {
			await _sdSync.WaitAsync();
			T res = await act();
			_sdSync.Release();
			return res;
		}
		private static T SdSync<T>(Func<T> act) {
			_sdSync.Wait();
			T res = act();
			_sdSync.Release();
			return res;
		}
		internal static void PushServerData(ulong serverId, ServerUserJson json) {
			SdSync(() => {
				Dictionary<ulong, ServerData> map;
				if (_serverData.ContainsKey(json.user.id)) {
					map = _serverData[json.user.id];
				} else {
					map = new Dictionary<ulong, ServerData>();
					_serverData.Add(json.user.id, map);
				}
				User.CreateFromJson(json.user, serverId);
				var data = new ServerData(json.user.id, serverId, json);
				if (map.ContainsKey(serverId)) {
					map[serverId] = data;
				} else {
					map.Add(serverId, data);
				}
			});
		}
		public ulong Id {
			get;
		}
		public Ternary IsMentionable {
			get => Ternary.Yes;
		}
		public string MentionContent {
			get => MessageBuilder.MentionOpen + "@!" + Id + MessageBuilder.MentionClose;
		}
		public string MentionNoNicknameContent {
			get => MessageBuilder.MentionOpen + "@" + Id + MessageBuilder.MentionClose;
		}
		string IMentionable.MentionFallbackName {
			get => MentionNoNicknameContent;
		}
		private bool HasKey(ulong sid) {
			return _serverData[Id].ContainsKey(sid);
		}
		private bool HasNicknameImpl(ulong forServer) {
			return HasKey(forServer) && _serverData[Id][forServer].HasNickname;
		}
		public virtual async Task<User> ToUserAsync(Client client) {
			return await User.GetAsync(Id, client);
		}
		public virtual async Task<User> ToUserAsync() {
			return await User.GetAsync(Id);
		}
		public virtual User ToUser(Client client) {
			return User.Get(Id, client);
		}
		public virtual User ToUser() {
			return User.Get(Id);
		}
		public Task<bool> IsMentionableByAsync(ulong userId, ulong inServerId, Client client) {
			return true.AsTask();
		}
		public async Task<bool> IsInServerAsync(ulong serverId, Client client) {
			return IsInServer(await Server.GetAsync(serverId, client));
		}
		public async Task<bool> IsInServerAsync(ulong serverId) {
			return IsInServer(await Server.GetAsync(serverId));
		}
		public bool IsInServer(ulong serverId, Client client) {
			return IsInServer(Server.Get(serverId, client));
		}
		public bool IsInServer(ulong serverId) {
			return IsInServer(Server.Get(serverId));
		}
		public bool IsInServer(Server server) {
			return server.Members.ContainsUser(this);
		}
		// TODO: Implement GET guild user data endpoint, then methods that accept Client objects should go here
		// (Then remove nulls, defaults, falses from some method returns, and make async methods)
		[CanBeNull]
		public ServerData GetServerData(ulong serverId) {
			return HasKey(serverId) ? _serverData[Id][serverId] : null;
		}
		[CanBeNull]
		public ServerData GetServerData(Server server) {
			return GetServerData(server.Id);
		}
		public bool HasNickname(ulong forServer) {
			return SdSync(() => HasNicknameImpl(forServer));
		}
		public bool HasNickname(Server forServer) {
			return HasNickname(forServer.Id);
		}
		public string GetNickname(ulong forServer) {
			// TODO: Allow client, use ToUser(client).QualifiedName (catch exceptions and use ID as fallback)
			_sdSync.Wait();
			string res;
			if (HasNicknameImpl(forServer)) {
				res = _serverData[Id][forServer].Nickname;
				_sdSync.Release();
			} else {
				_sdSync.Release();
				try {
					res = ToUser().QualifiedName;
				} catch (NoSuchUserException) {
					res = Id.ToString();
				}
			}
			return res;
		}
		public string GetNickname(Server forServer) {
			return GetNickname(forServer.Id);
		}
		public DateTime GetJoinTime(ulong forServer) {
			return SdSync(() => HasKey(forServer) ? _serverData[Id][forServer].UserJoinTime : default);
		}
		public DateTime GetJoinTime(Server forServer) {
			return GetJoinTime(forServer.Id);
		}
		public bool IsDeafened(ulong inServer) {
			return SdSync(() => HasKey(inServer) && _serverData[Id][inServer].IsDeafened);
		}
		public bool IsDeafened(Server inServer) {
			return IsDeafened(inServer.Id);
		}
		public bool IsMuted(ulong inServer) {
			return SdSync(() => HasKey(inServer) && _serverData[Id][inServer].IsMuted);
		}
		public bool IsMuted(Server inServer) {
			return IsMuted(inServer.Id);
		}
		public override string ToString() {
			return "User " + Id;
		}
		public bool Equals([CanBeNull] LazyUser u) {
			return !(u is null) && Id == u.Id;
		}
		public override bool Equals([CanBeNull] object obj) {
			return obj is LazyUser user && Equals(user);
		}
		public override int GetHashCode() {
			return Id.GetHashCode();
		}
		public static bool operator ==([CanBeNull] LazyUser a, [CanBeNull] LazyUser b) {
			return a is null ? b is null : a.Equals(b);
		}
		public static bool operator !=([CanBeNull] LazyUser a, [CanBeNull] LazyUser b) {
			return !(a == b);
		}
	}
}
