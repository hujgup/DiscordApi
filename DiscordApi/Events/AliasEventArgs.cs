using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Echo.Utils;

namespace Echo.Discord.Api.Events {
	public class AliasEventArgs<T> : BubbleEventArgs where T : BubbleEventArgs {
		protected readonly T _args;
		internal AliasEventArgs(T args) {
			_args = args;
		}
		/// <summary>
		/// Stops events further down the event chain from firing.
		/// </summary>
		/// <param name="stopNextPhase">If <see cref="BubbleEventArgs.IsBubbling"/> is false, indicates whether to also skip the bubbling phase. If <see cref="BubbleEventArgs.IsBubbling"/> is true, this has no effect.</param>
		public sealed override void StopPropagation(bool stopNextPhase) {
			_args.StopPropagation(stopNextPhase);
			base.StopPropagation(stopNextPhase);
		}
		/// <summary>
		/// Stops events further down the event chain from firing.
		/// </summary>
		public sealed override void StopPropagation() {
			_args.StopPropagation();
			base.StopPropagation();
		}
		public T Convert() {
			return _args;
		}
	}
}
