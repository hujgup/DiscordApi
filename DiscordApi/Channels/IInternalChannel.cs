using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Echo.Discord.Api.Events;

namespace Echo.Discord.Api.Channels {
	internal interface IInternalChannel : IChannel {
		void OnDelete(object sender, ChannelEventArgs e);
		void OnUpdate(object sender, ChannelEventArgs e);
	}
}
