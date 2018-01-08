/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Echo.Discord.Api.Json;
using Echo.Discord.Api.Channels;
using Echo.Discord.Api.Http;

namespace Echo.Discord.Api.Servers {
	// IAlias
	public class VoiceState : IIdentifiable {
		private static readonly BaseCache<VoiceState> _cache = new BaseCache<VoiceState>();
		internal VoiceState(VoiceStateJson json) {
		}
		ulong IIdentifiable.Id {
			get => UserId;
		}
		public ulong UserId {
			get;
		}
		public ulong ChannelId {
			get;
		}
		public ulong ServerId {
			get;
		}
		public string SessionId {
			get;
		}
		public bool IsGloballyDeafened {
			get;
		}
		public bool IsGloballyMuted {
			get;
		}
		public bool IsLocallyDeafened {
			get;
		}
		public bool IsLocallyMuted {
			get;
		}
		public bool IsDeafened {
			get => IsGloballyDeafened | IsLocallyDeafened;
		}
		public bool IsMuted {
			get => IsGloballyMuted | IsLocallyMuted;
		}
		public bool IsSuppressedByClientUser {
			get;
		}
		public async Task<IVoiceChannel> GetChannelAsync(Client client) {
			return (IVoiceChannel)await ChannelUtils.GetChannelAsync(ChannelId, client);
		}
		public async Task<IVoiceChannel> GetChannelAsync() {
			return (IVoiceChannel)await ChannelUtils.GetChannelAsync(ChannelId);
		}
		public IVoiceChannel GetChannel(Client client) {
			return (IVoiceChannel)ChannelUtils.GetChannel(ChannelId, client);
		}
		public IVoiceChannel GetChannel() {
			return (IVoiceChannel)ChannelUtils.GetChannel(ChannelId);
		}
		public async Task<Server> GetServerAsync(Client client) {
			return await Server.GetAsync(ServerId, client);
		}
		public async Task<Server> GetServerAsync() {
			return await Server.GetAsync(ServerId);
		}
		public Server GetServer(Client client) {
			return Server.Get(ServerId, client);
		}
		public Server GetServer() {
			return Server.Get(ServerId);
		}
		public async Task<ServerUser> GetUserAsync(Client client) {
			return await ServerUser.GetAsync(ServerId, client);
		}
		public async Task<ServerUser> GetUserAsync() {
			return await ServerUser.GetAsync(ServerId);
		}
		public ServerUser GetUser(Client client) {
			return ServerUser.Get(ServerId, client);
		}
		public ServerUser GetUser() {
			return ServerUser.Get(ServerId);
		}
	}
}
*/