using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ReSharper disable All

namespace Echo.Discord.Api.Json {
	#pragma warning disable 0649
	internal class ReactionJson {
		public int count;
		public bool me;
		public EmojiJson emoji;
	}
}
