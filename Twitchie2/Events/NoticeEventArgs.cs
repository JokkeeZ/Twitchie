using Twitchie2.Messages;

namespace Twitchie2.Events
{
	public class NoticeEventArgs : TwitchieEventArgs
	{
		public string MessageId { get; }
		public string Message { get; }

		public NoticeEventArgs(Twitchie twitchie, TwitchIrcMessage message) : base(twitchie)
		{
			var arg = message.PopDictionaryArgument();
			MessageId = arg.GetValue<string>("msg-id");

			// :tmi.twitch.tv NOTICE
			message.SkipArguments(2);

			Channel = message.PopArgument();
			Message = message.GetRemainingMessage(true);
		}
	}
}