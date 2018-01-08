using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Echo.Discord.Api.Http {
	public class BotToken : Token {
		public BotToken(string token) : base("Bot " + token) {
		}
	}
}
