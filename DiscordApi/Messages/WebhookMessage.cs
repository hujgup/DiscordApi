using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Echo.Discord.Api.Http;
using Echo.Discord.Api.Json;
using Echo.Utils.Json;

namespace Echo.Discord.Api.Messages {
	public class WebhookMessage : Message {
		internal WebhookMessage(JsonObjectReader r) : base(r) {
			WebhookId = r.ReadString("webhook_id");
			Type = MessageType.Webhook;
			var json = r.ReadObject<UserJson>("author");
			WebhookUsername = json.username;
			WebhookAvatar = json.avatar;
		}
		public string WebhookId {
			get;
			private set;
		}
		public string WebhookUsername {
			get;
			private set;
		}
		public string WebhookAvatar {
			get;
			private set;
		}
		internal override void UpdateInstance(JsonObjectReader r) {
			base.UpdateInstance(r);
			WebhookId = r.ReadString("webhook_id");
			Type = MessageType.Webhook;
			var json = r.ReadObject<UserJson>("author");
			WebhookUsername = json.username;
			WebhookAvatar = json.avatar;
		}
		public override string ToString() {
			return "[" + Channel.ToString() + "] => " + WebhookUsername + " (" + WebhookId + "): " + Content;
		}
	}
}
