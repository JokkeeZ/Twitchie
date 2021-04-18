using System;
using Twitchie2.Messages;

namespace Twitchie2.Events
{
	public sealed class NoticeEventArgs : EventArgs
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

			Channel = Twitchie.Instance.Channels.Find(x => x.Name == message.PopArgument());

			Message = message.GetRemainingMessage(true);
		}
	}
}
