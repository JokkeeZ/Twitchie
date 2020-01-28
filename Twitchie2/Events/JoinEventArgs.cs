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
			var (username, host) = message.PopUserHostArgument();
			Username = username;
			Host = host;

			// JOIN
			message.SkipArguments(1);

			var channel = message.GetRemainingMessage();
			Channel = Twitchie.Instance.Channels.Find(x => x.Name == channel);
		}
	}
}