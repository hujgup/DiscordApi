using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Echo.Discord.Api.Http {
	public abstract class Token : IEquatable<Token>, IEquatable<string> {
		protected Token(string data) {
			Data = data;
		}
		public string Data {
			get;
		}
		public override string ToString() {
			return Data;
		}
		public bool Equals([CanBeNull] Token other) {
			return !(other is null) && Data.Equals(other.Data);
		}
		public bool Equals([CanBeNull] string other) {
			return !(other is null) && Data.Equals(other);
		}
		public override bool Equals([CanBeNull] object obj) {
			return obj is Token token ? Equals(token) : (obj is string && Equals((string)obj));
		}
		public override int GetHashCode() {
			return Data.GetHashCode();
		}
		public static bool operator ==([CanBeNull] Token a, [CanBeNull] Token b) {
			return a is null ? b is null : a.Equals(b);
		}
		public static bool operator !=([CanBeNull] Token a, [CanBeNull] Token b) {
			return !(a == b);
		}
		public static bool operator ==([CanBeNull] Token a, [CanBeNull] string b) {
			return a is null ? b is null : a.Equals(b);
		}
		public static bool operator !=([CanBeNull] Token a, [CanBeNull] string b) {
			return !(a == b);
		}
		public static bool operator ==([CanBeNull] string a, [CanBeNull] Token b) {
			return a is null ? b is null : (!(b is null) && b.Equals(a));
		}
		public static bool operator !=([CanBeNull] string a, [CanBeNull] Token b) {
			return !(a == b);
		}
	}
}
