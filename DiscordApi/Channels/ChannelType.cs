using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Echo.Discord.Api.Channels {
	public enum ChannelType : int {
		ServerText = 0,
		DirectMessage = 1,
		[Obsolete("Voice channels are not supported.")]
		ServerVoice = 2,
		GroupDirectMessage = 3,
		ServerCategory = 4
	}
}
