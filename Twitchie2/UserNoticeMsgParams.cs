namespace Twitchie2
{
	public class UserNoticeMsgParams
	{
		public int CumulativeMonths { get; internal set; }
		public string DisplayName { get; internal set; }
		public string Login { get; internal set; }
		public string Months { get; internal set; }
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
