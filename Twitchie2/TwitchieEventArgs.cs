﻿using System;

namespace Twitchie2
{
	public class TwitchieEventArgs : EventArgs
	{
		private readonly Twitchie twitchie;

		public string Channel { get; protected set; }

		public TwitchieEventArgs(Twitchie twitchie) => this.twitchie = twitchie;

		public void SendMessage(string message)
			=> twitchie.SendMessage(Channel, message);

		public void SendAction(string action)
			=> twitchie.SendAction(Channel, action);

		public void SendMention(string user, string message)
			=> twitchie.SendMention(Channel, user, message);

		public void SendWhisper(string user, string message)
			=> twitchie.SendWhisper(Channel, user, message);
	}
}