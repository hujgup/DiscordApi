using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Echo.Discord.Api.Channels;
using Echo.Discord.Api.Http;
using Echo.Discord.Api.Messages;
using JetBrains.Annotations;

namespace Echo.Discord.Api.Events {
	public partial class PinEventArgs {
		internal static List<PinnedMessageData> Create(ulong channelId, [CanBeNull] Client client) {
			// get message and channel
			ServerTextChannel c = ServerTextChannel.Get(channelId, client);
			PinnedMessageData d = c.GetPinnedMessages(client);
			return d.Split();
		}
	}
}
