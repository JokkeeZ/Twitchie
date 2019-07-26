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

		#region Additional parameters
		public int MsgParamCumulativeMonths { get; }
		public string MsgParamDisplayName { get; }
		public string MsgParamLogin { get; }
		public string MsgParamMonths { get; }
		public int MsgParamPromoGiftTotal { get; }
		public string MsgParamPromoName { get; }
		public string MsgParamRecipientDisplayName { get; }
		public string MsgParamRecipientId { get; }
		public string MsgParamRecipientUsername { get; }
		public string MsgParamSenderLogin { get; }
		public string MsgParamSenderName { get; }

		//TODO: Actually boolean, don't know if its coming as int or bool on stream.
		public string MsgParamShouldShareStreak { get; }
		public int MsgParamStreakMonths { get; }
		public string MsgParamSubPlan { get; }
		public string MsgParamSubPlanName { get; }
		public int MsgParamViewerCount { get; }
		public string MsgParamRitualName { get; }
		public string MsgParamThreshold { get; }
		#endregion

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
				MsgParamCumulativeMonths = msgParamCumulativeMonths;
			}

			if (msg.TryGetValue("msg-param-displayName", out var msgParamDisplayName))
			{
				MsgParamDisplayName = msgParamDisplayName;
			}

			if (msg.TryGetValue("msg-param-login", out var msgParamLogin))
			{
				MsgParamLogin = msgParamLogin;
			}

			if (msg.TryGetValue("msg-param-months", out var msgParamMonths))
			{
				MsgParamMonths = msgParamMonths;
			}

			if (msg.TryGetIntValue("msg-param-promo-gift-total", out var msgParamPromoGiftTotal))
			{
				MsgParamPromoGiftTotal = msgParamPromoGiftTotal;
			}

			if (msg.TryGetValue("msg-param-promo-name", out var msgParamPromoName))
			{
				MsgParamPromoName = msgParamPromoName;
			}

			if (msg.TryGetValue("msg-param-recipient-display-name", out var msgParamRecipientDisplayName))
			{
				MsgParamRecipientDisplayName = msgParamRecipientDisplayName;
			}

			if (msg.TryGetValue("msg-param-recipient-id", out var msgParamRecipientId))
			{
				MsgParamRecipientId = msgParamRecipientId;
			}

			if (msg.TryGetValue("msg-param-recipient-user-name", out var msgParamRecipientUsername))
			{
				MsgParamRecipientUsername = msgParamRecipientUsername;
			}

			if (msg.TryGetValue("msg-param-sender-login", out var msgParamSenderLogin))
			{
				MsgParamSenderLogin = msgParamSenderLogin;
			}

			if (msg.TryGetValue("msg-param-sender-name", out var msgParamSenderName))
			{
				MsgParamSenderName = msgParamSenderName;
			}

			if (msg.TryGetValue("msg-param-should-share-streak", out var msgParamShouldShareStreak))
			{
				MsgParamShouldShareStreak = msgParamShouldShareStreak;
			}

			if (msg.TryGetIntValue("msg-param-streak-months", out var msgParamStreakMonths))
			{
				MsgParamStreakMonths = msgParamStreakMonths;
			}

			if (msg.TryGetValue("msg-param-sub-plan", out var msgParamSubPlan))
			{
				MsgParamSubPlan = msgParamSubPlan;
			}

			if (msg.TryGetValue("msg-param-sub-plan-name", out var msgParamSubPlanName))
			{
				MsgParamSubPlanName = msgParamSubPlanName;
			}

			if (msg.TryGetIntValue("msg-param-viewerCount", out var msgParamViewerCount))
			{
				MsgParamViewerCount = msgParamViewerCount;
			}

			if (msg.TryGetValue("msg-param-ritual-name", out var msgParamRitualName))
			{
				MsgParamRitualName = msgParamRitualName;
			}

			if (msg.TryGetValue("msg-param-threshold", out var msgParamThreshold))
			{
				MsgParamThreshold = msgParamThreshold;
			}
		}
	}

	public enum NoticeType
	{
		Sub,
		Resub,
		SubGift,
		AnonSubGift,
		SubMysteryGift,
		GiftPaidUpgrade,
		RewardGift,
		AnonGiftPaidUpgrade,
		Raid,
		Unraid,
		Ritual,
		BitsBadgetier
	}
}
