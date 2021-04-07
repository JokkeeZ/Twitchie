using System;
using Twitchie2.Messages;

namespace Twitchie2.Events
{
	public class PartEventArgs : EventArgs
	{
		public string Username { get; }
		public TwitchIrcChannel Channel { get; }

		public PartEventArgs(TwitchIrcMessage message)
		{
			Username = message.PopUserHostArgument().username;

			// PART
			message.SkipArguments(1);
			Channel = Twitchie.Instance.Channels.Find(x => x.Name == message.GetRemainingMessage());
		}
	}
}
