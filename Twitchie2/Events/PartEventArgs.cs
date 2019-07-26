using System;

namespace Twitchie2.Events
{
	public class PartEventArgs : EventArgs
	{
		public string Username { get; }
		public string Channel { get; }

		public PartEventArgs(string message)
		{
			var splittedMessage = message.Split(' ');

			Username = splittedMessage[0].Split(':')[1].Split('!')[0];
			Channel = splittedMessage[2];
		}
	}
}