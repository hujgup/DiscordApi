using System;
using System.Net;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using Newtonsoft.Json;
using Echo.Utils.Async;
using Echo.Discord.Api.Channels;
using Echo.Discord.Api.Json;
using Echo.Discord.Api.Servers;
using JetBrains.Annotations;
using Timer = System.Timers.Timer;
using ElapsedEventArgs = System.Timers.ElapsedEventArgs;
using Newtonsoft.Json.Linq;

namespace Echo.Discord.Api.Http {
	public class Client {
		private class BucketMap {
			private class BucketEqualityComparer : IEqualityComparer<ulong[]> {
				public int GetHashCode(ulong[] a) {
					return unchecked(a.Aggregate(17, (hash, val) => hash*23 + val.GetHashCode()));
				}
				public bool Equals(ulong[] a, ulong[] b) {
					bool res = a.Length == b.Length;
					if (res) {
						for (int i = 0; res && i < a.Length; i++) {
							res = a[i] == b[i];
						}
					}
					return res;
				}
			}

			private static readonly BucketEqualityComparer _eqCmp = new BucketEqualityComparer();
			private readonly SemaphoreSlim _sync;
			private readonly Dictionary<ulong[], SemaphoreSlim> _dic;
			public BucketMap() {
				_sync = new SemaphoreSlim(1);
				_dic = new Dictionary<ulong[], SemaphoreSlim>(_eqCmp);
			}
			public SemaphoreSlim this[ulong first, params ulong[] others] {
				get {
					_sync.Wait();
					ulong[] key = Concat(first, others);
					if (!_dic.TryGetValue(key, out SemaphoreSlim res)) {
						res = new SemaphoreSlim(1);
						_dic.Add(key, res);
					}
					_sync.Release();
					return res;
				}
			}
			private static ulong[] Concat(ulong first, ulong[] others) {
				var res = new ulong[others.Length + 1];
				res[0] = first;
				others.CopyTo(res, 1);
				return res;
			}
		}

