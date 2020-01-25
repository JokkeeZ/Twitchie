using Twitchie2.Messages;

namespace Twitchie2.Events
{
	public class NoticeEventArgs
	{
		public string MessageId { get; }
		public string Message { get; }
		public TwitchIrcChannel Channel { get; }

		public NoticeEventArgs(TwitchIrcMessage message)
		{
			var arg = message.PopDictionaryArgument();
			MessageId = arg.GetValue<string>("msg-id");

			// :tmi.twitch.tv NOTICE
			message.SkipArguments(2);

			var channel = message.PopArgument();
			Channel = Twitchie.Instance.Channels.Find(x => x.Name == channel);

			Message = message.GetRemainingMessage(true);
		}
	}
}