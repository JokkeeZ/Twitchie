using Twitchie2.Messages;

namespace Twitchie2.Events
{
	public class PartEventArgs
	{
		public string Username { get; }
		public TwitchIrcChannel Channel { get; }

		public PartEventArgs(TwitchIrcMessage message)
		{
			Username = message.PopUserHostArgument().username;

			// PART
			message.SkipArguments(1);

			var channel = message.GetRemainingMessage();
			Channel = Twitchie.Instance.Channels.Find(x => x.Name == channel);
		}
	}
}