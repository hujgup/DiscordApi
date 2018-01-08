using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Echo.Discord.Api.Http;

namespace Echo.Discord.Api.Channels {
	public interface INonCatServerChannel : IServerChannel, IHasCachedCategory {
	}
}
