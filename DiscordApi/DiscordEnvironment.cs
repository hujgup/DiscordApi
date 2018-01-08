using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Echo.Discord.Api.Channels;
using Echo.Discord.Api.Http;
using Echo.Discord.Api.Servers;
using Echo.Utils.Async;
using JetBrains.Annotations;
using Timer = System.Timers.Timer;

namespace Echo.Discord.Api {
	public partial class DiscordEnvironment {
		private static readonly AsyncLocal<DiscordEnvironment> _env = new AsyncLocal<DiscordEnvironment>();
		private DiscordEnvironment(Client client, string appName) {
			Client = client;
			AppName = appName;
		}
		public static void Create(Token token, string appName, Func<Client, Task> runner) {
			DiscordDebug.WriteLine("Creating client...", appName);
			var env = new DiscordEnvironment(new Client(token), appName);
			var t = new Thread(() => {
				AsyncContext.Switch(async () => {
					await env.RunAsync(async () => {
						DiscordDebug.WriteLine("Client created.", appName);
						await runner(env.Client);
					});
				});
			});
			t.Start();
		}
		public static void Create(Token token, string appName, Action<Client> runner) {
			Create(token, appName, client => {
				runner(client);
				return Task.CompletedTask;
			});
		}
		private static void CreateGateway(Token token, string appName, GatewayOptions options, int shardId, int shardCount, Func<GatewayClient, Task> runner) {
			DiscordDebug.WriteLine("Creating gateway client...", appName);
			var env = new DiscordEnvironment(new GatewayClient(token, shardId, shardCount), appName);
			var t = new Thread(() => {
				AsyncContext.Switch(async () => {
					await env.RunAsync(async () => {
						env.GatewayClient.FirePinEvents = options.HasFlag(GatewayOptions.FirePinEvents);
						Server[] servers = null;
						if (!options.HasFlag(GatewayOptions.ManualConnect)) {
							DiscordDebug.WriteLine("Connecting...", appName);
							servers = await env.GatewayClient.ConnectAsync();
							DiscordDebug.WriteLine("Connected.", appName);
						}
						DiscordDebug.WriteLine("Gateway client created.", appName);
						if (env.GatewayClient.FirePinEvents && servers != null) {
							DiscordDebug.WriteLine("Getting initial channel pins...");
							var tasks = new List<Task>();
							// ReSharper disable LoopCanBeConvertedToQuery (confusing)
							foreach (Server s in servers) {
								foreach (IChannel channel in s.Channels.Values.Where(x => x is ServerTextChannel)) {
									var c = (ServerTextChannel)channel;
									tasks.Add(c.GetPinnedMessagesAsync(env.Client));
								}
							}
							// ReSharper restore LoopCanBeConvertedToQuery
							await Task.WhenAll(tasks);
							DiscordDebug.WriteLine("Initial channel pins retreived.");
						}
						await runner((GatewayClient)env.Client);
					});
				});
			});
			t.Start();
		}
		private static void CreateGateway(Token token, string appName, GatewayOptions options, int shardId, int shardCount, Action<GatewayClient> runner) {
			CreateGateway(token, appName, options, shardId, shardCount, client => {
				runner(client);
				return Task.CompletedTask;
			});
		}
		public static void CreateGateway(Token token, string appName, GatewayOptions options, Func<GatewayClient, Task> runner) {
			CreateGateway(token, appName, options, 0, 1, runner);
		}
		public static void CreateGateway(Token token, string appName, Func<GatewayClient, Task> runner) {
			CreateGateway(token, appName, GatewayOptions.Default, runner);
		}
		public static void CreateGateway(Token token, string appName, GatewayOptions options, Action<GatewayClient> runner) {
			CreateGateway(token, appName, options, 0, 1, runner);
		}
		public static void CreateGateway(Token token, string appName, Action<GatewayClient> runner) {
			CreateGateway(token, appName, GatewayOptions.Default, runner);
		}
		public static void CreateShardedGateway(Token token, string appName, GatewayOptions options, params Func<GatewayClient, Task>[] runners) {
			for (int i = 0; i < runners.Length; i++) {
				CreateGateway(token, appName, options, i, runners.Length, runners[i]);
			}
		}
		public static void CreateShardedGateway(Token token, string appName, params Func<GatewayClient, Task>[] runners) {
			CreateShardedGateway(token, appName, GatewayOptions.Default, runners);
		}
		public static void CreateShardedGateway(Token token, string appName, GatewayOptions options, params Action<GatewayClient>[] runners) {
			for (int i = 0; i < runners.Length; i++) {
				CreateGateway(token, appName, options, i, runners.Length, runners[i]);
			}
		}
		public static void CreateShardedGateway(Token token, string appName, params Action<GatewayClient>[] runners) {
			CreateShardedGateway(token, appName, GatewayOptions.Default, runners);
		}
		[CanBeNull]
		public static DiscordEnvironment Current {
			get => _env.Value;
		}
		[CanBeNull]
		public static Client CurrentClient {
			get => Current?.Client;
		}
		[CanBeNull]
		public static GatewayClient CurrentGatewayClient {
			get => (GatewayClient)CurrentClient;
		}
		[CanBeNull]
		public static string CurrentAppName {
			get => Current?.AppName;
		}
		public Client Client {
			get;
		}
		public GatewayClient GatewayClient {
			get => (GatewayClient)Client;
		}
		public string AppName {
			get;
		}
		private static bool ContainsThread(IEnumerable<Thread> threads, Thread thread) {
			return threads.Any(t => t.ManagedThreadId == thread.ManagedThreadId);
		}
		public async Task RunAsync(Func<Task> runner) {
			bool isNull = Current == null;
			if (isNull || Current == this) {
				if (isNull) {
					_env.Value = this;
				}
				await runner();
			} else {
				throw new EnvironmentReassignmentException("A thread can only have one environment.");
			}
		}
		public void Run(Action runner) {
			#pragma warning disable 4014
			#pragma warning disable 1998
			RunAsync(async () => {
				runner();
			});
			#pragma warning restore 1998
			#pragma warning restore 4014
		}
		public override string ToString() {
			return "Environment for " + Client.ToString();
		}
	}
}
