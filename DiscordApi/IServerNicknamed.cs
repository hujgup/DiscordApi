using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Echo.Discord.Api.Servers;

namespace Echo.Discord.Api {
	interface IServerNicknamed {
		string GetNickname(ulong forServer);
		string GetNickname(Server forServer);
	}
}
