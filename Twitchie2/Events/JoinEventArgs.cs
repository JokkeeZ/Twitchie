using Twitchie2.Messages;

namespace Twitchie2.Events
{
	public class JoinEventArgs : TwitchieEventArgs
	{
		public string Username { get; }

		public string Host { get; }

		public JoinEventArgs(Twitchie twitchie, TwitchIrcMessage message) : base(twitchie)
		{
			var (username, host) = message.PopUserHostArgument();
			Username = username;
			Host = host;

			// JOIN
			message.SkipArguments(1);

			Channel = message.GetRemainingMessage();
		}
	}
}