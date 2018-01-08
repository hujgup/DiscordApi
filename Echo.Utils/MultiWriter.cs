using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Echo.Utils {
	public partial class MultiWriter : TextWriter {
		public MultiWriter(params TextWriter[] writers) {
			if (writers.Length == 0) {
				throw new ArgumentOutOfRangeException(nameof(writers), "Must have at least one writer.");
			}
			Writers = writers;
		}
		public IReadOnlyList<TextWriter> Writers {
			get;
		}
		public override Encoding Encoding {
			get => Writers[0].Encoding;
		}
		private void All(Action<TextWriter> x) {
			foreach (TextWriter w in Writers) {
				x(w);
			}
		}
	}
}
