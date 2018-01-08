using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Echo.Discord.Api {
	public interface INicknamed {
		bool HasNickname {
			get;
		}
		[CanBeNull]
		string Nickname {
			get;
		}
	}
}
