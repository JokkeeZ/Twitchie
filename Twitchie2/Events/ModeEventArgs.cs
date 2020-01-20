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
			var splitted = message.Split(' ');

			if (splitted[2].StartsWith("#"))
				Channel = splitted[2];

			if (splitted[3].Equals("+o"))
				AddingModerator = true;

			Username = splitted[4];
		}
	}
}