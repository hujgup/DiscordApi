using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Echo.Discord.Api.Servers;
using Echo.Discord.Api.Http;

namespace Echo.Discord.Api.Channels {
	public interface IServerChannel : IComparable<IServerChannel>, IChannel, INamed, IHasCachedServer {
		int Position {
			get;
		}
		PermissionMask Permissions {
			get;
		}
	}
}
