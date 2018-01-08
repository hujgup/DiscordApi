using System.Threading.Tasks;
using Echo.Utils;
using Echo.Discord.Api.Servers;
using Echo.Discord.Api.Http;
using JetBrains.Annotations;
// ReSharper disable InconsistentNaming (async method)

namespace Echo.Discord.Api.Messages {
	public interface IMentionable {
		Ternary IsMentionable {
			get;
		}
		string MentionContent {
			get;
		}
		string MentionFallbackName {
			get;
		}
		Task<bool> IsMentionableByAsync(ulong userId, ulong inServerId, Client client);
	}
}
