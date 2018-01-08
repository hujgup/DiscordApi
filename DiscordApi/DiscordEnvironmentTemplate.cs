using System;
using System.Threading;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Echo.Discord.Api {
	public partial class DiscordEnvironment {
		public static Thread CreateSubthread(ParameterizedThreadStart start) {
			DiscordEnvironment env = Current;
			return new Thread(arg => {
				DiscordDebug.WriteLine("Starting environment thread...", env?.AppName);
				try {
					if (env != null) {
						env.Run(() => {
							start(arg);
						});
					} else {
						start(arg);
					}
				} catch (Exception e) {
					DiscordDebug.WriteLine(e.ToString());
				} finally {
					DiscordDebug.WriteLine("Environment thread done.", env?.AppName);
				}
			});
		}
		public static Thread CreateSubthread(ThreadStart start) {
			DiscordEnvironment env = Current;
			return new Thread(() => {
				DiscordDebug.WriteLine("Starting environment thread...", env?.AppName);
				try {
					if (env != null) {
						env.Run(() => {
							start();
						});
					} else {
						start();
					}
				} catch (Exception e) {
					DiscordDebug.WriteLine(e.ToString());
				} finally {
					DiscordDebug.WriteLine("Environment thread done.", env?.AppName);
				}
			});
		}
		public static Thread CreateSubthread(ParameterizedThreadStart start, int maxStackSize) {
			DiscordEnvironment env = Current;
			return new Thread(arg => {
				DiscordDebug.WriteLine("Starting environment thread...", env?.AppName);
				try {
					if (env != null) {
						env.Run(() => {
							start(arg);
						});
					} else {
						start(arg);
					}
				} catch (Exception e) {
					DiscordDebug.WriteLine(e.ToString());
				} finally {
					DiscordDebug.WriteLine("Environment thread done.", env?.AppName);
				}
			}, maxStackSize);
		}
		public static Thread CreateSubthread(ThreadStart start, int maxStackSize) {
			DiscordEnvironment env = Current;
			return new Thread(() => {
				DiscordDebug.WriteLine("Starting environment thread...", env?.AppName);
				try {
					if (env != null) {
						env.Run(() => {
							start();
						});
					} else {
						start();
					}
				} catch (Exception e) {
					DiscordDebug.WriteLine(e.ToString());
				} finally {
					DiscordDebug.WriteLine("Environment thread done.", env?.AppName);
				}
			}, maxStackSize);
		}
	}
}