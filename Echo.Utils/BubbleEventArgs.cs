using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Echo.Utils.Async;

namespace Echo.Utils {
	public class BubbleEventArgs : EventArgs {
		public BubbleEventArgs() {
			IsStopped = false;
		}
		internal bool IsStopped {
			get;
			set;
		}
		internal bool StopNextPhase {
			get;
			private set;
		}
		public bool IsBubbling {
			get;
			internal set;
		}
		/// <summary>
		/// Stops events further down the event chain from firing.
		/// </summary>
		/// <param name="stopNextPhase">If <see cref="IsBubbling"/> is false, indicates whether to also skip the bubbling phase. If <see cref="IsBubbling"/> is true, this has no effect.</param>
		public virtual void StopPropagation(bool stopNextPhase) {
			IsStopped = true;
			StopNextPhase = stopNextPhase;
		}
		/// <summary>
		/// Stops events further down the event chain from firing.
		/// </summary>
		public virtual void StopPropagation() {
			IsStopped = true;
			StopNextPhase = false;
		}
	}
}
