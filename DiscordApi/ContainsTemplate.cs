using System.Collections.Generic;

namespace Echo.Discord.Api.Channels {
	public interface IHasChannel<T> where T : IChannel {
		ulong ChannelId {
			get;
		}
		T Channel {
			get;
		}
	}
	public interface IHasCachedChannel<T> where T : IChannel {
		ulong ChannelId {
			get;
		}
		CachedPromise<T> Channel {
			get;
		}
	}
	public interface IHasManyChannels<T> where T : IChannel {
		IReadOnlyDictionary<ulong, T> Channels {
			get;
		}
	}
	public interface IHasManyCachedChannels<T> where T : IChannel {
		IReadOnlyDictionary<ulong, CachedPromise<T>> Channels {
			get;
		}
	}
	public interface IHasCategory {
		bool HasCategory {
			get;
		}
		ulong CategoryId {
			get;
		}
		ChannelCategory Category {
			get;
		}
	}
	public interface IHasCachedCategory {
		bool HasCategory {
			get;
		}
		ulong CategoryId {
			get;
		}
		CachedPromise<ChannelCategory> Category {
			get;
		}
	}
	public interface IHasAfkChannel {
		bool HasAfkChannel {
			get;
		}
		ulong AfkChannelId {
			get;
		}
		IChannel AfkChannel {
			get;
		}
	}
	public interface IHasCachedAfkChannel {
		bool HasAfkChannel {
			get;
		}
		ulong AfkChannelId {
			get;
		}
		CachedPromise<IChannel> AfkChannel {
			get;
		}
	}
}

namespace Echo.Discord.Api.Messages {
	public interface IHasMessage<T> where T : Message {
		ulong MessageId {
			get;
		}
		T Message {
			get;
		}
	}
	public interface IHasCachedMessage<T> where T : Message {
		ulong MessageId {
			get;
		}
		CachedPromise<T> Message {
			get;
		}
	}
}

namespace Echo.Discord.Api.Servers {
	public interface IHasServer {
		ulong ServerId {
			get;
		}
		Server Server {
			get;
		}
	}
	public interface IHasCachedServer {
		ulong ServerId {
			get;
		}
		CachedPromise<Server> Server {
			get;
		}
	}
	public interface IHasManyServers {
		IReadOnlyDictionary<ulong, Server> Servers {
			get;
		}
	}
	public interface IHasManyCachedServers {
		IReadOnlyDictionary<ulong, CachedPromise<Server>> Servers {
			get;
		}
	}
}

namespace Echo.Discord.Api {
	public interface IHasOwner {
		ulong OwnerId {
			get;
		}
		User Owner {
			get;
		}
	}
	public interface IHasCachedOwner {
		ulong OwnerId {
			get;
		}
		CachedPromise<User> Owner {
			get;
		}
		LazyUser LazyOwner {
			get;
		}
	}
	public interface IHasManyMembers {
		IReadOnlyDictionary<ulong, User> Members {
			get;
		}
	}
	public interface IHasManyCachedMembers {
		IReadOnlyDictionary<ulong, CachedPromise<User>> Members {
			get;
		}
		IReadOnlyDictionary<ulong, LazyUser> LazyMembers {
			get;
		}
	}
	public interface IHasManyMentionedUsers {
		IReadOnlyDictionary<ulong, User> MentionedUsers {
			get;
		}
	}
	public interface IHasManyCachedMentionedUsers {
		IReadOnlyDictionary<ulong, CachedPromise<User>> MentionedUsers {
			get;
		}
		IReadOnlyDictionary<ulong, LazyUser> LazyMentionedUsers {
			get;
		}
	}
	public interface IHasCreator {
		bool HasCreator {
			get;
		}
		ulong CreatorId {
			get;
		}
		User Creator {
			get;
		}
	}
	public interface IHasCachedCreator {
		bool HasCreator {
			get;
		}
		ulong CreatorId {
			get;
		}
		CachedPromise<User> Creator {
			get;
		}
		LazyUser LazyCreator {
			get;
		}
	}
	public interface IHasRelatedUser {
		ulong RelatedUserId {
			get;
		}
		User RelatedUser {
			get;
		}
	}
	public interface IHasCachedRelatedUser {
		ulong RelatedUserId {
			get;
		}
		CachedPromise<User> RelatedUser {
			get;
		}
		LazyUser LazyRelatedUser {
			get;
		}
	}
	public interface IHasManyRoles {
		IReadOnlyDictionary<ulong, Role> Roles {
			get;
		}
	}
	public interface IHasManyCachedRoles {
		IReadOnlyDictionary<ulong, CachedPromise<Role>> Roles {
			get;
		}
	}
	public interface IHasManyMentionedRoles {
		IReadOnlyDictionary<ulong, Role> MentionedRoles {
			get;
		}
	}
	public interface IHasManyCachedMentionedRoles {
		IReadOnlyDictionary<ulong, CachedPromise<Role>> MentionedRoles {
			get;
		}
	}
	public interface IHasManyUsableRoles {
		IReadOnlyDictionary<ulong, Role> UsableRoles {
			get;
		}
	}
	public interface IHasManyCachedUsableRoles {
		IReadOnlyDictionary<ulong, CachedPromise<Role>> UsableRoles {
			get;
		}
	}
	public interface IHasManyEmojis {
		IReadOnlyDictionary<ulong, ServerEmoji> Emojis {
			get;
		}
	}
	public interface IHasManyCachedEmojis {
		IReadOnlyDictionary<ulong, CachedPromise<ServerEmoji>> Emojis {
			get;
		}
		IReadOnlyDictionary<ulong, LazyServerEmoji> LazyEmojis {
			get;
		}
	}
	public interface IHasManyPresences {
		IReadOnlyDictionary<ulong, Presence> Presences {
			get;
		}
	}
	public interface IHasManyCachedPresences {
		IReadOnlyDictionary<ulong, CachedPromise<Presence>> Presences {
			get;
		}
	}
}
