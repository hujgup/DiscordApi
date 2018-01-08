using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Echo.Utils.Json {
	public class NotNullException : Exception {
		protected NotNullException(SerializationInfo info, StreamingContext context) : base(info, context) {
		}
		public NotNullException(string message, Exception innerException) : base(message, innerException) {
		}
		public NotNullException(string message) : base(message) {
		}
		public NotNullException() {
		}
	}
}
