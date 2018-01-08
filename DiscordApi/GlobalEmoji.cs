using System.Threading.Tasks;
using Echo.Utils;
using Echo.Discord.Api.Http;
using JetBrains.Annotations;

namespace Echo.Discord.Api {
	public class GlobalEmoji : Emoji {
		public GlobalEmoji(string name) {
			Name = name;
			IsGlobal = true;
		}
		public override Ternary IsMentionable {
			get => Ternary.Yes;
		}
		public override string MentionContent {
			get => ":" + Name + ":";
		}
		public override string MentionFallbackName {
			get => MentionContent;
		}
		public override Task<bool> IsMentionableByAsync(ulong userId, ulong inServerId, [CanBeNull] Client client) {
			return Task.Run(() => true);
		}
		public override bool Equals([CanBeNull] Emoji ej) {
			return ej is GlobalEmoji && Name == ej.Name;
		}
		public override int GetHashCode() {
			// ReSharper disable once NonReadonlyMemberInGetHashCode (protected set only occurs in constructor)
			return Name.GetHashCode();
		}
	}
}
