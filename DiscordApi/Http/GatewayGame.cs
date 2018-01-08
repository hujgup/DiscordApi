using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Echo.Utils.Json;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;

namespace Echo.Discord.Api.Http {
	internal class GatewayGame {
		/// <summary>
		/// Creates a new <see cref="GatewayGame"/> object with <see cref="GameState.Streaming"/> as the state.
		/// </summary>
		/// <param name="name">The name of the game being streamed.</param>
		/// <param name="twitchUrl">The URL to the client's Twitch channel.</param>
		public GatewayGame(string name, string twitchUrl) {
			Name = name;
			State = GameState.Streaming;
			TwitchUrl = twitchUrl;
		}
		/// <summary>
		/// Creates a new <see cref="GatewayGame"/> object.
		/// </summary>
		/// <param name="name">The name of the game/song/media being played/heard/watched.</param>
		/// <param name="state">What activity the client is participating in. Cannot be <see cref="GameState.Streaming"/>.</param>
		public GatewayGame(string name, GameState state) {
			Name = name;
			State = state;
			TwitchUrl = null;
		}
		public string Name {
			get;
		}
		public GameState State {
			get;
		}
		[CanBeNull]
		public string TwitchUrl {
			get;
		}
		public static explicit operator JObject(GatewayGame data) {
			JsonObjectBuilder<JObject> b = new RootJsonObjectBuilder()
				.StringProperty("name", data.Name)
				.Int64Property("type", (long)data.State);
			if (data.State == GameState.Streaming) {
				// ReSharper disable once AssignNullToNotNullAttribute (Constructor enforces this constraint)
				b.StringProperty("url", data.TwitchUrl);
			}
			return b.Build();
		}
	}
}
