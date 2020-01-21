using Twitchie2.Messages;

namespace Twitchie2.Events
{
	public class PartEventArgs : TwitchieEventArgs
	{
		public string Username { get; }

		public PartEventArgs(Twitchie twitchie, TwitchIrcMessage message) : base(twitchie)
		{
			Username = message.PopUserHostArgument().username;

			// PART
			message.SkipArguments(1);

			Channel = message.GetRemainingMessage();
		}
	}
}