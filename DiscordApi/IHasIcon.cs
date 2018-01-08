using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Echo.Discord.Api {
	public interface IHasIcon {
		Uri Icon {
			get;
		}
		string IconFormat {
			get;
		}
	}
}
