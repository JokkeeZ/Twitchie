using Twitchie2.Messages;

namespace Twitchie2.Events
{
	public class ModeEventArgs : TwitchieEventArgs
	{
		public bool AddingModerator { get; }
		public string Username { get; }

		public ModeEventArgs(Twitchie twitchie, TwitchIrcMessage message) : base(twitchie)
		{
			// :jtv MODE #<channel> +o/-o <username>
			message.SkipArguments(2);

			Channel = message.PopArgument();
			AddingModerator = message.PopArgument() == "+o";
			Username = message.PopArgument();
		}
	}
}