using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Echo.Utils.Json;
using Newtonsoft.Json.Linq;

namespace Echo.Discord.Api.Http {
	internal class GatewayIdentifyData {
		public GatewayIdentifyData(Client client, bool allowCompression, int offlineUserCutoffThreshold, int shardId, int shardCount, GatewayUpdateStatus updateStatus) {
			if (offlineUserCutoffThreshold < 50 || offlineUserCutoffThreshold > 250) {
				throw new ArgumentOutOfRangeException("Parameter " + nameof(offlineUserCutoffThreshold) + " must be in range [50, 250].");
			}
			AuthentToken = client.Token;
			AllowCompression = allowCompression;
			OfflineUserCutoffThreshold = offlineUserCutoffThreshold;
			ShardId = shardId;
			ShardCount = shardCount;
			UpdateStatus = updateStatus;
		}
		public string AuthentToken {
			get;
		}
		public bool AllowCompression {
			get;
		}
		public int OfflineUserCutoffThreshold {
			get;
		}
		public int ShardId {
			get;
		}
		public int ShardCount {
			get;
		}
		public GatewayUpdateStatus UpdateStatus {
			get;
		}
		public static explicit operator JObject(GatewayIdentifyData data) {
			return new RootJsonObjectBuilder()
				.StringProperty("token", data.AuthentToken)
				.BooleanProperty("compress", data.AllowCompression)
				.Int32Property("large_threshold", data.OfflineUserCutoffThreshold)
				.ArrayProperty("shard")
					.AddInt32(data.ShardId)
					.AddInt32(data.ShardCount)
					.Build()
				.ObjectProperty("presence", (JObject)data.UpdateStatus)
				.ObjectProperty("properties")
					.StringProperty("$os", Environment.OSVersion.VersionString)
					.StringProperty("$browser", Info.LibraryName)
					.StringProperty("$device", Info.LibraryName)
					.Build()
				.Build();
		}
	}
}
