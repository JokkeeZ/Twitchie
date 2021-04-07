using System;
using Twitchie2.Messages;

namespace Twitchie2.Events
{
	public class ClearMessageEventArgs : EventArgs
	{
		public string Username { get; }
		public string Message { get; }
		public string TargetMsgId { get; }
		public TwitchIrcChannel Channel { get; }

		public ClearMessageEventArgs(TwitchIrcMessage message)
		{
			var arg = message.PopDictionaryArgument();

			Username = arg.GetValue<string>("login");
			TargetMsgId = arg.GetValue<string>("target-msg-id");

			//:tmi.twitch.tv CLEARMSG
			message.SkipArguments(2);

			Channel = Twitchie.Instance.Channels.Find(x => x.Name == message.PopArgument());

			Message = message.GetRemainingMessage(true);
		}
	}
}
