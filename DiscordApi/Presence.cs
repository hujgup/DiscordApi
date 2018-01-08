using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Echo.Discord.Api.Json;

namespace Echo.Discord.Api {
	//  : IIdentifiable
	public class Presence : IIdentifiable {
		//private static readonly Cache<Presence> cache = new Cache<Presence>();
		internal Presence(PresenceUpdateJson json) {
			// TODO: Presence constructor
			UserId = json.user.id;
		}
		public ulong UserId {
			get;
		}
		ulong IIdentifiable.Id {
			get => UserId;
		}
	}
}
