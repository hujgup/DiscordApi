using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Echo.Discord.Api {
	public enum PermissionMask : uint {
		None = 0,
		CreateServerInvites = 0x1,
		KickMembers = 0x2,
		BanMembers = 0x4,
		Administrator = 0x8,
		ManageChannels = 0x10,
		ManageServer = 0x20,
		AddReactions = 0x40,
		ViewAuditLog = 0x80,
		[Obsolete("Use PermissionMask.ViewChannel instead.")]
		ReadMessages = 0x400,
		ViewChannel = 0x400,
		SendMessages = 0x800,
		SendTextToSpeechMessages = 0x1000,
		ManageMessages = 0x2000,
		EmbedLinks = 0x4000,
		AttachFiles = 0x8000,
		ReadMessageHistory = 0x10000,
		MentionEveryone = 0x20000,
		UseExternalEmojis = 0x40000,
		ConnectToVoiceChannel = 0x100000,
		SpeakInVoiceChannel = 0x200000,
		MuteVoiceChannelMembers = 0x400000,
		DeafenVoiceChannelMembers = 0x800000,
		MoveMembersBetweenVoiceChannels = 0x1000000,
		UseVoiceActivityDetection = 0x2000000,
		ChangeOwnNickname = 0x4000000,
		ChangeOtherUsersNicknames = 0x8000000,
		ManageRoles = 0x10000000,
		ManageWebhooks = 0x20000000,
		ManageEmojis = 0x40000000
	}
}
