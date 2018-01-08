using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Echo.Discord.Api.Events;
using Echo.Discord.Api.Http;

namespace Echo.Discord.Api.Channels {
	public interface IChannel : IEquatable<IChannel>, ICached, IAlias {
		ChannelType Type {
			get;
		}
		event EventHandler<ChannelEventArgs> Delete;
		event EventHandler<ChannelEventArgs> Update;
	}
}
