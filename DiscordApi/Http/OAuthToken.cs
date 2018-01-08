using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Echo.Discord.Api.Http {
	public class OAuthToken : Token {
		public OAuthToken(string token) : base("Bearer " + token) {
		}
	}
}
