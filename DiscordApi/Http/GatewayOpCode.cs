﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Echo.Discord.Api.Http {
	internal enum GatewayOpCode {
		Dispatch = 0,
		Heartbeat = 1,
		Identify = 2,
		StatusUpdate = 3,
		VoiceStateUpdate = 4,
		VoiceServerPing = 5,
		Resume = 6,
		Reconnect = 7,
		RequestGuildMembers = 8,
		InvalidSession = 9,
		Hello = 10,
		HeartbeatAck = 11
	}
}
