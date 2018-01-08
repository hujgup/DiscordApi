using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Echo.Utils;
using Echo.Utils.Async;
using Echo.Discord.Api.Channels;
using Echo.Discord.Api.Http;
using JetBrains.Annotations;

namespace Echo.Discord.Api.Messages {
	public partial class MessageBuilder {
		public const string MentionOpen = "<";
		public const string MentionClose = ">";
		private static readonly Regex _escapeRegex = new Regex(@"([<#= regexText #>])");
		private readonly StringBuilder _msg;
		private readonly HashSet<TextChannel> _channels;
		private readonly List<string> _open;
		public MessageBuilder() {
			_msg = new StringBuilder();
			_channels = new HashSet<TextChannel>();
			_open = new List<string>();
		}
		public static string Escape(string str) {
			return _escapeRegex.Replace(str, "\\$1");
		}
		public MessageBuilder AddChannel(TextChannel channel) {
			_channels.Add(channel);
			return this;
		}
		public MessageBuilder Text(string str) {
			_msg.Append(Escape(str));
			return this;
		}
		public static string CreateMentionString(LazyUser user, bool useNickname) {
			return useNickname ? user.MentionContent : user.MentionNoNicknameContent;
		}
		public static string CreateMentionString(IMentionable mentionable, [CanBeNull] Client client) {
			// TODO: Call mentionable.IsMentionableBy to determine when IsMentionable is Maybe (pass current user object).
			return mentionable.IsMentionable.IsTrue() ? mentionable.MentionContent : mentionable.MentionFallbackName;
		}
		public static string CreateMentionString(IMentionable mentionable) {
			return CreateMentionString(mentionable, null);
		}
		public MessageBuilder Mention(LazyUser user, bool useNickname) {
			_msg.Append(CreateMentionString(user, useNickname));
			return this;
		}
		public MessageBuilder Mention(IMentionable mentionable) {
			_msg.Append(CreateMentionString(mentionable, null));
			return this;
		}
		public MessageBuilder MentionEveryone() {
			// TODO: Mention everyone
			return this;
		}
		public MessageBuilder MentionHere() {
			// TODO: Mention here
			return this;
		}
		public async Task SendAsync() {
			string msgStr = ToString();
			var tasks = new List<Task>(_channels.Count);
			foreach (TextChannel channel in _channels) {
				tasks.Add(channel.SendMessageAsync(msgStr));
			}
			await Task.WhenAll(tasks);
		}
		public void Send() {
			SendAsync().Await();
		}
		public string Build() {
			if (_open.Count > 0) {
				throw new InvalidMessageStructureException("Cannot finalize message structure because the following block(s) have not been closed: [\"" + string.Join("\", \"", _open) + "\"].");
			}
			return _msg.ToString();
		}
		public override string ToString() {
			return _msg.ToString();
		}
	}
}
