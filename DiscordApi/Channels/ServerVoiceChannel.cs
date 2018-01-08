/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Echo.Discord.Api.Json;
using Echo.Discord.Api.Http;

namespace Echo.Discord.Api.Channels {
	//  : IServerChannel, IVoiceChannel
	public class ServerVoiceChannel : IChannel {
		private static readonly BaseCache<ServerVoiceChannel> _cache = new BaseCache<ServerVoiceChannel>();
		private ServerVoiceChannel(ChannelJson json) {
		}
		internal static ServerVoiceChannel CreateFromJson(ChannelJson json) {
			return null;
		}
		public static async Task<ServerVoiceChannel> GetAsync(ulong channelId, Client client) {
			return await FactoryUtils.GetChannelAsync(_cache, nameof(ServerVoiceChannel), json => new ServerVoiceChannel(json), Populate, channelId, client, ChannelType.ServerVoice);
		}
		public static async Task<ServerVoiceChannel> GetAsync(ulong channelId) {
			return await GetAsync(channelId, null);
		}
		public static ServerVoiceChannel Get(ulong channelId, Client client) {
			return GetAsync(channelId, client).Await();
		}
		public static ServerVoiceChannel Get(ulong channelId) {
			return GetAsync(channelId).Await();
		}
		public ulong Id {
			get;
		}
		public ChannelType Type {
			get;
		}
		internal static void Populate(ServerVoiceChannel obj, ChannelJson json) {
		}
		public async Task PullUpdateAsync(Client client) {
			await FactoryUtils.UpdateAsync(this, client.GetChannelJsonAsync, Populate);
		}
		public void PullUpdate(Client client) {
			PullUpdateAsync(client).Await();
		}
	}
}
*/
