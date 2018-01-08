using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Echo.Utils;
using Echo.Utils.Async;
using Echo.Discord.Api.Json;
using Echo.Discord.Api.Channels;
using Echo.Discord.Api.Http;
using Echo.Discord.Api.Events;
using JetBrains.Annotations;

namespace Echo.Discord.Api.Servers {
	public partial class Server : IEquatable<Server>, IHasManyChannels<IChannel>, ICached, INamed, IAlias, IHasCachedOwner, IHasCachedAfkChannel, IHasManyRoles, IHasManyEmojis, IHasManyMembers, IHasManyPresences {
		internal static readonly BaseCache<Server> _cache = new BaseCache<Server>();
		private ulong? _afkChannelId;
		private Server(ServerJson json) {
			Id = json.id;
		}
		internal static Server CreateFromJson(ServerJson json, [CanBeNull] object state) {
			return FactoryUtils.CreateFromJson(_cache, state, json2 => new Server(json2), Populate, json);
		}
		public static async Task<Server> GetAsync(ulong serverId, [CanBeNull] Client client) {
			return await FactoryUtils.GetAsync<Server, ServerJson, NoSuchServerException>(_cache, "Server", null, FactoryUtils.ResolveGetter<ServerJson>(client, c2 => c2.GetServerJsonAsync), json => new Server(json), Populate, serverId);
		}
		public static async Task<Server> GetAsync(ulong serverId) {
			return await GetAsync(serverId, DiscordEnvironment.CurrentClient);
		}
		public static Server Get(ulong serverId, [CanBeNull] Client client) {
			return GetAsync(serverId, client).Await();
		}
		public static Server Get(ulong serverId) {
			return GetAsync(serverId).Await();
		}
		[CanBeNull]
		public static Server GetCached(ulong serverId) {
			return FactoryUtils.GetCached(_cache, serverId);
		}
		public ulong Id {
			get;
		}
		public string Name {
			get;
			private set;
		}
		public string Icon {
			get;
			private set;
		}
		public string Splash {
			get;
			private set;
		}
		public ulong OwnerId {
			get;
			private set;
		}
		public CachedPromise<User> Owner {
			get;
			private set;
		}
		public LazyUser LazyOwner {
			get;
			private set;
		}
		public string Region {
			get;
			private set;
		}
		public bool HasAfkChannel {
			get => _afkChannelId != null;
		}
		public ulong AfkChannelId {
			get {
				if (!HasAfkChannel) {
					throw new NoSuchChannelException("Cannot get the AFK channel ID when the instance server has no AFK channel.");
				}
				return (ulong)_afkChannelId;
			}
		}
		public CachedPromise<IChannel> AfkChannel {
			get;
			private set;
		}
		public int AfkTimeout {
			get;
			private set;
		}
		public bool CanEmbed {
			get;
			private set;
		}
		public ulong? EmbedChannelId {
			get;
			private set;
		}
		public VerificationLevel VerificationLevel {
			get;
			private set;
		}
		public NotificationLevel DefaultNotificationLevel {
			get;
			private set;
		}
		public NsfwFilterLevel NsfwFilterLevel {
			get;
			private set;
		}
		public IReadOnlyDictionary<ulong, Role> Roles {
			get;
			private set;
		}
		public Role EveryoneRole {
			get;
			private set;
		}
		public IReadOnlyDictionary<ulong, ServerEmoji> Emojis {
			get;
			private set;
		}
		public IReadOnlyList<string> Features {
			get;
			private set;
		}
		public MfaLevel AuthentLevel {
			get;
			private set;
		}
		public bool WidgetEnabled {
			get;
			private set;
		}
		public ulong? WidgetChannelId {
			get;
			private set;
		}
		public DateTime ApplicationJoinTime {
			get;
			private set;
		}
		public Ternary IsLarge {
			get;
			private set;
		}
		public Ternary IsUnavailable {
			get;
			private set;
		}
		public int MemberCount {
			get;
			private set;
		}
		public IReadOnlyDictionary<ulong, User> Members {
			get;
			private set;
		}
		/*
		public IReadOnlyList<VoiceState> VoiceStates {
			get;
			private set;
		}
		*/
		public IReadOnlyDictionary<ulong, IChannel> Channels {
			get;
			private set;
		}
		public IReadOnlyDictionary<ulong, Presence> Presences {
			get;
			private set;
		}
		internal static void Populate(Server obj, ServerJson json, [CanBeNull] object state) {
			obj.Name = json.name;
			obj.Icon = json.icon;
			obj.Splash = json.splash;
			obj.OwnerId = json.owner_id;
			obj.Owner = new CachedPromise<User>(User._cache, obj.OwnerId, User.GetAsync);
			obj.LazyOwner = LazyUser.Get(obj.OwnerId);
			obj.Region = json.region;
			obj._afkChannelId = json.afk_channel_id;
			obj.AfkChannel = obj._afkChannelId != null ? new CachedPromise<IChannel>(ChannelUtils._globalCache, (ulong)obj._afkChannelId, async (id, client) => await ChannelUtils.GetChannelAsync(id, client)) : new CachedPromise<IChannel>(null);
			obj.AfkTimeout = json.afk_timeout;
			obj.CanEmbed = json.embed_enabled;
			obj.EmbedChannelId = json.embed_channel_id;
			obj.VerificationLevel = json.verification_level;
			obj.DefaultNotificationLevel = json.default_message_notifications;
			obj.NsfwFilterLevel = json.explicit_content_filter;
			obj.Features = json.features.ToList();
			obj.AuthentLevel = json.mfa_level;
			obj.WidgetEnabled = json.widget_enabled;
			obj.WidgetChannelId = json.widget_channel_id;
			// TODO: Instead of default values, turn these into promises
			obj.ApplicationJoinTime = json.joined_at != null ? DateTime.Parse(json.joined_at) : DateTime.MinValue;
			obj.IsLarge = json.large != null ? TernaryUtils.FromNullable(json.large) : Ternary.Maybe;
			obj.IsUnavailable = json.unavailable != null ? TernaryUtils.FromNullable(json.unavailable) : Ternary.Maybe;
			obj.MemberCount = json.member_count != null ? (int)json.member_count : -1;
			/*
			if (json.voice_states != null) {
				obj.VoiceStates = (from x in json.voice_states select new VoiceState(x)).ToList();
			}
			*/
			obj.Channels = json.channels?.Where(x => x != null).Select(x => ChannelUtils.CreateFromJson(x, obj.Id)).ToIdDic();
			obj.Presences = json.presences?.Where(x => x != null).Select(x => new Presence(x)).ToDictionary(x => x.UserId);
			obj.Emojis = json.emojis.Where(x => x != null).ToDictionary(x => x.id, x => ServerEmoji.CreateFromJson(obj.Id, x, obj.Id));
			obj.Roles = json.roles.Where(x => x != null).ToDictionary(x => x.id, x => Role.CreateFromJson(obj.Id, x, obj.Id));
			obj.EveryoneRole = obj.Roles.Values.First(x => x.Name == Role.EveryoneName);
			if (json.members != null) {
				foreach (ServerUserJson suj in json.members) {
					User.CreateFromJson(suj.user, obj.Id);
					LazyUser.PushServerData(obj.Id, suj);
				}
				obj.Members = json.members.Where(x => x != null).Select(x => {
					User res = User.CreateFromJson(x.user, obj.Id);
					LazyUser.PushServerData(obj.Id, x);
					return res;
				}).ToIdDic();
			} else {
				obj.Members = null;
			}
		}
		internal void OnMessagePin(object sender, PinEventArgs e) {
			MessagePin?.Invoke(sender, e);
		}
		internal void OnMessageUnpin(object sender, PinEventArgs e) {
			MessageUnpin?.Invoke(sender, e);
		}
		public event EventHandler<PinEventArgs> MessagePin;
		public event EventHandler<PinEventArgs> MessageUnpin;
		private async Task<T> ValidateAfkChannelAsync<T>(Func<ulong, Task<T>> getter) {
			if (HasAfkChannel) {
				return await getter((ulong)_afkChannelId);
				// ReSharper disable once RedundantIfElseBlock
			} else {
				throw new NoSuchChannelException("Server " + Id + " does not have an AFK channel.");
			}
		}
		private T ValidateAfkChannel<T>(Func<ulong, T> getter) {
			if (HasAfkChannel) {
				return getter((ulong)_afkChannelId);
				// ReSharper disable once RedundantIfElseBlock
			} else {
				throw new NoSuchChannelException("Server " + Id + " does not have an AFK channel.");
			}
		}
		private async Task<T> ValidateWidgetChannelAsync<T>(Func<ulong, Task<T>> getter) {
			if (WidgetEnabled) {
				return await getter((ulong)WidgetChannelId);
				// ReSharper disable once RedundantIfElseBlock
			} else {
				throw new NoSuchChannelException("Server " + Id + " does not have a widget channel.");
			}
		}
		private T ValidateWidgetChannel<T>(Func<ulong, T> getter) {
			if (WidgetEnabled) {
				return getter((ulong)WidgetChannelId);
				// ReSharper disable once RedundantIfElseBlock
			} else {
				throw new NoSuchChannelException("Server " + Id + " does not have a widget channel.");
			}
		}
		private TOut ConditionalGet<TOut, TExcept>(ulong id, Predicate<ulong> conditional, Func<ulong, TOut> getter) where TExcept : NoSuchObjectException {
			if (conditional(id)) {
				return getter(id);
				// ReSharper disable once RedundantIfElseBlock
			} else {
				throw (TExcept)Activator.CreateInstance(typeof(TExcept), nameof(TOut) + " with ID " + id + " is not in this server.");
			}
		}
		private bool HasChannelOfType(ulong channelId, params ChannelType[] types) {
			return Channels.Values.FirstOrDefault(channel => types.Contains(channel.Type) && channel.Id == channelId) != null;
		}
		public bool HasFeature(string feature) {
			return Features.Contains(feature);
		}
		public bool HasChannel(INonCatServerChannel channel) {
			return HasChannel(channel.Id);
		}
		public bool HasChannel(ulong channelId) {
			// ChannelType.ServerVoice
			return HasChannelOfType(channelId, ChannelType.ServerText);
		}
		public bool HasTextChannel(ServerTextChannel channel) {
			return HasTextChannel(channel.Id);
		}
		public bool HasTextChannel(ulong channelId) {
			return HasChannelOfType(channelId, ChannelType.ServerText);
		}
		/*
		public bool HasVoiceChannel(ServerVoiceChannel channel) {
			return HasVoiceChannel(channel.Id);
		}
		public bool HasVoiceChannel(ulong channelId) {
			return HasChannelOfType(channelId, ChannelType.ServerVoice);
		}
		*/
		public bool HasCategory(ChannelCategory category) {
			return HasCategory(category.Id);
		}
		public bool HasCategory(ulong categoryId) {
			return HasChannelOfType(categoryId, ChannelType.ServerCategory);
		}
		public INonCatServerChannel GetChannel(ulong channelId) {
			return ConditionalGet<INonCatServerChannel, NoSuchChannelException>(channelId, HasChannel, ChannelUtils.GetNonCatServerChannel);
		}
		public ServerTextChannel GetTextChannel(ulong channelId) {
			return ConditionalGet<ServerTextChannel, NoSuchChannelException>(channelId, HasTextChannel, ServerTextChannel.Get);
		}
		/*
		public ServerVoiceChannel GetVoiceChannel(ulong channelId) {
			return ConditionalGet<ServerVoiceChannel, NoSuchChannelException>(channelId, HasVoiceChannel, ServerVoiceChannel.Get);
		}
		*/
		public ChannelCategory GetCategory(ulong categoryId) {
			return ConditionalGet<ChannelCategory, NoSuchChannelException>(categoryId, HasCategory, ChannelCategory.Get);
		}
		public async Task<IChannel> GetWidgetChannelAsync([CanBeNull] Client client) {
			return await ValidateWidgetChannelAsync(id => ChannelUtils.GetChannelAsync(id, client));
		}
		public async Task<IChannel> GetWidgetChannelAsync() {
			return await GetWidgetChannelAsync(null);
		}
		public IChannel GetWidgetChannel([CanBeNull] Client client) {
			return ValidateWidgetChannel(id => ChannelUtils.GetChannel(id, client));
		}
		public IChannel GetWidgetChannel() {
			return GetWidgetChannel(null);
		}
		public async Task PullUpdateAsync([CanBeNull] Client client) {
			client = client ?? DiscordEnvironment.CurrentClient;
			FactoryUtils.ValidateInEnv(client);
			await FactoryUtils.UpdateAsync(this, null, client.GetServerJsonAsync, Populate);
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
		public bool Equals([CanBeNull] Server sv) {
			return !(sv is null) && Id == sv.Id;
		}
		public override bool Equals([CanBeNull] object obj) {
			return obj is Server server && Equals(server);
		}
		public override int GetHashCode() {
			return Id.GetHashCode();
		}
		public static bool operator ==([CanBeNull] Server a, [CanBeNull] Server b) {
			return a is null ? b is null : a.Equals(b);
		}
		public static bool operator !=([CanBeNull] Server a, [CanBeNull] Server b) {
			return !(a == b);
		}
	}
}
