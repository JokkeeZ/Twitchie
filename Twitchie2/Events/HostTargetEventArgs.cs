using Twitchie2.Messages;

namespace Twitchie2.Events
{
	public class HostTargetEventArgs : TwitchieEventArgs
	{
		public int Viewers { get; }
		public string TargetChannel { get; }
		public bool IsStarting { get; }

		public HostTargetEventArgs(Twitchie twitchie, TwitchIrcMessage message) : base(twitchie)
		{
			message.SkipArguments(2);

			Channel = message.PopArgument();

			var targetChannel = message.PopArgument();
			IsStarting = targetChannel != ":-";
			TargetChannel = IsStarting ? targetChannel.Substring(1) : string.Empty;
			Viewers = message.GetRemainingArgument<int>();
		}
	}
}