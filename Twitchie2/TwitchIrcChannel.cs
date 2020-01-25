using System;

namespace Twitchie2
{
	public class TwitchIrcChannel
	{
		public string Name { get; }

		public bool Joined { get; private set; }

		public TwitchIrcChannel(string channel)
		{
			if (string.IsNullOrWhiteSpace(channel))
				throw new ArgumentException("Invalid channel! Channel can not be null or whitespace.");

			Name = channel[0] == '#' ? channel : '#' + channel;
		}

		public void SendMessage(string message)
			=> Twitchie.Instance.SendMessage(Name, message);

		public void SendAction(string action)
			=> Twitchie.Instance.SendAction(Name, action);

		public void SendMention(string user, string message)
			=> Twitchie.Instance.SendMention(Name, user, message);

		public void SendWhisper(string user, string message)
			=> Twitchie.Instance.SendWhisper(Name, user, message);

		public void Join()
		{
			if (!Joined)
			{
				Twitchie.Instance.WriteIrcMessage($"JOIN {Name}");
				Joined = true;
			}
		}

		public void Part()
		{
			if (Joined)
			{
				Twitchie.Instance.WriteIrcMessage($"PART {Name}");
				Joined = false;
			}
		}
	}
}