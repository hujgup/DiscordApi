using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Echo.Utils {
	public class CannotFinalizeException : Exception {
		protected CannotFinalizeException(SerializationInfo info, StreamingContext context) : base(info, context) {
		}
		public CannotFinalizeException(string message, Exception innerException) : base(message, innerException) {
		}
		public CannotFinalizeException(string message) : base(message) {
		}
		public CannotFinalizeException() {
		}
	}
}
