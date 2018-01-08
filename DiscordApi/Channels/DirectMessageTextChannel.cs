using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Echo.Utils.Async;
using Echo.Discord.Api.Json;
using Echo.Discord.Api.Http;
using Echo.Discord.Api.Events;
using JetBrains.Annotations;

namespace Echo.Discord.Api.Channels {
	public partial class DirectMessageTextChannel : TextChannel, IDirectMessageChannel {
		internal static readonly BaseCache<DirectMessageTextChannel> _cache = new BaseCache<DirectMessageTextChannel>();
		internal DirectMessageTextChannel(ChannelJson json, ulong ogId) : base(json, ogId) {
		}
		internal static DirectMessageTextChannel CreateFromJson(ChannelJson json, [CanBeNull] object state) {
			return (DirectMessageTextChannel)ChannelUtils.CreateFromJson(json, state) ?? throw new ArgumentOutOfRangeException(nameof(json), "Incorrect JSON type.");
		}
		public static new async Task<DirectMessageTextChannel> GetAsync(ulong channelId, [CanBeNull] Client client) {
			return await FactoryUtils.GetChannelAsync(_cache, "DirectMessageTextChannel", null, json => new DirectMessageTextChannel(json, channelId), Populate, channelId, client, ChannelType.DirectMessage, ChannelType.GroupDirectMessage);
		}
		public static new async Task<DirectMessageTextChannel> GetAsync(ulong channelId) {
			return await GetAsync(channelId, DiscordEnvironment.CurrentClient);
		}
		public static new DirectMessageTextChannel Get(ulong channelId, [CanBeNull] Client client) {
			return GetAsync(channelId, client).Await();
		}
		public static new DirectMessageTextChannel Get(ulong channelId) {
			return GetAsync(channelId).Await();
		}
		[CanBeNull]
		public static new DirectMessageTextChannel GetCached(ulong channelId) {
			return FactoryUtils.GetCached(_cache, channelId);
		}
		public IReadOnlyDictionary<ulong, User> Members {
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
		internal override void Populate(ChannelJson json, [CanBeNull] object state) {
			Members = json.recipients.Select(x => User.CreateFromJson(x, state)).ToIdDic();
			OwnerId = state != null ? (ulong)state : (ulong)json.owner_id;
			Owner = new CachedPromise<User>(User._cache, OwnerId, User.GetAsync);
			LazyOwner = LazyUser.Get(OwnerId);
		}
		public override void Uncache() {
			ChannelUtils.Uncache(_cache, this);
		}
		public override string ToString() {
			return "DM " + Id;
		}
	}
}
