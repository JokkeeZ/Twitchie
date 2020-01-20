using System;
using System.Collections.Generic;

namespace Twitchie2.Events
{
	public class UserNoticeEventArgs : EventArgs
	{
		public string BadgeInfo { get; }
		public List<TwitchBadge> Badges { get; } = new List<TwitchBadge>();
		public string Color { get; }
		public string DisplayName { get; }
		public string Emotes { get; }
		public string MessageId { get; }
		public string LoginName { get; }
		public bool Mod { get; }
		public NoticeType MsgId { get; }
		public string RoomId { get; }
		public string SystemMsg { get; }
		public string UserId { get; }
		public string Channel { get; }
		public string Message { get; }

		public UserNoticeMsgParams MsgParams { get; } = new UserNoticeMsgParams();

		public UserNoticeEventArgs(string message)
		{
			var msg = KeyValueMessage.Parse(message);

			if (msg.TryGetValue("badge-info", out var badgeInfo))
				BadgeInfo = badgeInfo;

			if (msg.TryGetValue("badges", out var badges) && (badges.Contains("/")))
			{
				if (!badges.Contains(","))
				{
					var split = badges.Split('/');
					Badges.Add(new TwitchBadge(split[0], split[1]));
				}

				foreach (var badge in badges.Split(','))
				{
					var split = badge.Split('/');
					Badges.Add(new TwitchBadge(split[0], split[1]));
				}
			}

			if (msg.TryGetValue("color", out var color))
				Color = color;

			if (msg.TryGetValue("display-name", out var displayName))
				DisplayName = displayName;

			if (msg.TryGetValue("emotes", out var emotes))
				Emotes = emotes;

			if (msg.TryGetValue("id", out var id))
				MessageId = id;

			if (msg.TryGetValue("login", out var login))
				LoginName = login;

			if (msg.TryGetIntValue("mod", out var mod))
				Mod = mod == 1;

			if (msg.TryGetValue("msg-id", out var msgId))
				MsgId = (NoticeType)Enum.Parse(typeof(NoticeType), msgId, true);

			if (msg.TryGetValue("room-id", out var roomId))
				RoomId = roomId;

			if (msg.TryGetValue("system-msg", out var systemMsg))
				SystemMsg = systemMsg;

			if (msg.TryGetValue("user-id", out var userId))
				UserId = userId;

			// Additional parameters
			if (msg.TryGetIntValue("msg-param-cumulative-months", out var msgParamCumulativeMonths))
				MsgParams.CumulativeMonths = msgParamCumulativeMonths;

			if (msg.TryGetValue("msg-param-displayName", out var msgParamDisplayName))
				MsgParams.DisplayName = msgParamDisplayName;

			if (msg.TryGetValue("msg-param-login", out var msgParamLogin))
				MsgParams.Login = msgParamLogin;

			if (msg.TryGetValue("msg-param-months", out var msgParamMonths))
				MsgParams.Months = msgParamMonths;

			if (msg.TryGetIntValue("msg-param-promo-gift-total", out var msgParamPromoGiftTotal))
				MsgParams.PromoGiftTotal = msgParamPromoGiftTotal;

			if (msg.TryGetValue("msg-param-promo-name", out var msgParamPromoName))
				MsgParams.PromoName = msgParamPromoName;

			if (msg.TryGetValue("msg-param-recipient-display-name", out var msgParamRecipientDisplayName))
				MsgParams.RecipientDisplayName = msgParamRecipientDisplayName;

			if (msg.TryGetValue("msg-param-recipient-id", out var msgParamRecipientId))
				MsgParams.RecipientId = msgParamRecipientId;

			if (msg.TryGetValue("msg-param-recipient-user-name", out var msgParamRecipientUsername))
				MsgParams.RecipientUsername = msgParamRecipientUsername;

			if (msg.TryGetValue("msg-param-sender-login", out var msgParamSenderLogin))
				MsgParams.SenderLogin = msgParamSenderLogin;

			if (msg.TryGetValue("msg-param-sender-name", out var msgParamSenderName))
				MsgParams.SenderName = msgParamSenderName;

			if (msg.TryGetIntValue("msg-param-should-share-streak", out var msgParamShouldShareStreak))
				MsgParams.ShouldShareStreak = msgParamShouldShareStreak == 1;

			if (msg.TryGetIntValue("msg-param-streak-months", out var msgParamStreakMonths))
				MsgParams.StreakMonths = msgParamStreakMonths;

			if (msg.TryGetValue("msg-param-sub-plan", out var msgParamSubPlan))
				MsgParams.SubPlan = msgParamSubPlan;

			if (msg.TryGetValue("msg-param-sub-plan-name", out var msgParamSubPlanName))
				MsgParams.SubPlanName = msgParamSubPlanName;

			if (msg.TryGetIntValue("msg-param-viewerCount", out var msgParamViewerCount))
				MsgParams.ViewerCount = msgParamViewerCount;

			if (msg.TryGetValue("msg-param-ritual-name", out var msgParamRitualName))
				MsgParams.RitualName = msgParamRitualName;

			if (msg.TryGetValue("msg-param-threshold", out var msgParamThreshold))
				MsgParams.Threshold = msgParamThreshold;

			var splittedMessage = message.Split(' ');
			Channel = splittedMessage[3];

			// Did user enter a message? 
			if (splittedMessage.Length > 4)
			{
				Message = message.Substring(message.IndexOf(Channel) + (Channel.Length + 2));
			}
		}
	}
}
