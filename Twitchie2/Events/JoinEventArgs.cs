using System;

namespace Twitchie2.Events
{
	public class JoinEventArgs : EventArgs
	{
		public string Username { get; }
		public string Channel { get; }

		public JoinEventArgs(string message)
		{
			Username = message.Split(' ')[0].Split(':')[1].Split('!')[0];
			Channel = message.Split(' ')[2];
		}
	}
}