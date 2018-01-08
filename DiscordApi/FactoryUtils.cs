using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Echo.Discord.Api.Json;
using Echo.Discord.Api.Http;
using Echo.Discord.Api.Channels;
using Echo.Discord.Api.Servers;
using JetBrains.Annotations;

namespace Echo.Discord.Api {
	internal static class FactoryUtils {
		private static Func<T, object, bool> WrapAction<T>(Action<T, object> action) {
			return (x, y) => {
				action(x, y);
				return false;
			};
		}
		private static async Task<TObj> PrivateGetAsync<TObj, TJson, TExcept>(ICache<TObj> cache, string typeName, [CanBeNull] object state, [CanBeNull] Func<ulong, Task<TJson>> getter, Func<TJson, TObj> creator, Action<TObj, TJson, object> populator, ulong id, ChannelType[] types) where TObj : IIdentifiable where TJson : JsonId where TExcept : NoSuchObjectException {
			return await cache.MutexAsync(async () => {
				TObj res;
				if (cache.Contains(id)) {
					res = cache[id];
				} else if (getter != null) {
					TJson json = await getter(id);
					if (types.Length != 0) {
						FieldInfo fType = json.GetType().GetField("type");
						if (fType != null) {
							int type = (int)fType.GetValue(json);
							#pragma warning disable 0618
							if (type == (int)ChannelType.ServerVoice) {
								#pragma warning restore 0618
								throw new VoiceUnsupportedException("Voice channels are not supported by the DiscordApi library at this time.");
							} else if (!Enum.IsDefined(typeof(ChannelType), type)) {
								throw new UnsupportedChannelTypeException("Channel type " + type + " is not known of to the DiscordApi library.");
							} else if (types.Contains((ChannelType)type)) {
								res = CreateFromJson(cache, state, creator, populator, json);
							} else {
								throw new WrongChannelTypeException(id + " is not a " + typeName + ".");
							}
						} else {
							throw new WrongChannelTypeException(id + " is not a " + typeName + ".");
						}
					} else {
						res = CreateFromJson(cache, state, creator, populator, json);
					}
				} else {
					throw (TExcept)Activator.CreateInstance(typeof(TExcept), typeName + " with ID " + id + " does not exist, and cannot be retrieved remotely because a client was not specified.");
				}
				return res;
			});
		}
		public static T Cast<T>(object obj) {
			return (T)obj;
		}
		public static TOut Switch<TData, TOut>(ChannelType type, [CanBeNull] object state, TData data, Func<TData, object, TOut> serverText, Func<TData, object, TOut> directMessageText, Func<TData, object, TOut> serverCategory) {
			TOut res;
			switch (type) {
				case ChannelType.DirectMessage:
				case ChannelType.GroupDirectMessage:
					res = directMessageText(data, state);
					break;
				case ChannelType.ServerCategory:
					res = serverCategory(data, state);
					break;
				case ChannelType.ServerText:
					res = serverText(data, state);
					break;
				#pragma warning disable 0618
				case ChannelType.ServerVoice:
					#pragma warning restore 0618
					throw new VoiceUnsupportedException("Voice channels are not supported by the DiscordApi library at this time.");
				default:
					throw new UnsupportedChannelTypeException("Channel type " + type + " is not known of to the DiscordApi library.");
			}
			return res;
		}
		public static void Switch<TData>(ChannelType type, [CanBeNull] object state, TData data, Action<TData, object> serverText, Action<TData, object> directMessageText, Action<TData, object> serverCategory) {
			Switch(type, state, data, WrapAction(serverText), WrapAction(directMessageText), WrapAction(serverCategory));
		}
		public static T Switch<T>(ChannelJson json, [CanBeNull] object state, Func<ChannelJson, object, T> serverText, Func<ChannelJson, object, T> directMessageText, Func<ChannelJson, object, T> serverCategory) {
			return Switch(json.type, state, json, serverText, directMessageText, serverCategory);
		}
		public static void Switch(ChannelJson json, [CanBeNull] object state, Action<ChannelJson, object> serverText, Action<ChannelJson, object> directMessageText, Action<ChannelJson, object> serverCategory) {
			Switch(json, state, WrapAction(serverText), WrapAction(directMessageText), WrapAction(serverCategory));
		}
		public static void PopulateInstance(IChannel obj, ChannelJson json, [CanBeNull] object state) {
			Switch(json, state, (json2, state2) => TextChannel.Populate((TextChannel)obj, json2, state2), (json2, state2) => TextChannel.Populate((TextChannel)obj, json2, state2), (json2, state2) => ChannelCategory.Populate((ChannelCategory)obj, json2, state2));
		}
		public static TObj CreateFromJson<TObj, TJson>(ICache<TObj> cache, [CanBeNull] object state, Func<TJson, TObj> creator, Action<TObj, TJson, object> populator, TJson json) where TObj : IIdentifiable where TJson : JsonId {
			return cache.Mutex(() => {
				TObj res;
				if (cache.Contains(json.id)) {
					res = cache[json.id];
				} else {
					res = creator(json);
					cache.Add(res);
					if (typeof(IChannel).IsAssignableFrom(typeof(TObj))) {
						ChannelUtils._globalCache.Add((IChannel)res);
					}
				}
				populator(res, json, state);
				return res;
			});
		}
		[CanBeNull]
		public static Func<ulong, Task<T>> ResolveGetter<T>([CanBeNull] Client client, Func<Client, Func<ulong, Task<T>>> getter) {
			client = client ?? DiscordEnvironment.CurrentClient;
			return client != null ? getter(client) : null;
		}
		public static async Task<TObj> GetAsync<TObj, TJson, TExcept>(ICache<TObj> cache, string typeName, [CanBeNull] object state, [CanBeNull] Func<ulong, Task<TJson>> getter, Func<TJson, TObj> creator, Action<TObj, TJson, object> populator, ulong id) where TObj : IIdentifiable where TJson : JsonId where TExcept : NoSuchObjectException {
			return await PrivateGetAsync<TObj, TJson, TExcept>(cache, typeName, state, getter, creator, populator, id, new ChannelType[0]);
		}
		public static async Task<T> GetChannelAsync<T>(ICache<T> cache, string typeName, [CanBeNull] object state, Func<ChannelJson, T> creator, Action<T, ChannelJson, object> populator, ulong id, Client client, params ChannelType[] types) where T : IIdentifiable {
			return await PrivateGetAsync<T, ChannelJson, NoSuchChannelException>(cache, typeName, state, ResolveGetter<ChannelJson>(client, c => c.GetChannelJsonAsync), creator, populator, id, types);
		}
		public static async Task UpdateAsync<TObj, TJson>(TObj obj, [CanBeNull] object state, [CanBeNull] Func<ulong, Task<TJson>> remoteGetter, Action<TObj, TJson, object> populator) where TObj : IIdentifiable where TJson : class {
			TJson json = remoteGetter == null ? null : await remoteGetter(obj.Id);
			if (json != null) {
				populator(obj, json, state);
			}
		}
		// ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Global
		public static void ValidateInEnv([CanBeNull] Client client) {
			if (client == null) {
				throw new NotInEnvironmentException("When a client is not specified, a PullUpdate method call requires the current thread to be in a DiscordEnvironment.");
			}
		}
		[CanBeNull]
		public static T GetCached<T>(ICache<T> cache, ulong id) where T : IIdentifiable {
			return cache.Mutex(() => cache.Contains(id) ? cache[id] : default);
		}
		public static void Uncache<T>(ICache<T> cache, ulong id) where T : IIdentifiable {
			cache.Mutex(() => cache.Remove(id));
		}
		public static void Uncache<T>(ICache<T> cache, T item) where T : IIdentifiable {
			cache.Mutex(() => cache.Remove(item));
		}
	}
}
