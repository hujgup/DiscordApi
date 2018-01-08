using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Echo.Discord.Api.Messages;

// ReSharper disable All

namespace Echo.Discord.Api.Json {
	#pragma warning disable 0649
	internal class MessageJson : JsonId {
		public ulong channel_id;
		public UserJson user;
		public string content;
		public string timestamp;
		public string edited_timestamp;
		public bool tts;
		public bool mention_everyone;
		public UserJson[] mentions;
		public RoleJson[] mention_roles;
		// TODO: On attachments, public AttachmentJson[] attachments;
		// TODO: On embeds, public EmbedJson[] embeds;
		public InternalMessageType type;
		// All below are nullable
		public ReactionJson[] reactions;
		public ulong? nonce;
		public bool? pinned;
		public string webhook_id;
	}
}
