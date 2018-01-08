using System;
using JetBrains.Annotations;

namespace Echo.Utils {
	public partial class SharedRef<T> {
		// In the below methods, ingore refactor suggestions for ReferenceEquals - default(T) is not a valid substitute for null in these cases
		public override string ToString() {
			return ReferenceEquals(Value, null) ? "null" : Value.ToString();
		}
		public bool Equals([CanBeNull] IConstRef<T> r) {
			return !(r is null) && Value.Equals(r.Value);
		}
		public bool Equals([CanBeNull] T v) {
			return ReferenceEquals(v, null) ? ReferenceEquals(Value, null) : Value.Equals(v);
		}
		public override bool Equals([CanBeNull] object obj) {
			bool res;
			// ReSharper disable once ConvertIfStatementToSwitchStatement
			if (obj is IConstRef<T> obj2) {
				res = Equals(obj2);
			} else if (obj is T obj3) {
				res = Equals(obj3);
			} else {
				res = false;
			}
			return res;
		}
		public override int GetHashCode() {
			// ReSharper disable NonReadonlyMemberInGetHashCode
			return ReferenceEquals(Value, null) ? 0 : Value.GetHashCode();
			// ReSharper restore NonReadonlyMemberInGetHashCode
		}
		public static bool operator ==([CanBeNull] SharedRef<T> a, [CanBeNull] IConstRef<T> b) {
			return a is null ? b is null : a.Equals(b);
		}
		public static bool operator !=([CanBeNull] SharedRef<T> a, [CanBeNull] IConstRef<T> b) {
			return !(a == b);
		}
		public static bool operator ==([CanBeNull] SharedRef<T> a, [CanBeNull] T b) {
			return a is null ? ReferenceEquals(b, null) : a.Equals(b);
		}
		public static bool operator !=([CanBeNull] SharedRef<T> a, [CanBeNull] T b) {
			return !(a == b);
		}
		public static bool operator ==([CanBeNull] T a, [CanBeNull] SharedRef<T> b) {
			return ReferenceEquals(a, null) ? b is null : (b != null && b.Equals(a));
		}
		public static bool operator !=([CanBeNull] T a, [CanBeNull] SharedRef<T> b) {
			return !(a == b);
		}
	}
	public partial class AliasRef<T> {
		// In the below methods, ingore refactor suggestions for ReferenceEquals - default(T) is not a valid substitute for null in these cases
		public override string ToString() {
			return ReferenceEquals(Value, null) ? "null" : Value.ToString();
		}
		public bool Equals([CanBeNull] IConstRef<T> r) {
			return !(r is null) && Value.Equals(r.Value);
		}
		public bool Equals([CanBeNull] T v) {
			return ReferenceEquals(v, null) ? ReferenceEquals(Value, null) : Value.Equals(v);
		}
		public override bool Equals([CanBeNull] object obj) {
			bool res;
			// ReSharper disable once ConvertIfStatementToSwitchStatement
			if (obj is IConstRef<T> obj2) {
				res = Equals(obj2);
			} else if (obj is T obj3) {
				res = Equals(obj3);
			} else {
				res = false;
			}
			return res;
		}
		public override int GetHashCode() {
			// ReSharper disable NonReadonlyMemberInGetHashCode
			return ReferenceEquals(Value, null) ? 0 : Value.GetHashCode();
			// ReSharper restore NonReadonlyMemberInGetHashCode
		}
		public static bool operator ==([CanBeNull] AliasRef<T> a, [CanBeNull] IConstRef<T> b) {
			return a is null ? b is null : a.Equals(b);
		}
		public static bool operator !=([CanBeNull] AliasRef<T> a, [CanBeNull] IConstRef<T> b) {
			return !(a == b);
		}
		public static bool operator ==([CanBeNull] AliasRef<T> a, [CanBeNull] T b) {
			return a is null ? ReferenceEquals(b, null) : a.Equals(b);
		}
		public static bool operator !=([CanBeNull] AliasRef<T> a, [CanBeNull] T b) {
			return !(a == b);
		}
		public static bool operator ==([CanBeNull] T a, [CanBeNull] AliasRef<T> b) {
			return ReferenceEquals(a, null) ? b is null : (b != null && b.Equals(a));
		}
		public static bool operator !=([CanBeNull] T a, [CanBeNull] AliasRef<T> b) {
			return !(a == b);
		}
	}
}