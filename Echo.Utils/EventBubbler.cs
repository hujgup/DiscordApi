using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Echo.Utils.Async;

namespace Echo.Utils {
	public delegate void BubbleEventInvoker<in T>(object sender, T eventArgs) where T : BubbleEventArgs;

	public delegate void BubbleSetUp<in T>(T eventArgs) where T : BubbleEventArgs;

	public delegate void BubbleTearDown<in T>(T eventArgs, bool stopped) where T : BubbleEventArgs;

	public class EventBubbler<T> where T : BubbleEventArgs {
		private static readonly BubbleSetUp<T> _defaultSetUp = ea => {};
		private readonly BubbleSetUp<T> _beforeRun;
		private readonly BubbleTearDown<T> _afterRun;
		private readonly List<BubbleEventInvoker<T>> _events;
		public EventBubbler(BubbleSetUp<T> beforeRun, BubbleTearDown<T> afterRun, params BubbleEventInvoker<T>[] events) {
			_beforeRun = beforeRun;
			_afterRun = afterRun;
			_events = events.ToList();
		}
		public EventBubbler(BubbleSetUp<T> beforeRun, params BubbleEventInvoker<T>[] events) : this(beforeRun, (ea, s) => {}, events) {
		}
		public EventBubbler(BubbleTearDown<T> afterRun, params BubbleEventInvoker<T>[] events) : this(_defaultSetUp, afterRun, events) {
		}
		/// <summary>
		/// Creates a new EventBubbler.
		/// </summary>
		/// <param name="events">The event bubbling sequence, in decresing specificity order (most specific event goes first).</param>
		public EventBubbler(params BubbleEventInvoker<T>[] events) : this(_defaultSetUp, events) {
		}
		/// <summary>
		/// <para>Executes <paramref name="callback"/> iff event is in the capturing phase (i.e. IsBubbling is false).</para>
		/// <para>Use this for generating event subscription functions (SomeEvent += <see cref="Capture(Action{object, T})"/>(() => { /* your code */ });).</para>
		/// </summary>
		/// <param name="callback">The callback function.</param>
		public static EventHandler<T> Capture(Action<object, T> callback) {
			return (sender, e) => {
				if (!e.IsBubbling) {
					callback(sender, e);
				}
			};
		}
		/// <summary>
		/// <para>Executes <paramref name="callback"/> iff event is in the bubbling phase (i.e. IsBubbling is true).</para>
		/// <para>Use this for generating event subscription functions (SomeEvent += <see cref="Bubble(Action{object, T})"/>(() => { /* your code */ });).</para>
		/// </summary>
		/// <param name="callback">The callback function.</param>
		public static EventHandler<T> Bubble(Action<object, T> callback) {
			return (sender, e) => {
				if (!e.IsBubbling) {
					callback(sender, e);
				}
			};
		}
		public void Append(BubbleEventInvoker<T> evt) {
			_events.Add(evt);
		}
		public void Invoke(object sender, T eventArgs) {
			_beforeRun(eventArgs);
			// Capturing phase
			eventArgs.IsBubbling = false;
			for (int i = _events.Count - 1; i >= 0; i--) {
				_events[i](sender, eventArgs);
				if (eventArgs.IsStopped) {
					break;
				}
			}
			eventArgs.IsStopped = false;
			// Bubbling phase
			if (!eventArgs.StopNextPhase) {
				eventArgs.IsBubbling = true;
				foreach (BubbleEventInvoker<T> evt in _events) {
					evt(sender, eventArgs);
					if (eventArgs.IsStopped) {
						break;
					}
				}
			}
			_afterRun(eventArgs, eventArgs.IsStopped);
		}
	}
}
