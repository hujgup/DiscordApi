using System;
using System.IO;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Echo.Utils {
	public partial class MultiWriter {
		public override void Close() {
			All(x => x.Close());
		}
		public override void Flush() {
			All(x => x.Flush());
		}
		public override async Task FlushAsync() {
			var w = new Task[Writers.Count];
			int i = 0;
			All(x => w[i++] = x.FlushAsync());
			await Task.WhenAll(w);
		}
		public override void Write(bool value) {
			All(x => x.Write(value));
		}
		public override void WriteLine(bool value) {
			All(x => x.WriteLine(value));
		}
		public override void Write(char value) {
			All(x => x.Write(value));
		}
		public override async Task WriteAsync(char value) {
			var w = new Task[Writers.Count];
			int i = 0;
			All(x => w[i++] = x.WriteAsync(value));
			await Task.WhenAll(w);
		}
		public override void WriteLine(char value) {
			All(x => x.WriteLine(value));
		}
		public override async Task WriteLineAsync(char value) {
			var w = new Task[Writers.Count];
			int i = 0;
			All(x => w[i++] = x.WriteLineAsync(value));
			await Task.WhenAll(w);
		}
		public override void Write(char[] buffer, int index, int count) {
			All(x => x.Write(buffer, index, count));
		}
		public override async Task WriteAsync(char[] buffer, int index, int count) {
			var w = new Task[Writers.Count];
			int i = 0;
			All(x => w[i++] = x.WriteAsync(buffer, index, count));
			await Task.WhenAll(w);
		}
		public override void WriteLine(char[] buffer, int index, int count) {
			All(x => x.WriteLine(buffer, index, count));
		}
		public override async Task WriteLineAsync(char[] buffer, int index, int count) {
			var w = new Task[Writers.Count];
			int i = 0;
			All(x => w[i++] = x.WriteLineAsync(buffer, index, count));
			await Task.WhenAll(w);
		}
		public override void Write(decimal value) {
			All(x => x.Write(value));
		}
		public override void WriteLine(decimal value) {
			All(x => x.WriteLine(value));
		}
		public override void Write(double value) {
			All(x => x.Write(value));
		}
		public override void WriteLine(double value) {
			All(x => x.WriteLine(value));
		}
		public override void Write(float value) {
			All(x => x.Write(value));
		}
		public override void WriteLine(float value) {
			All(x => x.WriteLine(value));
		}
		public override void Write(int value) {
			All(x => x.Write(value));
		}
		public override void WriteLine(int value) {
			All(x => x.WriteLine(value));
		}
		public override void Write(long value) {
			All(x => x.Write(value));
		}
		public override void WriteLine(long value) {
			All(x => x.WriteLine(value));
		}
		public override void Write([CanBeNull] object value) {
			All(x => x.Write(value));
		}
		public override void WriteLine([CanBeNull] object value) {
			All(x => x.WriteLine(value));
		}
		public override void Write([CanBeNull] string value) {
			All(x => x.Write(value));
		}
		public override async Task WriteAsync([CanBeNull] string value) {
			var w = new Task[Writers.Count];
			int i = 0;
			All(x => w[i++] = x.WriteAsync(value));
			await Task.WhenAll(w);
		}
		public override void WriteLine([CanBeNull] string value) {
			All(x => x.WriteLine(value));
		}
		public override async Task WriteLineAsync([CanBeNull] string value) {
			var w = new Task[Writers.Count];
			int i = 0;
			All(x => w[i++] = x.WriteLineAsync(value));
			await Task.WhenAll(w);
		}
		public override void Write(string format, [CanBeNull] object arg0) {
			All(x => x.Write(format, arg0));
		}
		public override void WriteLine(string format, [CanBeNull] object arg0) {
			All(x => x.WriteLine(format, arg0));
		}
		public override void Write(string format, [CanBeNull] object arg0, [CanBeNull] object arg1) {
			All(x => x.Write(format, arg0, arg1));
		}
		public override void WriteLine(string format, [CanBeNull] object arg0, [CanBeNull] object arg1) {
			All(x => x.WriteLine(format, arg0, arg1));
		}
		public override void Write(string format, [CanBeNull] object arg0, [CanBeNull] object arg1, [CanBeNull] object arg2) {
			All(x => x.Write(format, arg0, arg1, arg2));
		}
		public override void WriteLine(string format, [CanBeNull] object arg0, [CanBeNull] object arg1, [CanBeNull] object arg2) {
			All(x => x.WriteLine(format, arg0, arg1, arg2));
		}
		public override void Write(uint value) {
			All(x => x.Write(value));
		}
		public override void WriteLine(uint value) {
			All(x => x.WriteLine(value));
		}
		public override void Write(ulong value) {
			All(x => x.Write(value));
		}
		public override void WriteLine(ulong value) {
			All(x => x.WriteLine(value));
		}
	}
}