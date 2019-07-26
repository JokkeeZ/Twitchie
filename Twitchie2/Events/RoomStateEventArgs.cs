using System;

namespace Twitchie2.Events
{
	public class RoomStateEventArgs : EventArgs
	{
		public bool EmoteOnly { get; }
		public int FollowersOnly { get; }
		public bool R9k { get; }
		public int SlowMode { get; }
		public bool SubOnly { get; }
		public string Channel { get; }

		public RoomStateEventArgs(string message)
		{
			var splittedMessage = message.Split(' ');
			Channel = splittedMessage[3];

			if (!splittedMessage[0].Contains(";"))
			{
				var key = splittedMessage[0].Substring(1);
				var val = int.Parse(splittedMessage[0].Split('=')[1]);

				if (key == "emote-only")
				{
					EmoteOnly = val == 1;
				}

				if (key == "followers-only")
				{
					FollowersOnly = val;
				}

				if (key == "r9k")
				{
					R9k = val == 1;
				}

				if (key == "slow")
				{
					SlowMode = val;
				}

				if (key == "subs-only")
				{
					SubOnly = val == 1;
				}
			}
			else
			{
				var msg = KeyValueMessage.Parse(message);

				if (msg.TryGetValue("emote-only", out var emoteOnly))
				{
					EmoteOnly = int.Parse(emoteOnly) == 1;
				}

				if (msg.TryGetValue("followers-only", out var followersOnly))
				{
					FollowersOnly = int.Parse(followersOnly);
				}

				if (msg.TryGetValue("r9k", out var r9k))
				{
					R9k = int.Parse(r9k) == 1;
				}

				if (msg.TryGetValue("slow", out var slowMode))
				{
					SlowMode = int.Parse(slowMode);
				}

				if (msg.TryGetValue("subs-only", out var subsOnly))
				{
					SubOnly = int.Parse(subsOnly) == 1;
				}
			}
		}
	}
}