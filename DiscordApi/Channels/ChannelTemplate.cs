using System;
using Echo.Discord.Api.Events;
using JetBrains.Annotations;

namespace Echo.Discord.Api.Channels {
	public partial class ChannelCategory {
		public bool Equals([CanBeNull] IChannel channel) {
			return ChannelUtils.ChannelEquals(this, channel);
		}
		public override bool Equals([CanBeNull] object obj) {
			return ChannelUtils.ChannelEquals(this, obj);
		}
		public override int GetHashCode() {
			return Id.GetHashCode();
		}
		public static bool operator ==([CanBeNull] ChannelCategory a, [CanBeNull] IChannel b) {
			return ChannelUtils.ChannelEquals(a, b);
		}
		public static bool operator !=([CanBeNull] ChannelCategory a, [CanBeNull] IChannel b) {
			return !(a == b);
		}
	}
	public partial class TextChannel {
		public bool Equals([CanBeNull] IChannel channel) {
			return ChannelUtils.ChannelEquals(this, channel);
		}
		public override bool Equals([CanBeNull] object obj) {
			return ChannelUtils.ChannelEquals(this, obj);
		}
		public override int GetHashCode() {
			return Id.GetHashCode();
		}
		public static bool operator ==([CanBeNull] TextChannel a, [CanBeNull] IChannel b) {
			return ChannelUtils.ChannelEquals(a, b);
		}
		public static bool operator !=([CanBeNull] TextChannel a, [CanBeNull] IChannel b) {
			return !(a == b);
		}
	}
	public partial class ChannelCategory {
		public int CompareTo([CanBeNull] IServerChannel channel) {
			return ChannelUtils.Compare(this, channel);
		}
		public static bool operator <([CanBeNull] ChannelCategory a, [CanBeNull] IServerChannel b) {
			return ChannelUtils.Compare(a, b) < 0;
		}
		public static bool operator <=([CanBeNull] ChannelCategory a, [CanBeNull] IServerChannel b) {
			return ChannelUtils.Compare(a, b) <= 0;
		}
		public static bool operator >([CanBeNull] ChannelCategory a, [CanBeNull] IServerChannel b) {
			return ChannelUtils.Compare(a, b) > 0;
		}
		public static bool operator >=([CanBeNull] ChannelCategory a, [CanBeNull] IServerChannel b) {
			return ChannelUtils.Compare(a, b) >= 0;
		}
	}
	public partial class ServerTextChannel {
		public int CompareTo([CanBeNull] IServerChannel channel) {
			return ChannelUtils.Compare(this, channel);
		}
		public static bool operator <([CanBeNull] ServerTextChannel a, [CanBeNull] IServerChannel b) {
			return ChannelUtils.Compare(a, b) < 0;
		}
		public static bool operator <=([CanBeNull] ServerTextChannel a, [CanBeNull] IServerChannel b) {
			return ChannelUtils.Compare(a, b) <= 0;
		}
		public static bool operator >([CanBeNull] ServerTextChannel a, [CanBeNull] IServerChannel b) {
			return ChannelUtils.Compare(a, b) > 0;
		}
		public static bool operator >=([CanBeNull] ServerTextChannel a, [CanBeNull] IServerChannel b) {
			return ChannelUtils.Compare(a, b) >= 0;
		}
	}
	public partial class ChannelCategory {
		internal static void OnCreate(object sender, ChannelEventArgs e) {
			Create?.Invoke(sender, e.Convert<ChannelCategory>());
		}
		public static event EventHandler<ChannelEventArgs<ChannelCategory>> Create;
		internal static void OnUpdateAny(object sender, ChannelEventArgs e) {
			UpdateAny?.Invoke(sender, e.Convert<ChannelCategory>());
		}
		public static event EventHandler<ChannelEventArgs<ChannelCategory>> UpdateAny;
		internal static void OnDeleteAny(object sender, ChannelEventArgs e) {
			DeleteAny?.Invoke(sender, e.Convert<ChannelCategory>());
		}
		public static event EventHandler<ChannelEventArgs<ChannelCategory>> DeleteAny;
	}
	public partial class ServerTextChannel {
		internal static new void OnCreate(object sender, ChannelEventArgs e) {
			Create?.Invoke(sender, e.Convert<ServerTextChannel>());
		}
		public static new event EventHandler<ChannelEventArgs<ServerTextChannel>> Create;
		internal static new void OnUpdateAny(object sender, ChannelEventArgs e) {
			UpdateAny?.Invoke(sender, e.Convert<ServerTextChannel>());
		}
		public static new event EventHandler<ChannelEventArgs<ServerTextChannel>> UpdateAny;
		internal static new void OnDeleteAny(object sender, ChannelEventArgs e) {
			DeleteAny?.Invoke(sender, e.Convert<ServerTextChannel>());
		}
		public static new event EventHandler<ChannelEventArgs<ServerTextChannel>> DeleteAny;
	}
	public partial class DirectMessageTextChannel {
		internal static new void OnCreate(object sender, ChannelEventArgs e) {
			Create?.Invoke(sender, e.Convert<DirectMessageTextChannel>());
		}
		public static new event EventHandler<ChannelEventArgs<DirectMessageTextChannel>> Create;
		internal static new void OnUpdateAny(object sender, ChannelEventArgs e) {
			UpdateAny?.Invoke(sender, e.Convert<DirectMessageTextChannel>());
		}
		public static new event EventHandler<ChannelEventArgs<DirectMessageTextChannel>> UpdateAny;
		internal static new void OnDeleteAny(object sender, ChannelEventArgs e) {
			DeleteAny?.Invoke(sender, e.Convert<DirectMessageTextChannel>());
		}
		public static new event EventHandler<ChannelEventArgs<DirectMessageTextChannel>> DeleteAny;
	}
	public partial class TextChannel {
		internal static void OnCreate(object sender, ChannelEventArgs e) {
			Create?.Invoke(sender, e.Convert<TextChannel>());
		}
		public static event EventHandler<ChannelEventArgs<TextChannel>> Create;
		internal static void OnUpdateAny(object sender, ChannelEventArgs e) {
			UpdateAny?.Invoke(sender, e.Convert<TextChannel>());
		}
		public static event EventHandler<ChannelEventArgs<TextChannel>> UpdateAny;
		internal static void OnDeleteAny(object sender, ChannelEventArgs e) {
			DeleteAny?.Invoke(sender, e.Convert<TextChannel>());
		}
		public static event EventHandler<ChannelEventArgs<TextChannel>> DeleteAny;
	}
	public partial class ChannelCategory : IInternalChannel {
		void IInternalChannel.OnUpdate(object sender, ChannelEventArgs e) {
			Update?.Invoke(sender, e);
		}
		public event EventHandler<ChannelEventArgs> Update;
		void IInternalChannel.OnDelete(object sender, ChannelEventArgs e) {
			Delete?.Invoke(sender, e);
		}
		public event EventHandler<ChannelEventArgs> Delete;
	}
	public partial class TextChannel : IInternalChannel {
		void IInternalChannel.OnUpdate(object sender, ChannelEventArgs e) {
			Update?.Invoke(sender, e);
		}
		public event EventHandler<ChannelEventArgs> Update;
		void IInternalChannel.OnDelete(object sender, ChannelEventArgs e) {
			Delete?.Invoke(sender, e);
		}
		public event EventHandler<ChannelEventArgs> Delete;
	}
	public static partial class ChannelUtils {
		internal static void OnCreate(object sender, ChannelEventArgs e) {
			Create?.Invoke(sender, e);
		}
		public static event EventHandler<ChannelEventArgs> Create;
		internal static void OnChannelCreate(object sender, ChannelEventArgs e) {
			ChannelCreate?.Invoke(sender, e);
		}
		public static event EventHandler<ChannelEventArgs> ChannelCreate;
		internal static void OnServerChannelCreate(object sender, ChannelEventArgs e) {
			ServerChannelCreate?.Invoke(sender, e.Convert<IServerChannel>());
		}
		public static event EventHandler<ChannelEventArgs<IServerChannel>> ServerChannelCreate;
		internal static void OnNonCatServerChannelCreate(object sender, ChannelEventArgs e) {
			NonCatServerChannelCreate?.Invoke(sender, e.Convert<INonCatServerChannel>());
		}
		public static event EventHandler<ChannelEventArgs<INonCatServerChannel>> NonCatServerChannelCreate;
		internal static void OnDirectMessageChannelCreate(object sender, ChannelEventArgs e) {
			DirectMessageChannelCreate?.Invoke(sender, e.Convert<IDirectMessageChannel>());
		}
		public static event EventHandler<ChannelEventArgs<IDirectMessageChannel>> DirectMessageChannelCreate;
		internal static void OnUpdate(object sender, ChannelEventArgs e) {
			Update?.Invoke(sender, e);
		}
		public static event EventHandler<ChannelEventArgs> Update;
		internal static void OnChannelUpdate(object sender, ChannelEventArgs e) {
			ChannelUpdate?.Invoke(sender, e);
		}
		public static event EventHandler<ChannelEventArgs> ChannelUpdate;
		internal static void OnServerChannelUpdate(object sender, ChannelEventArgs e) {
			ServerChannelUpdate?.Invoke(sender, e.Convert<IServerChannel>());
		}
		public static event EventHandler<ChannelEventArgs<IServerChannel>> ServerChannelUpdate;
		internal static void OnNonCatServerChannelUpdate(object sender, ChannelEventArgs e) {
			NonCatServerChannelUpdate?.Invoke(sender, e.Convert<INonCatServerChannel>());
		}
		public static event EventHandler<ChannelEventArgs<INonCatServerChannel>> NonCatServerChannelUpdate;
		internal static void OnDirectMessageChannelUpdate(object sender, ChannelEventArgs e) {
			DirectMessageChannelUpdate?.Invoke(sender, e.Convert<IDirectMessageChannel>());
		}
		public static event EventHandler<ChannelEventArgs<IDirectMessageChannel>> DirectMessageChannelUpdate;
		internal static void OnDelete(object sender, ChannelEventArgs e) {
			Delete?.Invoke(sender, e);
		}
		public static event EventHandler<ChannelEventArgs> Delete;
		internal static void OnChannelDelete(object sender, ChannelEventArgs e) {
			ChannelDelete?.Invoke(sender, e);
		}
		public static event EventHandler<ChannelEventArgs> ChannelDelete;
		internal static void OnServerChannelDelete(object sender, ChannelEventArgs e) {
			ServerChannelDelete?.Invoke(sender, e.Convert<IServerChannel>());
		}
		public static event EventHandler<ChannelEventArgs<IServerChannel>> ServerChannelDelete;
		internal static void OnNonCatServerChannelDelete(object sender, ChannelEventArgs e) {
			NonCatServerChannelDelete?.Invoke(sender, e.Convert<INonCatServerChannel>());
		}
		public static event EventHandler<ChannelEventArgs<INonCatServerChannel>> NonCatServerChannelDelete;
		internal static void OnDirectMessageChannelDelete(object sender, ChannelEventArgs e) {
			DirectMessageChannelDelete?.Invoke(sender, e.Convert<IDirectMessageChannel>());
		}
		public static event EventHandler<ChannelEventArgs<IDirectMessageChannel>> DirectMessageChannelDelete;
	}
}



