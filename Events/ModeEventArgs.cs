using System;
using Twitchie2.Messages;

namespace Twitchie2.Events
{
	public class ModeEventArgs : EventArgs
	{
		public bool AddingModerator { get; }
		public string Username { get; }
		public TwitchIrcChannel Channel { get; }

		public ModeEventArgs(TwitchIrcMessage message)
		{
			// :jtv MODE
			message.SkipArguments(2);

			Channel = Twitchie.Instance.Channels.Find(x => x.Name == message.PopArgument());

			AddingModerator = message.PopArgument() == "+o";
			Username = message.PopArgument();
		}
	}
}
