using System;

namespace Twitchie2.Events
{
	public class ModeEventArgs : EventArgs
	{
		public bool AddingModerator { get; }
		public string Username { get; }
		public string Channel { get; }

		public ModeEventArgs(string message)
		{
			var splittedMessage = message.Split(' ');

			if (splittedMessage[2].StartsWith("#"))
			{
				Channel = splittedMessage[2];
			}

			if (splittedMessage[3].Equals("+o"))
			{
				AddingModerator = true;
			}

			Username = splittedMessage[4];
		}
	}
}