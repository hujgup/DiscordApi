using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Echo.Discord.Api.Servers {
	public enum NsfwFilterLevel : int {
		Disabled = 0,
		MembersWithoutRoles = 1,
		AllMembers = 2
	}
}
