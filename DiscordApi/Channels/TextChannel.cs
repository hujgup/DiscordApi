using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Echo.Discord.Api.Events;
using Echo.Utils.Async;
using Echo.Discord.Api.Json;
using Echo.Discord.Api.Http;
using Echo.Discord.Api.Messages;
using JetBrains.Annotations;

namespace Echo.Discord.Api.Channels {
	public abstract partial class TextChannel {
		internal TextChannel(ChannelJson json, ulong ogId) {
			if (json.id == 0) {
				throw new NoSuchChannelException("No channel with ID " + ogId + " exists.");
			}
			Id = json.id;
			Type = json.type;
		}
		public static async Task<TextChannel> GetAsync(ulong channelId, [CanBeNull] Client client) {
			return (TextChannel)await ChannelUtils.GetChannelAsync(channelId, client, ChannelType.DirectMessage, ChannelType.GroupDirectMessage, ChannelType.ServerText);
		}
		public static async Task<TextChannel> GetAsync(ulong channelId) {
			return await GetAsync(channelId, DiscordEnvironment.CurrentClient);
		}
		public static TextChannel Get(ulong channelId, [CanBeNull] Client client) {
			return GetAsync(channelId, client).Await();
		}
		public static TextChannel Get(ulong channelId) {
			return GetAsync(channelId).Await();
		}
		[CanBeNull]
		public static TextChannel GetCached(ulong channelId) {
			return FactoryUtils.GetCached(DirectMessageTextChannel._cache, channelId) ?? (TextChannel)FactoryUtils.GetCached(ServerTextChannel._cache, channelId);
		}
		public ulong Id {
			get;
		}
		public ChannelType Type {
			get;
		}
		// Don't call these from outside derived classes
		internal static void Populate(TextChannel obj, ChannelJson json, [CanBeNull] object state) {
			obj.Populate(json, state);
		}
		internal abstract void Populate(ChannelJson json, [CanBeNull] object state);
		public abstract override string ToString();
		public abstract void Uncache();
		public async Task PullUpdateAsync([CanBeNull] Client client) {
			client = client ?? DiscordEnvironment.CurrentClient;
			FactoryUtils.ValidateInEnv(client);
			await FactoryUtils.UpdateAsync(this, null, client.GetChannelJsonAsync, Populate);
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
		public async Task SendMessageAsync(string msg) {
			// TODO: On send message API endpoint
			await Task.Run(() => null);
		}
		public async Task SendUnformattedMessageAsync(string msg) {
			await SendMessageAsync(MessageBuilder.Escape(msg));
		}
		public void SendMessage(string msg) {
			SendMessageAsync(msg).Await();
		}
		public void SendUnformattedMessage(string msg) {
			SendUnformattedMessageAsync(msg).Await();
		}
	}
}
