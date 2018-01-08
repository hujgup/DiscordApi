using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Echo.Utils.Async;
using Echo.Discord.Api.Http;
using JetBrains.Annotations;

namespace Echo.Discord.Api {
	public static class IHasIconExtensions {
		[ItemCanBeNull]
		public static async Task<Image> GetIconAsync(this IHasIcon i, [CanBeNull] Client client) {
			client = client ?? DiscordEnvironment.CurrentClient;
			return client == null ? null : await client.GetIconAsync(i);
		}
		public static Image GetIcon(this IHasIcon i, [CanBeNull] Client client) {
			return GetIconAsync(i, client).Await();
		}
	}
}
