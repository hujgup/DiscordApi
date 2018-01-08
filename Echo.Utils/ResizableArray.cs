using System;

namespace Echo.Utils {
	public class ResizableArray<T> {
		private T[] _arr;
		public ResizableArray(int capacity) {
			if (capacity <= 0) {
				throw new ArgumentOutOfRangeException(nameof(capacity), "Array capacity must be positive.");
			}
			_arr = new T[capacity];
			Count = 0;
		}
		public ResizableArray() : this(1024) {
		}
		public T this[int i] {
			get => _arr[i];
			set {
				if (i >= _arr.Length) {
					Resize(i);
				}
				_arr[i] = value;
				Count = Math.Max(i + 1, Count);
			}
		}
		public int Count {
			get;
			private set;
		}
		public ArraySegment<T> BackingArray {
			get => new ArraySegment<T>(_arr, 0, Count);
		}
		private void Resize(int target) {
			int scalar = (int)Math.Ceiling((double)target/_arr.Length);
			if (scalar > 1) {
				var newArr = new T[scalar*_arr.Length];
				Array.Copy(_arr, newArr, Count);
				_arr = newArr;
			}
		}
		private void Resize() {
			Resize(2*_arr.Length - 2);
		}
		public void Append(T value) {
			this[Count] = value;
		}
		public void Append(T[] src, int srcOffset, int count) {
			for (int i = 0; i < count; i++) {
				Append(src[i + srcOffset]);
			}
		}
		public void Append(T[] src, int count) {
			Append(src, 0, count);
		}
		public void Append(T[] src) {
			Append(src, src.Length);
		}
		public void Append(ArraySegment<T> src) {
			Validate.NotNull(src.Array, nameof(src.Array));
			Append(src.Array, src.Offset, src.Count);
		}
		public void CopyTo(int destOffset, T[] src, int srcOffset, int count) {
			for (int i = 0; i < count; i++) {
				this[i + destOffset] = src[i + srcOffset];
			}
		}
		public void CopyTo(int destOffset, T[] src, int count) {
			CopyTo(destOffset, src, 0, count);
		}
		public void CopyTo(int destOffset, T[] src) {
			CopyTo(destOffset, src, src.Length);
		}
		public void CopyTo(int destOffset, ArraySegment<T> src) {
			Validate.NotNull(src.Array, nameof(src.Array));
			CopyTo(destOffset, src.Array, src.Offset, src.Count);
		}
		public override string ToString() {
			return BackingArray.ToString();
		}
		public static implicit operator ArraySegment<T>(ResizableArray<T> arr) {
			return arr.BackingArray;
		}
	}
}
