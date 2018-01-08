using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Echo.Utils;
using Echo.Discord.Api.Http;
using Echo.Discord.Api.Messages;
using Echo.Discord.Api.Servers;
using JetBrains.Annotations;

namespace Echo.Discord.Api {
	public class LazyServerEmoji : Emoji, IIdentifiable {
		public LazyServerEmoji(ulong emojiId, string name) {
			Id = emojiId;
			Name = name;
			IsGlobal = false;
		}
		public ulong Id {
			get;
		}
		public override Ternary IsMentionable {
			get => Ternary.Maybe;
		}
		public override string MentionContent {
			get => MessageBuilder.MentionOpen + ":" + Name + ":" + Id + MessageBuilder.MentionOpen;
		}
		public override string MentionFallbackName {
			get => ":" + Name + ":";
		}
		public virtual async Task<ServerEmoji> ToServerEmojiAsync(ulong serverId, [CanBeNull] Client client) {
			return await ServerEmoji.GetAsync(Id, serverId, client);
		}
		public virtual async Task<ServerEmoji> ToServerEmojiAsync(Server server, [CanBeNull] Client client) {
			return await ServerEmoji.GetAsync(Id, server, client);
		}
		public virtual async Task<ServerEmoji> ToServerEmojiAsync(ulong serverId) {
			return await ServerEmoji.GetAsync(Id, serverId);
		}
		public virtual async Task<ServerEmoji> ToServerEmojiAsync(Server server) {
			return await ServerEmoji.GetAsync(Id, server);
		}
		public virtual ServerEmoji ToServerEmoji(ulong serverId, [CanBeNull] Client client) {
			return ServerEmoji.Get(Id, serverId, client);
		}
		public virtual ServerEmoji ToServerEmoji(Server server, [CanBeNull] Client client) {
			return ServerEmoji.Get(Id, server, client);
		}
		public virtual ServerEmoji ToServerEmoji(ulong serverId) {
			return ServerEmoji.Get(Id, serverId);
		}
		public virtual ServerEmoji ToServerEmoji(Server server) {
			return ServerEmoji.Get(Id, server);
		}
		public virtual ServerEmoji CachedServerEmoji() {
			return ServerEmoji.GetCached(Id);
		}
		public override async Task<bool> IsMentionableByAsync(ulong userId, ulong inServerId, [CanBeNull] Client client) {
			LazyUser u = LazyUser.Get(userId);
			ServerEmoji fill = await ToServerEmojiAsync(inServerId, client);
			return (await u.IsInServerAsync(inServerId, client)) ? u.GetServerData(inServerId).Roles.Any(kvp => fill.UsableRoles.ContainsKey(kvp.Key)) : false;
		}
		public override bool Equals([CanBeNull] Emoji ej) {
			return ej is LazyServerEmoji emoji && Id == emoji.Id;
		}
		public override int GetHashCode() {
			return Id.GetHashCode();
		}
		public override string ToString() {
			return ":" + Name + ":";
		}
	}
}
