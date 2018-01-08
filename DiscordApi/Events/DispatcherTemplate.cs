using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Echo.Utils;
using Echo.Utils.Async;
using Echo.Discord.Api.Http;
using Echo.Discord.Api.Channels;
using Echo.Discord.Api.Servers;
using Echo.Discord.Api.Messages;

namespace Echo.Discord.Api.Events {
	internal partial class Dispatcher {
		// TODO: Events for other things
		private void SetUpChannelEvents() {
			// CHANNEL EVENT: Create
			_listener.Listen(data => {
				var d = (GatewayEvent.Channel)data.Data;
				var args = new ChannelEventArgs(d.Data);
				
				var b = new EventBubbler<ChannelEventArgs>();
				Server i0;
				// ReSharper disable once SwitchStatementMissingSomeCases (ignoring ServerVoice)
				switch (d.Data.Type) {
					case ChannelType.DirectMessage:
						b.Append(DirectMessageTextChannel.OnCreate);
						b.Append(TextChannel.OnCreate);
						b.Append(ChannelUtils.OnDirectMessageChannelCreate);
						b.Append(ChannelUtils.OnChannelCreate);
						b.Append(ChannelUtils.OnCreate);
						break;
					case ChannelType.GroupDirectMessage:
						b.Append(DirectMessageTextChannel.OnCreate);
						b.Append(TextChannel.OnCreate);
						b.Append(ChannelUtils.OnDirectMessageChannelCreate);
						b.Append(ChannelUtils.OnChannelCreate);
						b.Append(ChannelUtils.OnCreate);
						break;
					case ChannelType.ServerText:
						i0 = ((IServerChannel)d.Data).Server.GetCachedValue();
						if (i0 != null) {
							b.Append(i0.OnTextChannelCreate);
						}
						if (i0 != null) {
							b.Append(i0.OnNonCatChannelCreate);
						}
						if (i0 != null) {
							b.Append(i0.OnChannelCreate);
						}
						b.Append(ServerTextChannel.OnCreate);
						b.Append(TextChannel.OnCreate);
						b.Append(ChannelUtils.OnNonCatServerChannelCreate);
						b.Append(ChannelUtils.OnServerChannelCreate);
						b.Append(ChannelUtils.OnChannelCreate);
						b.Append(ChannelUtils.OnCreate);
						break;
					case ChannelType.ServerCategory:
						i0 = ((IServerChannel)d.Data).Server.GetCachedValue();
						if (i0 != null) {
							b.Append(i0.OnCategoryCreate);
						}
						if (i0 != null) {
							b.Append(i0.OnChannelCreate);
						}
						b.Append(ChannelCategory.OnCreate);
						b.Append(ChannelUtils.OnServerChannelCreate);
						b.Append(ChannelUtils.OnCreate);
						break;
				}
				b.Invoke(_client, args);
				
			}, ResponseListener.Unbounded, GatewayOpCode.Dispatch, GatewayEvent.Channel.CreateId);
			// CHANNEL EVENT: Delete
			_listener.Listen(data => {
				var d = (GatewayEvent.Channel)data.Data;
				var args = new ChannelEventArgs(d.Data);
				
				var b = new EventBubbler<ChannelEventArgs>();
				b.Append(((IInternalChannel)d.Data).OnDelete);
				Server i0;
				// ReSharper disable once SwitchStatementMissingSomeCases (ignoring ServerVoice)
				switch (d.Data.Type) {
					case ChannelType.DirectMessage:
						b.Append(DirectMessageTextChannel.OnDeleteAny);
						b.Append(TextChannel.OnDeleteAny);
						b.Append(ChannelUtils.OnDirectMessageChannelDelete);
						b.Append(ChannelUtils.OnChannelDelete);
						b.Append(ChannelUtils.OnDelete);
						break;
					case ChannelType.GroupDirectMessage:
						b.Append(DirectMessageTextChannel.OnDeleteAny);
						b.Append(TextChannel.OnDeleteAny);
						b.Append(ChannelUtils.OnDirectMessageChannelDelete);
						b.Append(ChannelUtils.OnChannelDelete);
						b.Append(ChannelUtils.OnDelete);
						break;
					case ChannelType.ServerText:
						i0 = ((IServerChannel)d.Data).Server.GetCachedValue();
						if (i0 != null) {
							b.Append(i0.OnTextChannelDelete);
						}
						if (i0 != null) {
							b.Append(i0.OnNonCatChannelDelete);
						}
						if (i0 != null) {
							b.Append(i0.OnChannelDelete);
						}
						b.Append(ServerTextChannel.OnDeleteAny);
						b.Append(TextChannel.OnDeleteAny);
						b.Append(ChannelUtils.OnNonCatServerChannelDelete);
						b.Append(ChannelUtils.OnServerChannelDelete);
						b.Append(ChannelUtils.OnChannelDelete);
						b.Append(ChannelUtils.OnDelete);
						break;
					case ChannelType.ServerCategory:
						i0 = ((IServerChannel)d.Data).Server.GetCachedValue();
						if (i0 != null) {
							b.Append(i0.OnCategoryDelete);
						}
						if (i0 != null) {
							b.Append(i0.OnChannelDelete);
						}
						b.Append(ChannelCategory.OnDeleteAny);
						b.Append(ChannelUtils.OnServerChannelDelete);
						b.Append(ChannelUtils.OnDelete);
						break;
				}
				b.Invoke(_client, args);
				d.Data.Uncache();
			}, ResponseListener.Unbounded, GatewayOpCode.Dispatch, GatewayEvent.Channel.DeleteId);
			// CHANNEL EVENT: Update
			_listener.Listen(data => {
				var d = (GatewayEvent.Channel)data.Data;
				var args = new ChannelEventArgs(d.Data);
				
				var b = new EventBubbler<ChannelEventArgs>();
				b.Append(((IInternalChannel)d.Data).OnUpdate);
				Server i0;
				// ReSharper disable once SwitchStatementMissingSomeCases (ignoring ServerVoice)
				switch (d.Data.Type) {
					case ChannelType.DirectMessage:
						b.Append(DirectMessageTextChannel.OnUpdateAny);
						b.Append(TextChannel.OnUpdateAny);
						b.Append(ChannelUtils.OnDirectMessageChannelUpdate);
						b.Append(ChannelUtils.OnChannelUpdate);
						b.Append(ChannelUtils.OnUpdate);
						break;
					case ChannelType.GroupDirectMessage:
						b.Append(DirectMessageTextChannel.OnUpdateAny);
						b.Append(TextChannel.OnUpdateAny);
						b.Append(ChannelUtils.OnDirectMessageChannelUpdate);
						b.Append(ChannelUtils.OnChannelUpdate);
						b.Append(ChannelUtils.OnUpdate);
						break;
					case ChannelType.ServerText:
						i0 = ((IServerChannel)d.Data).Server.GetCachedValue();
						if (i0 != null) {
							b.Append(i0.OnTextChannelUpdate);
						}
						if (i0 != null) {
							b.Append(i0.OnNonCatChannelUpdate);
						}
						if (i0 != null) {
							b.Append(i0.OnChannelUpdate);
						}
						b.Append(ServerTextChannel.OnUpdateAny);
						b.Append(TextChannel.OnUpdateAny);
						b.Append(ChannelUtils.OnNonCatServerChannelUpdate);
						b.Append(ChannelUtils.OnServerChannelUpdate);
						b.Append(ChannelUtils.OnChannelUpdate);
						b.Append(ChannelUtils.OnUpdate);
						break;
					case ChannelType.ServerCategory:
						i0 = ((IServerChannel)d.Data).Server.GetCachedValue();
						if (i0 != null) {
							b.Append(i0.OnCategoryUpdate);
						}
						if (i0 != null) {
							b.Append(i0.OnChannelUpdate);
						}
						b.Append(ChannelCategory.OnUpdateAny);
						b.Append(ChannelUtils.OnServerChannelUpdate);
						b.Append(ChannelUtils.OnUpdate);
						break;
				}
				b.Invoke(_client, args);
				
			}, ResponseListener.Unbounded, GatewayOpCode.Dispatch, GatewayEvent.Channel.UpdateId);
		}

	}
}