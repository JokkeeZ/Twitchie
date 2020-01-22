using System;
using System.Collections.Generic;
using Twitchie2.Messages;

namespace Twitchie2.Events
{
	public class UserNoticeEventArgs : TwitchieEventArgs
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
		public int RoomId { get; }
		public string SystemMsg { get; }
		public int UserId { get; }
		public string Message { get; }
		public string Timestamp { get; }

		public UserNoticeMsgParams MsgParams { get; } = new UserNoticeMsgParams();

		public UserNoticeEventArgs(Twitchie twitchie, TwitchIrcMessage message) : base(twitchie)
		{
			var arg = message.PopDictionaryArgument();
			BadgeInfo = arg.GetValue<string>("badge-info");

			var badges = arg.GetValue<string>("badges");
			if (!string.IsNullOrWhiteSpace(badges))
			{
				foreach (var badge in badges.Split(','))
				{
					var split = badge.Split('/');
					Badges.Add(new TwitchBadge(split[0], split[1]));
				}
			}

			Color = arg.GetValue<string>("color");
			DisplayName = arg.GetValue<string>("display-name");
			Emotes = arg.GetValue<string>("emotes");
			MessageId = arg.GetValue<string>("id");
			LoginName = arg.GetValue<string>("login");
			Mod = arg.GetValue<int>("mod") == 1;
			MsgId = (NoticeType)Enum.Parse(typeof(NoticeType), arg.GetValue<string>("msg-id"), true);
			RoomId = arg.GetValue<int>("room-id");
			SystemMsg = arg.GetValue<string>("system-msg");
			Timestamp = arg.GetValue<string>("tmi-sent-ts");
			UserId = arg.GetValue<int>("user-id");

			// Additional parameters.
			MsgParams.CumulativeMonths = arg.GetValue<int>("msg-param-cumulative-months");
			MsgParams.DisplayName = arg.GetValue<string>("msg-param-displayName");
			MsgParams.Login = arg.GetValue<string>("msg-param-login");
			MsgParams.Months = arg.GetValue<int>("msg-param-months");
			MsgParams.PromoGiftTotal = arg.GetValue<int>("msg-param-promo-gift-total");
			MsgParams.PromoName = arg.GetValue<string>("msg-param-promo-name");
			MsgParams.RecipientDisplayName = arg.GetValue<string>("msg-param-recipient-display-name");
			MsgParams.RecipientId = arg.GetValue<string>("msg-param-recipient-id");
			MsgParams.RecipientUsername = arg.GetValue<string>("msg-param-recipient-user-name");
			MsgParams.SenderLogin = arg.GetValue<string>("msg-param-sender-login");
			MsgParams.SenderName = arg.GetValue<string>("msg-param-sender-name");
			MsgParams.ShouldShareStreak = arg.GetValue<int>("msg-param-should-share-streak") == 1;
			MsgParams.StreakMonths = arg.GetValue<int>("msg-param-streak-months");
			MsgParams.SubPlan = arg.GetValue<string>("msg-param-sub-plan");
			MsgParams.SubPlanName = arg.GetValue<string>("msg-param-sub-plan-name");
			MsgParams.ViewerCount = arg.GetValue<int>("msg-param-viewerCount");
			MsgParams.RitualName = arg.GetValue<string>("msg-param-ritual-name");
			MsgParams.Threshold = arg.GetValue<string>("msg-param-threshold");

			// :tmi.twitch.tv USERNOTICE
			message.SkipArguments(2);

			Channel = message.PopArgument();
			Message = message.GetRemainingMessage();
		}
	}

	public class UserNoticeMsgParams
	{
		public int CumulativeMonths { get; internal set; }
		public string DisplayName { get; internal set; }
		public string Login { get; internal set; }
		public int Months { get; internal set; }
		public int PromoGiftTotal { get; internal set; }
		public string PromoName { get; internal set; }
		public string RecipientDisplayName { get; internal set; }
		public string RecipientId { get; internal set; }
		public string RecipientUsername { get; internal set; }
		public string SenderLogin { get; internal set; }
		public string SenderName { get; internal set; }
		public bool ShouldShareStreak { get; internal set; }
		public int StreakMonths { get; internal set; }
		public string SubPlan { get; internal set; }
		public string SubPlanName { get; internal set; }
		public int ViewerCount { get; internal set; }
		public string RitualName { get; internal set; }
		public string Threshold { get; internal set; }
	}
}
