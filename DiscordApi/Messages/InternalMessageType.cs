using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Echo.Discord.Api.Messages {
	internal enum InternalMessageType {
		Default = 0,
		RecipientAdd = 1,
		RecipientRemove = 2,
		Call = 3,
		ChannelNameChange = 4,
		ChannelIconChange = 5,
		ChannelPinnedMessage = 6,
		GuildMemberJoin = 7
	}
}
