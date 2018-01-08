using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Echo.Utils.Async;
using Echo.Discord.Api.Http;
using Echo.Discord.Api.Json;
using Echo.Discord.Api.Events;
using JetBrains.Annotations;

namespace Echo.Discord.Api.Channels {
	public static partial class ChannelUtils {
		private static readonly Dictionary<ChannelType, ICache<IChannel>> _typeCaches = new Dictionary<ChannelType, ICache<IChannel>>() {
			{ ChannelType.DirectMessage, new ChannelAliasCache<DirectMessageTextChannel>(DirectMessageTextChannel._cache) },
			{ ChannelType.GroupDirectMessage, new ChannelAliasCache<DirectMessageTextChannel>(DirectMessageTextChannel._cache) },
			{ ChannelType.ServerCategory, new ChannelAliasCache<ChannelCategory>(ChannelCategory._cache) },
			{ ChannelType.ServerText, new ChannelAliasCache<ServerTextChannel>(ServerTextChannel._cache) }
			//{ ChannelType.ServerVoice, ServerVoiceChannel._cache }
		};
		internal static readonly BaseCache<IChannel> _globalCache = new BaseCache<IChannel>();
		[CanBeNull]
		internal static IChannel CreateFromJson(ChannelJson json, [CanBeNull] object state) {
			IChannel res = null;
			if (_typeCaches.ContainsKey(json.type)) {
				_globalCache.Mutex(() => {
					ICache<IChannel> cache = _typeCaches[json.type];
					cache.Mutex(() => {
						if (cache.Contains(json.id)) {
							res = cache[json.id];
						} else if (_globalCache.Contains(json.id)) {
							throw new WrongChannelTypeException("JSON specifies type of channel is " + json.type + ", but cached data says it should be " + _globalCache[json.id].Type + ".");
						} else {
							switch (json.type) {
								case ChannelType.DirectMessage:
								case ChannelType.GroupDirectMessage:
									res = new DirectMessageTextChannel(json, json.id);
									TextChannel.Populate((TextChannel)res, json, state);
									break;
								case ChannelType.ServerCategory:
									res = new ChannelCategory(json.id, json.id);
									ChannelCategory.Populate((ChannelCategory)res, json, state);
									break;
								case ChannelType.ServerText:
									res = new ServerTextChannel(json, json.id);
									TextChannel.Populate((TextChannel)res, json, state);
									break;
							}
							_globalCache.Add(res);
							cache.Add(res);
						}
					});
				});
			}
			return res;
		}
		internal static bool ChannelEquals([CanBeNull] IChannel a, [CanBeNull] IChannel b) {
			return a is null ? b is null : !(b is null) && a.Type == b.Type && a.Id == b.Id;
		}
		internal static bool ChannelEquals([CanBeNull] IChannel a, [CanBeNull] object b) {
			return b is IChannel ? ChannelEquals(a, (IChannel)b) : false;
		}
		internal static int Compare([CanBeNull] IServerChannel a, [CanBeNull] IServerChannel b) {
			int res = a is null ? (b is null ? 0 : -1) : (b is null ? 1 : 0);
			if (res == 0 && a != null) {
				// ReSharper disable once PossibleNullReferenceException
				res = a.ServerId.CompareTo(b.ServerId);
				if (res == 0) {
					res = a.Position.CompareTo(b.Position);
				}
			}
			return res;
		}
		internal static void Uncache<T>(ICache<T> cache, ulong id) where T : IChannel {
			_globalCache.Mutex(() => {
				_globalCache.Remove(id);
				FactoryUtils.Uncache(cache, id);
			});
		}
		internal static void Uncache<T>(ICache<T> cache, T item) where T : IChannel {
			_globalCache.Mutex(() => {
				_globalCache.Remove(item);
				FactoryUtils.Uncache(cache, item);
			});
		}
		public static async Task<IChannel> GetChannelAsync(ulong channelId, [CanBeNull] Client client, params ChannelType[] types) {
			client = client ?? DiscordEnvironment.CurrentClient;
			FactoryUtils.ValidateInEnv(client);
			return await FactoryUtils.GetChannelAsync(_globalCache, "IChannel", null, json => {
				return CreateFromJson(json, null);
			}, (x, json, state) => {
				FactoryUtils.PopulateInstance(x, json, state);
			}, channelId, client, types);
		}
		public static async Task<IChannel> GetChannelAsync(ulong channelId, params ChannelType[] types) {
			return await GetChannelAsync(channelId, null, types);
		}
		public static IChannel GetChannel(ulong channelId, [CanBeNull] Client client, params ChannelType[] types) {
			return GetChannelAsync(channelId, client, types).Await();
		}
		public static IChannel GetChannel(ulong channelId, params ChannelType[] types) {
			return GetChannelAsync(channelId, types).Await();
		}
		[CanBeNull]
		public static IChannel GetCachedChannel(ulong channelId) {
			return FactoryUtils.GetCached(_globalCache, channelId);
		}
		public static async Task<IChannel> GetNonCatChannelAsync(ulong channelId, [CanBeNull] Client client) {
			// ChannelType.ServerVoice
			return await GetChannelAsync(channelId, client, ChannelType.DirectMessage, ChannelType.GroupDirectMessage, ChannelType.ServerText);
		}
		public static async Task<IChannel> GetNonCatChannelAsync(ulong channelId) {
			return await GetNonCatChannelAsync(channelId, null);
		}
		public static IChannel GetNonCatChannel(ulong channelId, [CanBeNull] Client client) {
			return GetNonCatChannelAsync(channelId, client).Await();
		}
		public static IChannel GetNonCatChannel(ulong channelId) {
			return GetNonCatChannelAsync(channelId).Await();
		}
		[CanBeNull]
		public static IChannel GetCachedNonCatChannel(ulong channelId) {
			return FactoryUtils.GetCached(_globalCache, channelId);
		}
		public static async Task<IServerChannel> GetServerChannelAsync(ulong channelId, [CanBeNull] Client client) {
			// ChannelType.ServerVoice
			return (IServerChannel)await GetChannelAsync(channelId, client, ChannelType.ServerCategory, ChannelType.ServerText);
		}
		public static async Task<IServerChannel> GetServerChannelAsync(ulong channelId) {
			return await GetServerChannelAsync(channelId, null);
		}
		public static IServerChannel GetServerChannel(ulong channelId, [CanBeNull] Client client) {
			return GetServerChannelAsync(channelId, client).Await();
		}
		public static IServerChannel GetServerChannel(ulong channelId) {
			return GetServerChannelAsync(channelId).Await();
		}
		[CanBeNull]
		public static IServerChannel GetCachedServerChannel(ulong channelId) {
			return (IServerChannel)FactoryUtils.GetCached(_globalCache, channelId);
		}
		public static async Task<INonCatServerChannel> GetNonCatServerChannelAsync(ulong channelId, [CanBeNull] Client client) {
			// ChannelType.ServerVoice
			return (INonCatServerChannel)await GetChannelAsync(channelId, client, ChannelType.ServerText);
		}
		public static async Task<INonCatServerChannel> GetNonCatServerChannelAsync(ulong channelId) {
			return await GetNonCatServerChannelAsync(channelId, null);
		}
		public static INonCatServerChannel GetNonCatServerChannel(ulong channelId, [CanBeNull] Client client) {
			return GetNonCatServerChannelAsync(channelId, client).Await();
		}
		public static INonCatServerChannel GetNonCatServerChannel(ulong channelId) {
			return GetNonCatServerChannelAsync(channelId).Await();
		}
		[CanBeNull]
		public static INonCatServerChannel GetCachedNonCatServerChannel(ulong channelId) {
			return (INonCatServerChannel)FactoryUtils.GetCached(_globalCache, channelId);
		}
		/*
		public static async Task<IVoiceChannel> GetVoiceChannelAsync(ulong channelId, Client client) {
			return (IVoiceChannel)await GetChannelAsync(channelId, client, ChannelType.ServerVoice);
		}
		public static async Task<IVoiceChannel> GetVoiceChannelAsync(ulong channelId) {
			return await GetVoiceChannelAsync(channelId, null);
		}
		public static IVoiceChannel GetVoiceChannel(ulong channelId, Client client) {
			return GetVoiceChannelAsync(channelId, client).Await();
		}
		public static IVoiceChannel GetVoiceChannel(ulong channelId) {
			return GetVoiceChannelAsync(channelId).Await();
		}
		*/
		public static async Task<IDirectMessageChannel> GetDirectMessageChannelAsync(ulong channelId, [CanBeNull] Client client) {
			return (IDirectMessageChannel)await GetChannelAsync(channelId, client, ChannelType.DirectMessage, ChannelType.GroupDirectMessage);
		}
		public static async Task<IDirectMessageChannel> GetDirectMessageChannelAsync(ulong channelId) {
			return await GetDirectMessageChannelAsync(channelId, null);
		}
		public static IDirectMessageChannel GetDirectMessageChannel(ulong channelId, [CanBeNull] Client client) {
			return GetDirectMessageChannelAsync(channelId, client).Await();
		}
		public static IDirectMessageChannel GetDirectMessageChannel(ulong channelId) {
			return GetDirectMessageChannelAsync(channelId).Await();
		}
		[CanBeNull]
		public static IDirectMessageChannel GetCachedDirectMessageChannel(ulong channelId) {
			return (IDirectMessageChannel)FactoryUtils.GetCached(_globalCache, channelId);
		}
	}
}
