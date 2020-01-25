using Twitchie2.Messages;

namespace Twitchie2.Events
{
	public class ModeEventArgs
	{
		public bool AddingModerator { get; }
		public string Username { get; }
		public TwitchIrcChannel Channel { get; }

		public ModeEventArgs(TwitchIrcMessage message)
		{
			// :jtv MODE #<channel> +o/-o <username>
			message.SkipArguments(2);

			var channel = message.PopArgument();
			Channel = Twitchie.Instance.Channels.Find(x => x.Name == channel);

			AddingModerator = message.PopArgument() == "+o";
			Username = message.PopArgument();
		}
	}
}