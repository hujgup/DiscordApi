using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Echo.Utils;
using Echo.Discord.Api.Http;
using Echo.Discord.Api.Channels;
using Echo.Discord.Api.Servers;
using Echo.Discord.Api.Events;

namespace Echo.Discord.Api.Json {
	public static class Test {
		private static string B(ChannelEventArgs e) {
			return e.IsBubbling ? "Bubble" : "Capture";
		}
		private static EventHandler<ChannelEventArgs> L(string name) {
			return (sender, e) => {
				Console.WriteLine(name + " " + B(e));
			};
		}
		public static EventHandler<ChannelEventArgs<T>> L<T>(string name, bool stop) where T : IChannel {
			return (sender, e) => {
				L(name)(sender, e.Convert());
				if (stop) {
					e.StopPropagation(true);
				}
			};
		}
		public static EventHandler<ChannelEventArgs<T>> L<T>(string name) where T : IChannel {
			return (sender, e) => {
				L(name)(sender, e.Convert());
			};
		}
		public static string Prettify(string json) {
			return JsonConvert.SerializeObject(JsonConvert.DeserializeObject(json), Formatting.Indented);
		}
		public static void Main(string[] args) {
			TextWriter fileOut = TextWriter.Synchronized(new StreamWriter(new FileStream("output.log", FileMode.Create)));
			Console.SetOut(new MultiWriter(Console.Out, fileOut));
			//Debug.Listeners.Add(new ConsoleTraceListener());
			DiscordEnvironment.CreateGateway(new BotToken("Mzg5NzIwNTg1MjI1MjQwNTc4.DTBrBQ.F_hS16KEEwjKothN6Mo8NoF2p-A"), "DebugBot", client => {
				Console.WriteLine("Setting up events...");
				Server s = Server.GetCached(300146519619796993U);
				s.TextChannelCreate += L<ServerTextChannel>("s.TextChannelCreate");
				s.NonCatChannelCreate += L<INonCatServerChannel>("s.NonCatChannelCreate");
				s.ChannelCreate += L<IServerChannel>("s.ChannelCreate", true);
				ServerTextChannel.Create += L<ServerTextChannel>("ServerTextChannel.Create");
				TextChannel.Create += L<TextChannel>("TextChannel.Create");
				ChannelUtils.NonCatServerChannelCreate += L<INonCatServerChannel>("ChannelUtils.NonCatServerChannelCreate");
				ChannelUtils.ServerChannelCreate += L<IServerChannel>("ChannelUtils.ServerChannelCreate");
				ChannelUtils.ChannelCreate += L("ChannelUtils.ChannelCreate");
				ChannelUtils.Create += L("ChannelUtils.Create");
				Console.WriteLine("Events created.");
				Console.ReadLine();
			});
			Console.ReadLine();

			//try {
			//ulong channel = 345312662646947851; // Server text
			//ulong channel = 0; // DM single
			//ulong channel = 311527722918608896; // Server voice
			//ulong channel = 0; // DM group
			//ulong channel = 375504585730686976; // Server category
			//DiscordDebug.WriteLine(Client.GetChannel(channel, token));
			//DiscordDebug.WriteLine(DiscordDebug.ToString(c.GetChannelAsync(channel).Await()));

			//ulong server = 311527722474143744;
			//DiscordDebug.WriteLine(DiscordDebug.ToString(c.GetServer(server)));
			//ITraceWriter w = new MemoryTraceWriter();
			//try {
			//DiscordDebug.WriteLine(c.GetServerJson(server, w).Await());
			//DiscordDebug.WriteLine(Client.GetServer(server, token));
			//} catch (Exception) {
			//DiscordDebug.WriteLine(w);
			//}

			//ulong user = 205899965984276480;
			//DiscordDebug.WriteLine(DiscordDebug.ToString(ServerUser.Get(user, c)));

			//ulong emoji = 324119851801247745;
			//DiscordDebug.WriteLine(DiscordDebug.ToString(Emoji.Get(server, emoji, c)));
			//DiscordDebug.WriteLine(DiscordDebug.ToString(c.GetServerEmojiAsync(server, emoji).Await()));
			//} catch (Exception e) {
			//DiscordDebug.WriteLine(e);
			//}
		}
	}
}
