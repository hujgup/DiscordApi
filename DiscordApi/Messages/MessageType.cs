using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Echo.Discord.Api.Messages {
	/// <summary>
	/// Specifies what kind of user send a specific message.
	/// </summary>
	public enum MessageType {
		/// <summary>
		/// An OAuth user sent the message.
		/// </summary>
		User,
		/// <summary>
		/// A tokened user sent the message.
		/// </summary>
		Bot,
		/// <summary>
		/// A webhook sent the message.
		/// </summary>
		Webhook
	}
}
