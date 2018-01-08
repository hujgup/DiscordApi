using System;
using Echo.Utils;
using Echo.Discord.Api.Channels;
using Echo.Discord.Api.Events;
using Echo.Discord.Api.Http;
using Echo.Discord.Api.Messages;
using Echo.Discord.Api.Servers;

namespace Echo.Discord.Api.Channels {
	public partial class ChannelCategory {
		public static event EventHandler<ChannelEventArgs<ChannelCategory>> Create {
			add {
				GatewayClient c = DiscordEnvironment.CurrentGatewayClient;
				if (c == null) {
					throw new NotInEnvironmentException("Automatic static event subscription requires the current thread to be in a DiscordEnvironment. Try using DiscordEnvironment's CreateSubthread or Run methods, or subscribing to the event ChannelCategoryCreate on the GatewayClient instance Events property.");
				} else {
					c.Events.ChannelCategoryCreate += value;
				}
			}
			remove {
				GatewayClient c = DiscordEnvironment.CurrentGatewayClient;
				if (c == null) {
					throw new NotInEnvironmentException("Automatic static event unsubscription requires the current thread to be in a DiscordEnvironment. Try using DiscordEnvironment's CreateSubthread or Run methods, or unsubscribing from the event ChannelCategoryCreate on the GatewayClient instance Events property.");
				} else {
					c.Events.ChannelCategoryCreate -= value;
				}
			}
		}
	}
}

namespace Echo.Discord.Api.Events {
	public class EventListener {
		internal void OnChannelCategoryCreate(object sender, ChannelEventArgs<ChannelCategory> e) {
			ChannelCategoryCreate?.Invoke(sender, e);
		}
		public event EventHandler<ChannelEventArgs<ChannelCategory>> ChannelCategoryCreate;
	}
}
