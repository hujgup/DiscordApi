using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Echo.Discord.Api.Json;

namespace Echo.Discord.Api {
	internal static class PermissionUtils {
		internal static PermissionMask GetMask(IEnumerable<OverwriteJson> list) {
			return list.Aggregate(PermissionMask.None, (mask, json) => mask);
		}
	}
}
