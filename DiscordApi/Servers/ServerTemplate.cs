using System;
using Echo.Utils;
using Echo.Discord.Api.Events;
using Echo.Discord.Api.Channels;

namespace Echo.Discord.Api.Servers {
	public partial class Server {
		internal void OnChannelCreate(object sender, ChannelEventArgs e) {
			ChannelCreate?.Invoke(sender, e.Convert<IServerChannel>());
		}
		internal void OnNonCatChannelCreate(object sender, ChannelEventArgs e) {
			NonCatChannelCreate?.Invoke(sender, e.Convert<INonCatServerChannel>());
		}
		internal void OnCategoryCreate(object sender, ChannelEventArgs e) {
			CategoryCreate?.Invoke(sender, e.Convert<ChannelCategory>());
		}
		internal void OnTextChannelCreate(object sender, ChannelEventArgs e) {
			TextChannelCreate?.Invoke(sender, e.Convert<ServerTextChannel>());
		}
		public event EventHandler<ChannelEventArgs<IServerChannel>> ChannelCreate;
		public event EventHandler<ChannelEventArgs<INonCatServerChannel>> NonCatChannelCreate;
		public event EventHandler<ChannelEventArgs<ChannelCategory>> CategoryCreate;
		public event EventHandler<ChannelEventArgs<ServerTextChannel>> TextChannelCreate;
		internal void OnChannelUpdate(object sender, ChannelEventArgs e) {
			ChannelUpdate?.Invoke(sender, e.Convert<IServerChannel>());
		}
		internal void OnNonCatChannelUpdate(object sender, ChannelEventArgs e) {
			NonCatChannelUpdate?.Invoke(sender, e.Convert<INonCatServerChannel>());
		}
		internal void OnCategoryUpdate(object sender, ChannelEventArgs e) {
			CategoryUpdate?.Invoke(sender, e.Convert<ChannelCategory>());
		}
		internal void OnTextChannelUpdate(object sender, ChannelEventArgs e) {
			TextChannelUpdate?.Invoke(sender, e.Convert<ServerTextChannel>());
		}
		public event EventHandler<ChannelEventArgs<IServerChannel>> ChannelUpdate;
		public event EventHandler<ChannelEventArgs<INonCatServerChannel>> NonCatChannelUpdate;
		public event EventHandler<ChannelEventArgs<ChannelCategory>> CategoryUpdate;
		public event EventHandler<ChannelEventArgs<ServerTextChannel>> TextChannelUpdate;
		internal void OnChannelDelete(object sender, ChannelEventArgs e) {
			ChannelDelete?.Invoke(sender, e.Convert<IServerChannel>());
		}
		internal void OnNonCatChannelDelete(object sender, ChannelEventArgs e) {
			NonCatChannelDelete?.Invoke(sender, e.Convert<INonCatServerChannel>());
		}
		internal void OnCategoryDelete(object sender, ChannelEventArgs e) {
			CategoryDelete?.Invoke(sender, e.Convert<ChannelCategory>());
		}
		internal void OnTextChannelDelete(object sender, ChannelEventArgs e) {
			TextChannelDelete?.Invoke(sender, e.Convert<ServerTextChannel>());
		}
		public event EventHandler<ChannelEventArgs<IServerChannel>> ChannelDelete;
		public event EventHandler<ChannelEventArgs<INonCatServerChannel>> NonCatChannelDelete;
		public event EventHandler<ChannelEventArgs<ChannelCategory>> CategoryDelete;
		public event EventHandler<ChannelEventArgs<ServerTextChannel>> TextChannelDelete;
	}
}