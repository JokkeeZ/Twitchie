﻿using System;
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

		public UserNoticeMsgParams MsgParams { get; } = new UserNoticeMsgParams();

		public UserNoticeEventArgs(string message)
		{
			var msg = KeyValueMessage.Parse(message);

			if (msg.TryGetValue("badge-info", out var badgeInfo))
			{
				BadgeInfo = badgeInfo;
			}

			if (msg.TryGetValue("badges", out var badges))
			{
				// There is no badges
				if (badges.Contains("/"))
				{
					if (!badges.Contains(","))
					{
						Badges.Add(new TwitchBadge
						{
							Badge = badges.Split('/')[0],
							Version = badges.Split('/')[1]
						});
					}

					foreach (var badge in badges.Split(','))
					{
						Badges.Add(new TwitchBadge
						{
							Badge = badge.Split('/')[0],
							Version = badge.Split('/')[1]
						});
					}
				}
			}

			if (msg.TryGetValue("color", out var color))
			{
				Color = color;
			}

			if (msg.TryGetValue("display-name", out var displayName))
			{
				DisplayName = displayName;
			}

			if (msg.TryGetValue("emotes", out var emotes))
			{
				Emotes = emotes;
			}

			if (msg.TryGetValue("id", out var id))
			{
				MessageId = id;
			}

			if (msg.TryGetValue("login", out var login))
			{
				LoginName = login;
			}

			if (msg.TryGetIntValue("mod", out var mod))
			{
				Mod = mod == 1;
			}

			if (msg.TryGetValue("msg-id", out var msgId))
			{
				switch (msgId)
				{
					case "sub":
						MsgId = NoticeType.Sub;
						break;
					case "resub":
						MsgId = NoticeType.Resub;
						break;
					case "subgift":
						MsgId = NoticeType.SubGift;
						break;
					case "anonsubgift":
						MsgId = NoticeType.AnonSubGift;
						break;
					case "submysterygift":
						MsgId = NoticeType.SubMysteryGift;
						break;
					case "giftpaidupgrade":
						MsgId = NoticeType.GiftPaidUpgrade;
						break;
					case "rewardgift":
						MsgId = NoticeType.RewardGift;
						break;
					case "anongiftpaidupgrade":
						MsgId = NoticeType.AnonGiftPaidUpgrade;
						break;
					case "raid":
						MsgId = NoticeType.Raid;
						break;
					case "unraid":
						MsgId = NoticeType.Unraid;
						break;
					case "ritual":
						MsgId = NoticeType.Ritual;
						break;
					case "bitsbadgetier":
						MsgId = NoticeType.BitsBadgetier;
						break;
				}
			}

			if (msg.TryGetValue("room-id", out var roomId))
			{
				RoomId = roomId;
			}

			if (msg.TryGetValue("system-msg", out var systemMsg))
			{
				SystemMsg = systemMsg;
			}

			if (msg.TryGetValue("user-id", out var userId))
			{
				UserId = userId;
			}

			// Additional parameters
			if (msg.TryGetIntValue("msg-param-cumulative-months", out var msgParamCumulativeMonths))
			{
				MsgParams.MsgParamCumulativeMonths = msgParamCumulativeMonths;
			}

			if (msg.TryGetValue("msg-param-displayName", out var msgParamDisplayName))
			{
				MsgParams.MsgParamDisplayName = msgParamDisplayName;
			}

			if (msg.TryGetValue("msg-param-login", out var msgParamLogin))
			{
				MsgParams.MsgParamLogin = msgParamLogin;
			}

			if (msg.TryGetValue("msg-param-months", out var msgParamMonths))
			{
				MsgParams.MsgParamMonths = msgParamMonths;
			}

			if (msg.TryGetIntValue("msg-param-promo-gift-total", out var msgParamPromoGiftTotal))
			{
				MsgParams.MsgParamPromoGiftTotal = msgParamPromoGiftTotal;
			}

			if (msg.TryGetValue("msg-param-promo-name", out var msgParamPromoName))
			{
				MsgParams.MsgParamPromoName = msgParamPromoName;
			}

			if (msg.TryGetValue("msg-param-recipient-display-name", out var msgParamRecipientDisplayName))
			{
				MsgParams.MsgParamRecipientDisplayName = msgParamRecipientDisplayName;
			}

			if (msg.TryGetValue("msg-param-recipient-id", out var msgParamRecipientId))
			{
				MsgParams.MsgParamRecipientId = msgParamRecipientId;
			}

			if (msg.TryGetValue("msg-param-recipient-user-name", out var msgParamRecipientUsername))
			{
				MsgParams.MsgParamRecipientUsername = msgParamRecipientUsername;
			}

			if (msg.TryGetValue("msg-param-sender-login", out var msgParamSenderLogin))
			{
				MsgParams.MsgParamSenderLogin = msgParamSenderLogin;
			}

			if (msg.TryGetValue("msg-param-sender-name", out var msgParamSenderName))
			{
				MsgParams.MsgParamSenderName = msgParamSenderName;
			}

			if (msg.TryGetIntValue("msg-param-should-share-streak", out var msgParamShouldShareStreak))
			{
				MsgParams.MsgParamShouldShareStreak = msgParamShouldShareStreak == 1;
			}

			if (msg.TryGetIntValue("msg-param-streak-months", out var msgParamStreakMonths))
			{
				MsgParams.MsgParamStreakMonths = msgParamStreakMonths;
			}

			if (msg.TryGetValue("msg-param-sub-plan", out var msgParamSubPlan))
			{
				MsgParams.MsgParamSubPlan = msgParamSubPlan;
			}

			if (msg.TryGetValue("msg-param-sub-plan-name", out var msgParamSubPlanName))
			{
				MsgParams.MsgParamSubPlanName = msgParamSubPlanName;
			}

			if (msg.TryGetIntValue("msg-param-viewerCount", out var msgParamViewerCount))
			{
				MsgParams.MsgParamViewerCount = msgParamViewerCount;
			}

			if (msg.TryGetValue("msg-param-ritual-name", out var msgParamRitualName))
			{
				MsgParams.MsgParamRitualName = msgParamRitualName;
			}

			if (msg.TryGetValue("msg-param-threshold", out var msgParamThreshold))
			{
				MsgParams.MsgParamThreshold = msgParamThreshold;
			}
		}
	}
}