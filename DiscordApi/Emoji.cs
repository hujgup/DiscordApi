using System;
using System.Threading.Tasks;
using Echo.Utils;
using Echo.Discord.Api.Json;
using Echo.Discord.Api.Http;
using Echo.Discord.Api.Messages;
using JetBrains.Annotations;

namespace Echo.Discord.Api {
	public abstract class Emoji : IEquatable<Emoji>, INamed, IMentionable {
		public abstract Ternary IsMentionable {
			get;
		}
		public abstract string MentionContent {
			get;
		}
		public abstract string MentionFallbackName {
			get;
		}
		public bool IsGlobal {
			get;
			protected set;
		}
		public string Name {
			get;
			protected set;
		}
		internal static void Populate(Emoji obj, EmojiJson json) {
			obj.Name = json.name;
		}
		public abstract Task<bool> IsMentionableByAsync(ulong userId, ulong inServerId, [CanBeNull] Client client);
		public abstract bool Equals([CanBeNull] Emoji ej);
		public abstract override int GetHashCode();
		public override string ToString() {
			return Name;
		}
		public override bool Equals([CanBeNull] object obj) {
			return obj is Emoji emoji && Equals(emoji);
		}
		public static bool operator ==([CanBeNull] Emoji a, [CanBeNull] Emoji b) {
			return a is null ? b is null : a.Equals(b);
		}
		public static bool operator !=([CanBeNull] Emoji a, [CanBeNull] Emoji b) {
			return !(a == b);
		}
	}
}
