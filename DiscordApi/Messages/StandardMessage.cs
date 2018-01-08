using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Echo.Utils.Json;
using Echo.Discord.Api.Json;
using Echo.Discord.Api.Http;

namespace Echo.Discord.Api.Messages {
	public class StandardMessage : Message {
		internal StandardMessage(JsonObjectReader r) : base(r) {
			Author = User.CreateFromJson(r.ReadObject<UserJson>("author"), null);
			Type = Author.IsBot ? MessageType.Bot : MessageType.User;
		}
		public User Author {
			get;
			private set;
		}
		public bool IsBot {
			get => Type == MessageType.Bot;
		}
		internal override void UpdateInstance(JsonObjectReader r) {
			base.UpdateInstance(r);
			if (r.Contains("author")) {
				Author = User.CreateFromJson(r.ReadObject<UserJson>("author"), null);
				Type = Author.IsBot ? MessageType.Bot : MessageType.User;
			}
		}
		public override string ToString() {
			return "[" + Channel + "] => " + Author.QualifiedName + ": " + Content;
		}
	}
}