		protected const string _baseUrl = "https://discordapp.com/api/v6/";
		internal const string _cdnUrl = "https://cdn.discordapp.com/";
		private static readonly HttpClient _client = new HttpClient();
		private readonly SemaphoreSlim _rlGlobal;
		private readonly SemaphoreSlim _sync;
		private readonly SemaphoreSlim _cuMutex;
		private User _clientUser;
		internal Client(Token token) {
			Token = token.Data;
			_rlGlobal = new SemaphoreSlim(1);
			_sync = new SemaphoreSlim(1);
			_cuMutex = new SemaphoreSlim(1);
		}
		public string Token {
			get;
		}
		private Task<HttpResponseMessage> SendAsync(HttpMethod method, string url, [CanBeNull] IEnumerable<KeyValuePair<string, string>> args) {
			bool setContent = method.Method != HttpMethod.Get.Method;
			if (!setContent && args != null) {
				url += "?";
				url += args.Aggregate("", (current, kvp) => current + (kvp.Key + "=" + kvp.Value + "&"));
				url = url.Substring(0, url.Length - 1);
			}
			var msg = new HttpRequestMessage(method, url);
			msg.Headers.Add("Authorization", Token);
			msg.Headers.Add("User-Agent", "DiscordBot");
			if (setContent && args != null) {
				msg.Content = new FormUrlEncodedContent(args);
			}
			return _client.SendAsync(msg);
		}
		private async Task<HttpResponseMessage> SendAsync(HttpMethod method, string url) {
			return await SendAsync(method, url, null);
		}
		protected async Task<T> GetResponseAsync<T>(SemaphoreSlim bucket, HttpMethod method, string url, [CanBeNull] IEnumerable<KeyValuePair<string, string>> args) {
			//DiscordDebug.WriteLine("HTTP STATUS ==================== " + (int)msg.StatusCode);
			T res = default;
			bool goAgain;
			do {
				goAgain = false;
				await _sync.WaitAsync();
				await _rlGlobal.WaitAsync();
				_rlGlobal.Release();
				await bucket.WaitAsync();
				bucket.Release();
				_sync.Release();
				HttpResponseMessage msg = await SendAsync(method, url, args);
				switch ((int)msg.StatusCode) {
					case 400:
					case 405:
					case 404:
						throw new IncorrectApiStructureException("HTTP status " + (int)msg.StatusCode + " should not occur with a correctly-designed API.");
					case 401:
					case 403:
						throw new PermissionDeniedException("Client with token \"" + Token + "\" cannot perform the requested action because the user corresponding to that token lacks the required Discord permissions.");
					case 429:
						await _sync.WaitAsync();
						var data = JsonConvert.DeserializeObject<RateLimitJson>(await msg.Content.ReadAsStringAsync());
						SemaphoreSlim wait = data.global ? _rlGlobal : bucket;
						await wait.WaitAsync(); // Removing 1 already in semaphore
						var tmr = new Timer(data.retry_after);
						tmr.Elapsed += (sender, e) => {
							wait.Release();
						};
						tmr.Start();
						await wait.WaitAsync();
						_sync.Release();
						goAgain = true;
						break;
					default:
						if ((int)msg.StatusCode >= 500) {
							goAgain = true;
						} else {
							res = JsonConvert.DeserializeObject<T>(await msg.Content.ReadAsStringAsync());
						}
						break;
				}
			} while (goAgain);
			return res;
		}
		protected async Task<T> GetResponseAsync<T>(SemaphoreSlim bucket, HttpMethod method, string url) {
			return await GetResponseAsync<T>(bucket, method, url, null);
		}
		// CDN
		[DllImport("libwebp.dll")]
		// ReSharper disable once InconsistentNaming (extern method)
		private static unsafe extern byte* WebPDecodeARGB(byte* data, UIntPtr data_size, int* width, int* height);
		[DllImport("libwebp.dll")]
		private static unsafe extern void WebPFree(void* data);
		private static unsafe Image WebPDecode(byte[] webp) {
			byte* decoded;
			int width = 0;
			int height = 0;
			fixed (byte* webpData = webp) {
				decoded = WebPDecodeARGB(webpData, (UIntPtr)webp.Length, &width, &height);
			}
			int length = 4*width*height;
			// a, r, g, b, a, r, g, b...
			var res = new Bitmap(width, height, PixelFormat.Format32bppArgb);
			BitmapData data = res.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, res.PixelFormat);
			var dataPtr = (byte*)data.Scan0;
			var dataBuf = new byte[length];
			Marshal.Copy((IntPtr)dataPtr, dataBuf, 0, length);
			var dcBuf = new byte[length];
			Marshal.Copy((IntPtr)decoded, dcBuf, 0, length);
			dcBuf.CopyTo(dataBuf, 0);
			res.UnlockBits(data);
			WebPFree(decoded);
			return res;
		}
		// ReSharper disable once MemberCanBeMadeStatic.Global
		public async Task<Image> GetIconAsync(IHasIcon i) {
			bool goAgain;
			Image res = null;
			do {
				goAgain = false;
				var msg = new HttpRequestMessage(HttpMethod.Get, i.Icon);
				HttpResponseMessage msg2 = await _client.SendAsync(msg);
				switch ((int)msg2.StatusCode) {
					case 200:
						switch (i.IconFormat.ToLower()) {
							case "webp":
								res = WebPDecode(await msg2.Content.ReadAsByteArrayAsync());
								break;
							case "jpg":
							case "jpeg":
							case "png":
							case "gif":
								res = Image.FromStream(await msg2.Content.ReadAsStreamAsync());
								break;
							default:
								throw new UnknownImageFormatException("Image format " + i.IconFormat + " is not supported.");
						}
						res = Image.FromStream(await msg2.Content.ReadAsStreamAsync());
						break;
					case 404:
						throw new NotFoundException("Icon " + i.Icon + " does not exist on the Discord CDN.");
					default:
						goAgain = true;
						break;
				}
			} while (goAgain);
			return res;
		}
		public Image GetIcon(IHasIcon i) {
			return GetIconAsync(i).Await();
		}
		/*
		Buckets:
		Same if path is the same and all major params are the same
		Major params are:
			Channel ID
			Guild ID
			Webhook ID
		Message DELETE has its own seperate bucket
		*/
		// Bucket: Dynamic Channel
		private readonly BucketMap _channelBuckets = new BucketMap();
		internal async Task<ChannelJson> GetChannelJsonAsync(ulong channelId) {
			return await GetResponseAsync<ChannelJson>(_channelBuckets[channelId], HttpMethod.Get, _baseUrl + "channels/" + channelId);
		}
		internal async Task<JObject[]> GetPinnedMessagesAsync(ulong channelId) {
			return await GetResponseAsync<JObject[]>(_channelBuckets[channelId], HttpMethod.Get, _baseUrl + "channels/" + channelId + "/pins");
		}
		// Bucket: Dynamic Server
		private readonly BucketMap _serverBuckets = new BucketMap();
		internal async Task<ServerJson> GetServerJsonAsync(ulong serverId) {
			return await GetResponseAsync<ServerJson>(_serverBuckets[serverId], HttpMethod.Get, _baseUrl + "guilds/" + serverId);
		}
		internal async Task<RoleJson[]> GetServerRolesJsonAsync(ulong serverId) {
			return await GetResponseAsync<RoleJson[]>(_serverBuckets[serverId], HttpMethod.Get, _baseUrl + "guilds/" + serverId + "/roles");
		}
		internal async Task<EmojiJson> GetServerEmojiJsonAsync(ulong emojiId, ulong serverId) {
			return await GetResponseAsync<EmojiJson>(_serverBuckets[serverId], HttpMethod.Get, _baseUrl + "guilds/" + serverId + "/emojis/" + emojiId);
		}
		// Bucket: users/@me
		private readonly SemaphoreSlim _meBucket = new SemaphoreSlim(1);
		public async Task<User> GetClientUserAsync() {
			await _cuMutex.WaitAsync();
			try {
				_clientUser = User.CreateFromJson(await GetResponseAsync<UserJson>(_meBucket, HttpMethod.Get, _baseUrl + "users/@me"), null);
			} finally {
				_cuMutex.Release();
			}
			return _clientUser;
		}
		public User GetClientUser() {
			return GetClientUserAsync().Await();
		}
		// Bucket: users/{user_id}
		private readonly SemaphoreSlim _userBucket = new SemaphoreSlim(1);
		internal async Task<UserJson> GetUserJsonAsync(ulong userId) {
			return await GetResponseAsync<UserJson>(_userBucket, HttpMethod.Get, _baseUrl + "users/" + userId);
		}

