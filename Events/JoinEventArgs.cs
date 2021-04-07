using System;
using Twitchie2.Messages;

namespace Twitchie2.Events
{
	public class JoinEventArgs : EventArgs
	{
		public string Username { get; }
		public string Host { get; }
		public TwitchIrcChannel Channel { get; }

		public JoinEventArgs(TwitchIrcMessage message)
		{
			(Username, Host) = message.PopUserHostArgument();

			// JOIN
			message.SkipArguments(1);
			Channel = Twitchie.Instance.Channels.Find(x => x.Name == message.GetRemainingMessage());
		}
	}
}
