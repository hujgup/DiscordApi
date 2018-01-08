using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Echo.Discord.Api.Messages {
	public class PinnedMessageData {
		private PinnedMessageData([CanBeNull] IReadOnlyDictionary<ulong, Message> prev, IReadOnlyDictionary<ulong, Message> curr, IReadOnlyDictionary<ulong, Message> add, IReadOnlyDictionary<ulong, Message> rem) {
			CurrentlyPinned = curr;
			PreviouslyPinned = prev;
			Added = add;
			Removed = rem;
		}
		internal PinnedMessageData([CanBeNull] IReadOnlyDictionary<ulong, Message> prev, IReadOnlyDictionary<ulong ,Message> curr) {
			CurrentlyPinned = curr;
			PreviouslyPinned = prev;
			var add = new Dictionary<ulong, Message>();
			var rem = new Dictionary<ulong, Message>();
			if (prev != null) {
				List<ulong> newIds = curr.Keys.ToList();
				foreach (KeyValuePair<ulong, Message> kvp in prev) {
					if (curr.ContainsKey(kvp.Key)) {
						newIds.Remove(kvp.Key);
					} else {
						add.Add(kvp.Key, kvp.Value);
					}
				}
				foreach (ulong id in newIds) {
					rem.Add(id, curr[id]);
				}
			}
			Added = add;
			Removed = rem;
		}
		public IReadOnlyDictionary<ulong, Message> CurrentlyPinned {
			get;
		}
		[CanBeNull]
		public IReadOnlyDictionary<ulong, Message> PreviouslyPinned {
			get;
		}
		public bool HasChangeData {
			get => PreviouslyPinned != null;
		}
		public IReadOnlyDictionary<ulong, Message> Added {
			get;
		}
		public IReadOnlyDictionary<ulong, Message> Removed {
			get;
		}
		public bool Changed {
			get => HasChangeData && (Added.Count > 0 || Removed.Count > 0);
		}
		private static Dictionary<ulong, Message> Copy(IReadOnlyDictionary<ulong, Message> d) {
			return d.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
		}
		internal List<PinnedMessageData> Split() {
			Dictionary<ulong, Message> curr = Copy(CurrentlyPinned);
			foreach (KeyValuePair<ulong, Message> kvp in Added) {
				curr.Remove(kvp.Key);
			}
			foreach (KeyValuePair<ulong, Message> kvp in Removed) {
				curr.Add(kvp.Key, kvp.Value);
			}
			var res = new List<PinnedMessageData>();
			var empty = new Dictionary<ulong, Message>();
			foreach (KeyValuePair<ulong, Message> kvp in Added) {
				curr.Add(kvp.Key, kvp.Value);
				res.Add(new PinnedMessageData(PreviouslyPinned, curr, new Dictionary<ulong, Message>() {
					{ kvp.Key, kvp.Value }
				}, empty));
			}
			foreach (KeyValuePair<ulong, Message> kvp in Removed) {
				curr.Remove(kvp.Key);
				res.Add(new PinnedMessageData(PreviouslyPinned, curr, new Dictionary<ulong, Message>() {
					{ kvp.Key, kvp.Value }
				}, empty));
			}
			return res;
		}
	}
}
