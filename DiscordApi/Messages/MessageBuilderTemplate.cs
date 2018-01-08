using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Echo.Discord.Api.Channels;

namespace Echo.Discord.Api.Messages {
	public partial class MessageBuilder {
		public const string BoldDelim = "**";
		public const string ItalicsDelim = "*";
		public const string UnderscoreDelim = "__";
		public const string StrikethroughDelim = "~~";
		public const string InlineBlockDelim = "`";
		public bool InBoldBlock() {
			return _open.Contains("Bold");
		}
		public MessageBuilder OpenBold() {
			if (InBoldBlock()) {
				throw new InvalidMessageStructureException("Cannot open a Bold block because one is already open - nesting blocks of the same type is not allowed.");
			}
			_msg.Append(BoldDelim);
			_open.Add("Bold");
			return this;
		}
		public MessageBuilder CloseBold() {
			if (!InBoldBlock()) {
				throw new InvalidMessageStructureException("Cannot close a Bold block because one is not open.");
			}
			_msg.Append(BoldDelim);
			_open.Remove("Bold");
			return this;
		}
		public bool InItalicsBlock() {
			return _open.Contains("Italics");
		}
		public MessageBuilder OpenItalics() {
			if (InItalicsBlock()) {
				throw new InvalidMessageStructureException("Cannot open a Italics block because one is already open - nesting blocks of the same type is not allowed.");
			}
			_msg.Append(ItalicsDelim);
			_open.Add("Italics");
			return this;
		}
		public MessageBuilder CloseItalics() {
			if (!InItalicsBlock()) {
				throw new InvalidMessageStructureException("Cannot close a Italics block because one is not open.");
			}
			_msg.Append(ItalicsDelim);
			_open.Remove("Italics");
			return this;
		}
		public bool InUnderscoreBlock() {
			return _open.Contains("Underscore");
		}
		public MessageBuilder OpenUnderscore() {
			if (InUnderscoreBlock()) {
				throw new InvalidMessageStructureException("Cannot open a Underscore block because one is already open - nesting blocks of the same type is not allowed.");
			}
			_msg.Append(UnderscoreDelim);
			_open.Add("Underscore");
			return this;
		}
		public MessageBuilder CloseUnderscore() {
			if (!InUnderscoreBlock()) {
				throw new InvalidMessageStructureException("Cannot close a Underscore block because one is not open.");
			}
			_msg.Append(UnderscoreDelim);
			_open.Remove("Underscore");
			return this;
		}
		public bool InStrikethroughBlock() {
			return _open.Contains("Strikethrough");
		}
		public MessageBuilder OpenStrikethrough() {
			if (InStrikethroughBlock()) {
				throw new InvalidMessageStructureException("Cannot open a Strikethrough block because one is already open - nesting blocks of the same type is not allowed.");
			}
			_msg.Append(StrikethroughDelim);
			_open.Add("Strikethrough");
			return this;
		}
		public MessageBuilder CloseStrikethrough() {
			if (!InStrikethroughBlock()) {
				throw new InvalidMessageStructureException("Cannot close a Strikethrough block because one is not open.");
			}
			_msg.Append(StrikethroughDelim);
			_open.Remove("Strikethrough");
			return this;
		}
		public bool InInlineBlockBlock() {
			return _open.Contains("InlineBlock");
		}
		public MessageBuilder OpenInlineBlock() {
			if (InInlineBlockBlock()) {
				throw new InvalidMessageStructureException("Cannot open a InlineBlock block because one is already open - nesting blocks of the same type is not allowed.");
			}
			_msg.Append(InlineBlockDelim);
			_open.Add("InlineBlock");
			return this;
		}
		public MessageBuilder CloseInlineBlock() {
			if (!InInlineBlockBlock()) {
				throw new InvalidMessageStructureException("Cannot close a InlineBlock block because one is not open.");
			}
			_msg.Append(InlineBlockDelim);
			_open.Remove("InlineBlock");
			return this;
		}
	}
}
