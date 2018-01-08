using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Echo.Utils.Json;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;

namespace Echo.Discord.Api.Http {
	internal class GatewayUpdateStatus {
		/// <summary>
		/// Creates a new <see cref="GatewayUpdateStatus" /> object.
		/// </summary>
		/// <param name="status">Client user status. Null for <see cref="GatewayUpdateStatus"/>-exclusive value "invisible".</param>
		/// <param name="isAfk">True if client is AFK.</param>
		/// <param name="idleStartTime">Unix time (in milliseconds) of when client went idle. Null if not idle.</param>
		/// <param name="game">Data re: game being played by client, or null if none.</param>
		public GatewayUpdateStatus(UserStatus? status, bool isAfk, ulong? idleStartTime, [CanBeNull] GatewayGame game) {
			Status = status;
			IsAfk = isAfk;
			IdleStartTime = idleStartTime;
			Game = game;
		}
		/// <summary>
		/// Client user status. Null for <see cref="GatewayUpdateStatus"/>-exclusive value "invisible".
		/// </summary>
		public UserStatus? Status {
			get;
		}
		public bool IsAfk {
			get;
		}
		/// <summary>
		/// Unix time (in milliseconds) of when client went idle. Null if not idle.
		/// </summary>
		public ulong? IdleStartTime {
			get;
		}
		[CanBeNull]
		public GatewayGame Game {
			get;
		}
		public static explicit operator JObject(GatewayUpdateStatus data) {
			JsonObjectBuilder<JObject> b = new RootJsonObjectBuilder()
				.StringProperty("status", data.Status != null ? StringEnum.GetValue((UserStatus)data.Status) : "invisible")
				.BooleanProperty("afk", data.IsAfk)
			    .UInt64Property("since", data.IdleStartTime);
			if (data.Game != null) {
				b.ObjectProperty("game", (JObject)data.Game);
			} else {
				b.NullProperty("game");
			}
			return b.Build();
		}
	}
}
