using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Diagnostics;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace Echo.Discord.Api {
	internal static class DiscordDebug {
		private const string _wlPrefixPre = "[" + Info.LibraryName;
		private static string TypeName(object obj) {
			return obj.GetType().Name + ": ";
		}
		public static string JsonString(object obj) {
			return TypeName(obj) + JsonConvert.SerializeObject(obj, Formatting.Indented);
		}
		public static void WriteLine(string line, [CanBeNull] string appName) {
#if DEBUG
			string prefix = _wlPrefixPre;
			if (appName != null) {
				prefix += "/" + appName;
			}
			Debug.WriteLine(prefix + "] :: " + line);
#endif
		}
		public static void WriteLine(string line) {
#if DEBUG
			WriteLine(line, DiscordEnvironment.CurrentAppName);
#endif
		}
		public static void WriteLine([CanBeNull] object obj, [CanBeNull] string appName) {
#if DEBUG
			WriteLine(obj is null ? "null" : obj.ToString(), appName);
#endif
		}
		public static void WriteLine([CanBeNull] object obj) {
#if DEBUG
			WriteLine(obj, DiscordEnvironment.CurrentAppName);
#endif
		}
	}
}
