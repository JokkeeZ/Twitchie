using System;
using System.Collections.Generic;
using Twitchie2.Enums;
using Twitchie2.Messages;

namespace Twitchie2.Events
{
	public class UserNoticeEventArgs : EventArgs
	{
		public string BadgeInfo { get; }
		public List<TwitchBadge> Badges { get; }
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
		public TwitchIrcChannel Channel { get; }
		public UserNoticeMsgParams MsgParams { get; }

		public UserNoticeEventArgs(TwitchIrcMessage message)
		{
			var arg = message.PopDictionaryArgument();
			BadgeInfo = arg.GetValue<string>("badge-info");

			Badges = arg.PopBadges();

			Color = arg.GetValue<string>("color");
			DisplayName = arg.GetValue<string>("display-name");
			Emotes = arg.GetValue<string>("emotes");
			MessageId = arg.GetValue<string>("id");
			LoginName = arg.GetValue<string>("login");
			Mod = arg.GetValue<int>("mod") == 1;
			MsgId = (NoticeType)Enum.Parse(typeof(NoticeType), arg.GetValue<string>("msg-id"), true);
			RoomId = arg.GetValue<int>("room-id");
			SystemMsg = arg.GetValue<string>("system-msg").Replace("\\s", " ");
			Timestamp = arg.GetValue<string>("tmi-sent-ts");
			UserId = arg.GetValue<int>("user-id");

			// Additional parameters.
			MsgParams = new()
			{
				CumulativeMonths = arg.GetValue<int>("msg-param-cumulative-months"),
				DisplayName = arg.GetValue<string>("msg-param-displayName"),
				Login = arg.GetValue<string>("msg-param-login"),
				Months = arg.GetValue<int>("msg-param-months"),
				PromoGiftTotal = arg.GetValue<int>("msg-param-promo-gift-total"),
				PromoName = arg.GetValue<string>("msg-param-promo-name"),
				RecipientDisplayName = arg.GetValue<string>("msg-param-recipient-display-name"),
				RecipientId = arg.GetValue<string>("msg-param-recipient-id"),
				RecipientUsername = arg.GetValue<string>("msg-param-recipient-user-name"),
				SenderLogin = arg.GetValue<string>("msg-param-sender-login"),
				SenderName = arg.GetValue<string>("msg-param-sender-name"),
				ShouldShareStreak = arg.GetValue<int>("msg-param-should-share-streak") == 1,
				StreakMonths = arg.GetValue<int>("msg-param-streak-months"),
				SubPlan = arg.GetValue<string>("msg-param-sub-plan"),
				SubPlanName = arg.GetValue<string>("msg-param-sub-plan-name"),
				ViewerCount = arg.GetValue<int>("msg-param-viewerCount"),
				RitualName = arg.GetValue<string>("msg-param-ritual-name"),
				Threshold = arg.GetValue<string>("msg-param-threshold"),
				GiftMonths = arg.GetValue<int>("msg-param-gift-months")
			};

			// :tmi.twitch.tv USERNOTICE
			message.SkipArguments(2);

			Channel = Twitchie.Instance.Channels.Find(x => x.Name == message.PopArgument());
			Message = message.GetRemainingMessage();
		}
	}

	public class UserNoticeMsgParams
	{
		public int CumulativeMonths { get; init; }
		public string DisplayName { get; init; }
		public string Login { get; init; }
		public int Months { get; init; }
		public int PromoGiftTotal { get; init; }
		public string PromoName { get; init; }
		public string RecipientDisplayName { get; init; }
		public string RecipientId { get; init; }
		public string RecipientUsername { get; init; }
		public string SenderLogin { get; init; }
		public string SenderName { get; init; }
		public bool ShouldShareStreak { get; init; }
		public int StreakMonths { get; init; }
		public string SubPlan { get; init; }
		public string SubPlanName { get; init; }
		public int ViewerCount { get; init; }
		public string RitualName { get; init; }
		public string Threshold { get; init; }
		public int GiftMonths { get; init; }
	}
}
