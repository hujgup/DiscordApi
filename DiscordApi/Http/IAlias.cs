using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// ReSharper disable InconsistentNaming (can't mark interface methods with async)

namespace Echo.Discord.Api.Http {
	public interface IAlias {
		void PullUpdate();
		Task PullUpdateAsync();
		void PullUpdate(Client client);
		Task PullUpdateAsync(Client client);
	}
}
