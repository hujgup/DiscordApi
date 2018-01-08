using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Echo.Utils.Json {
	public class UnexpectedNullException : Exception {
		protected UnexpectedNullException(SerializationInfo info, StreamingContext context) : base(info, context) {
		}
		public UnexpectedNullException(string message, Exception innerException) : base(message, innerException) {
		}
		public UnexpectedNullException(string message) : base(message) {
		}
		public UnexpectedNullException() {
		}
	}
}
