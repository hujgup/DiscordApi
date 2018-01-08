using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Echo.Utils.Async;
using Echo.Discord.Api.Json;
using Echo.Discord.Api.Http;
using Echo.Discord.Api.Servers;
using JetBrains.Annotations;

namespace Echo.Discord.Api {
	public class User : LazyUser, ICached, INamed, IAlias {
		internal static readonly BaseCache<User> _cache = new BaseCache<User>();
		private User(UserJson json) : base(json.id) {
			Email = json.email;
		}
		internal static User CreateFromJson(UserJson json, [CanBeNull] object state) {
			return FactoryUtils.CreateFromJson(_cache, state, json2 => new User(json2), Populate, json);
		}
		public static async Task<User> GetAsync(ulong userId, [CanBeNull] Client client) {
			return await FactoryUtils.GetAsync<User, UserJson, NoSuchUserException>(_cache, "ServerUser", null, FactoryUtils.ResolveGetter<UserJson>(client, c2 => c2.GetUserJsonAsync), json => new User(json), Populate, userId);
		}
		public static async Task<User> GetAsync(ulong userId) {
			return await GetAsync(userId, DiscordEnvironment.CurrentClient);
		}
		public static User Get(ulong userId, [CanBeNull] Client client) {
			return GetAsync(userId, client).Await();
		}
		public static new User Get(ulong userId) {
			return GetAsync(userId).Await();
		}
		[CanBeNull]
		public static User GetCached(ulong userId) {
			return FactoryUtils.GetCached(_cache, userId);
		}
		public bool HasVisibleEmail {
			get => Email != null;
		}
		[CanBeNull]
		public string Email {
			get;
		}
		public string Name {
			get;
			private set;
		}
		public string Discriminator {
			// This is the number in Ring#4289
			get;
			private set;
		}
		public string QualifiedName {
			get => Name + "#" + Discriminator;
		}
		public string Avatar {
			get;
			private set;
		}
		public bool IsBot {
			get;
			private set;
		}
		public MfaLevel AuthentLevel {
			get;
			private set;
		}
		public bool EmailIsVerified {
			get;
			private set;
		}
		private static void Populate(User obj, UserJson json, [CanBeNull] object state) {
			obj.Name = json.username;
			obj.Discriminator = json.discriminator;
			obj.Avatar = json.avatar;
			obj.IsBot = json.bot ?? false;
			obj.AuthentLevel = (json.mfa_enabled != null && (bool)json.mfa_enabled) ? MfaLevel.Elevated : MfaLevel.None;
			obj.EmailIsVerified = json.verified ?? false;
		}
		public override Task<User> ToUserAsync([CanBeNull] Client client) {
			return this.AsTask();
		}
		public override Task<User> ToUserAsync() {
			return this.AsTask();
		}
		public override User ToUser([CanBeNull] Client client) {
			return this;
		}
		public override User ToUser() {
			return this;
		}
		public async Task PullUpdateAsync([CanBeNull] Client client) {
			client = client ?? DiscordEnvironment.CurrentClient;
			FactoryUtils.ValidateInEnv(client);
			await FactoryUtils.UpdateAsync(this, null, FactoryUtils.ResolveGetter<UserJson>(client, c2 => async id => await c2.GetUserJsonAsync(Id)), Populate);
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
			return QualifiedName;
		}
	}
}