		public async Task<IChannel> GetChannelAsync(ulong channelId) {
			return await ChannelUtils.GetChannelAsync(channelId, this);
		}
		public IChannel GetChannel(ulong channelId) {
			return ChannelUtils.GetChannel(channelId, this);
		}
		public async Task<IServerChannel> GetServerChannelAsync(ulong channelId) {
			return await ChannelUtils.GetServerChannelAsync(channelId, this);
		}
		public IServerChannel GetServerChannel(ulong channelId) {
			return ChannelUtils.GetServerChannel(channelId, this);
		}
		public async Task<INonCatServerChannel> GetNonCatServerChannelAsync(ulong channelId) {
			return await ChannelUtils.GetNonCatServerChannelAsync(channelId, this);
		}
		public INonCatServerChannel GetNonCatServerChannel(ulong channelId) {
			return ChannelUtils.GetNonCatServerChannel(channelId, this);
		}
		/*
		public async Task<IVoiceChannel> GetVoiceChannelAsync(ulong channelId) {
			return await ChannelUtils.GetVoiceChannelAsync(channelId, this);
		}
		public IVoiceChannel GetVoiceChannel(ulong channelId) {
			return ChannelUtils.GetVoiceChannel(channelId, this);
		}
		*/
		public async Task<TextChannel> GetTextChannelAsync(ulong channelId) {
			return await TextChannel.GetAsync(channelId, this);
		}
		public TextChannel GetTextChannel(ulong channelId) {
			return TextChannel.Get(channelId, this);
		}
		public async Task<ChannelCategory> GetChannelCategoryAsync(ulong categoryId) {
			return await ChannelCategory.GetAsync(categoryId, this);
		}
		public ChannelCategory GetChannelCategory(ulong categoryId) {
			return ChannelCategory.Get(categoryId, this);
		}
		public async Task<IDirectMessageChannel> GetDirectMessageChannelAsync(ulong channelId) {
			return await ChannelUtils.GetDirectMessageChannelAsync(channelId, this);
		}
		public IDirectMessageChannel GetDirectMessageChannel(ulong channelId) {
			return ChannelUtils.GetDirectMessageChannel(channelId, this);
		}
		public async Task<DirectMessageTextChannel> GetDirectMessageTextChannelAsync(ulong channelId) {
			return await DirectMessageTextChannel.GetAsync(channelId, this);
		}
		public DirectMessageTextChannel GetDirectMessageTextChannel(ulong channelId) {
			return DirectMessageTextChannel.Get(channelId, this);
		}
		public async Task<ServerTextChannel> GetServerTextChannelAsync(ulong channelId) {
			return await ServerTextChannel.GetAsync(channelId, this);
		}
		public ServerTextChannel GetServerTextChannel(ulong channelId) {
			return ServerTextChannel.Get(channelId, this);
		}
		/*
		public async Task<ServerVoiceChannel> GetServerVoiceChannelAsync(ulong channelId) {
			return await ServerVoiceChannel.GetAsync(channelId, this);
		}
		public ServerVoiceChannel GetServerVoiceChannel(ulong channelId) {
			return ServerVoiceChannel.Get(channelId, this);
		}
		*/
		public async Task<Server> GetServerAsync(ulong serverId) {
			return await Server.GetAsync(serverId, this);
		}
		public Server GetServer(ulong serverId) {
			return Server.Get(serverId, this);
		}
		public async Task<ServerEmoji> GetEmojiAsync(ulong emojiId, ulong serverId) {
			return await ServerEmoji.GetAsync(emojiId, serverId, this);
		}
		public async Task<ServerEmoji> GetEmojiAsync(ulong emojiId, Server server) {
			return await ServerEmoji.GetAsync(emojiId, server, this);
		}
		public ServerEmoji GetEmoji(ulong emojiId, ulong serverId) {
			return ServerEmoji.Get(emojiId, serverId, this);
		}
		public ServerEmoji GetEmoji(ulong emojiId, Server server) {
			return ServerEmoji.Get(emojiId, server, this);
		}
		public async Task<Role> GetRoleAsync(ulong roleId, ulong serverId) {
			return await Role.GetAsync(roleId, serverId, this);
		}
		public async Task<Role> GetRoleAsync(ulong roleId, Server server) {
			return await Role.GetAsync(roleId, server, this);
		}
		public Role GetRole(ulong roleId, ulong serverId) {
			return Role.Get(roleId, serverId, this);
		}
		public Role GetRole(ulong roleId, Server server) {
			return Role.Get(roleId, server, this);
		}
		public async Task<User> GetUserAsync(ulong userId) {
			return await User.GetAsync(userId, this);
		}
		public User GetUser(ulong userId) {
			return User.Get(userId, this);
		}
		public override string ToString() {
			return "Client: " + Token;
		}
	}
}
