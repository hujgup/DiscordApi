using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Echo.Utils;
using Echo.Utils.Async;
using Echo.Discord.Api.Json;
using Echo.Discord.Api.Servers;
using Echo.Discord.Api.Http;
using Echo.Discord.Api.Messages;
using Echo.Discord.Api.Events;
using Newtonsoft.Json.Linq;
using System.Threading;
using JetBrains.Annotations;

namespace Echo.Discord.Api.Channels {
	public partial class ServerTextChannel : TextChannel, INonCatServerChannel, IMentionable {
		internal static readonly BaseCache<ServerTextChannel> _cache = new BaseCache<ServerTextChannel>();
		private ulong? _cid;
		private IReadOnlyDictionary<ulong, Message> _oldPins;
		private SemaphoreSlim _opMutex;
		internal ServerTextChannel(ChannelJson json, ulong ogId) : base(json, ogId) {
			_oldPins = null;
			_opMutex = new SemaphoreSlim(1);
		}
		internal static ServerTextChannel CreateFromJson(ChannelJson json, [CanBeNull] object state) {
			return (ServerTextChannel)ChannelUtils.CreateFromJson(json, state) ?? throw new ArgumentOutOfRangeException(nameof(json), "Incorrect JSON type.");
		}
		public static new async Task<ServerTextChannel> GetAsync(ulong channelId, [CanBeNull] Client client) {
			return await FactoryUtils.GetChannelAsync(_cache, "ServerTextChannel", null, json => new ServerTextChannel(json, channelId), Populate, channelId, client, ChannelType.ServerText);
		}
		public static new async Task<ServerTextChannel> GetAsync(ulong channelId) {
			return await GetAsync(channelId, DiscordEnvironment.CurrentClient);
		}
		public static new ServerTextChannel Get(ulong channelId, [CanBeNull] Client client) {
			return GetAsync(channelId, client).Await();
		}
		public static new ServerTextChannel Get(ulong channelId) {
			return GetAsync(channelId).Await();
		}
		[CanBeNull]
		public static new ServerTextChannel GetCached(ulong channelId) {
			return FactoryUtils.GetCached(_cache, channelId);
		}
		public string Name {
			get;
			private set;
		}
		public string Topic {
			get;
			private set;
		}
		public bool IsNsfw {
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
		public bool HasCategory {
			get => _cid != null;
		}
		public ulong CategoryId {
			get {
				if (!HasCategory) {
					throw new NoSuchChannelException("Cannot get a category ID when the instance channel is not in a category.");
				}
				return (ulong)_cid;
			}
		}
		public CachedPromise<ChannelCategory> Category {
			get;
			private set;
		}
		public Ternary IsMentionable {
			get => Ternary.Yes;
		}
		public string MentionContent {
			get => MessageBuilder.MentionOpen + "#" + Id + MessageBuilder.MentionClose;
		}
		string IMentionable.MentionFallbackName {
			get => Name;
		}
		internal void OnMessagePin(object sender, PinEventArgs e) {
			MessagePin?.Invoke(sender, e);
		}
		internal void OnMessageUnpin(object sender, PinEventArgs e) {
			MessageUnpin?.Invoke(sender, e);
		}
		public event EventHandler<PinEventArgs> MessagePin;
		public event EventHandler<PinEventArgs> MessageUnpin;
		internal override void Populate(ChannelJson json, [CanBeNull] object state) {
			Name = json.name;
			Topic = json.topic;
			IsNsfw = json.nsfw == true;
			Position = (int)json.position;
			Permissions = PermissionUtils.GetMask(json.permission_overwrites);
			ServerId = state != null ? (ulong)state : (ulong)json.guild_id;
			Server = new CachedPromise<Servers.Server>(Servers.Server._cache, ServerId, Servers.Server.GetAsync);
			_cid = json.parent_id;
			Category = _cid != null ? new CachedPromise<ChannelCategory>(ChannelCategory._cache, (ulong)_cid, ChannelCategory.GetAsync) : new CachedPromise<ChannelCategory>(null);
		}
		public Task<bool> IsMentionableByAsync(ulong userId, ulong inServerId, [CanBeNull] Client client) {
			return true.AsTask();
		}
		/// <summary>
		/// Gets data pertaining to what messages are pinned to this channel. Calling this method initializes the state for the pin and unpin events.
		/// </summary>
		/// <param name="client">The Discord client to use to get the pinned messages.</param>
		/// <returns>A set of pinned messages, including what was pinned before and whas has changed since the last call to this method.</returns>
		public async Task<PinnedMessageData> GetPinnedMessagesAsync([CanBeNull] Client client) {
			if (client == null) {
				throw new NotInEnvironmentException("When a client is not specified, a GetPinnedMessages method call requires the current thread to be in a DiscordEnvironment.");
			}
			JObject[] json = await client.GetPinnedMessagesAsync(Id);
			IReadOnlyDictionary<ulong, Message> pinned = json.Select(Message.Create).ToIdDic();
			await _opMutex.WaitAsync();
			try {
				var res = new PinnedMessageData(_oldPins, pinned);
				_oldPins = pinned;
				return res;
			} finally {
				_opMutex.Release();
			}
		}
		public async Task<PinnedMessageData> GetPinnedMessagesAsync() {
			return await GetPinnedMessagesAsync(DiscordEnvironment.CurrentClient);
		}
		public PinnedMessageData GetPinnedMessages([CanBeNull] Client client) {
			return GetPinnedMessagesAsync(client).Await();
		}
		public PinnedMessageData GetPinnedMessages() {
			return GetPinnedMessagesAsync().Await();
		}
		public override void Uncache() {
			ChannelUtils.Uncache(_cache, this);
		}
		public override string ToString() {
			return "#" + Name;
		}
	}
}
