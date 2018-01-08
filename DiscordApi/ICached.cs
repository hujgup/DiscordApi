using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Echo.Discord.Api {
	public interface ICached : IIdentifiable {
		void Uncache();
	}
}
