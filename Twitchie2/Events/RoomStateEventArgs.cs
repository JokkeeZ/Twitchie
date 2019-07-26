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

				if (msg.TryGetIntValue("emote-only", out var emoteOnly))
				{
					EmoteOnly = emoteOnly == 1;
				}

				if (msg.TryGetIntValue("followers-only", out var followersOnly))
				{
					FollowersOnly = followersOnly;
				}

				if (msg.TryGetIntValue("r9k", out var r9k))
				{
					R9k = r9k == 1;
				}

				if (msg.TryGetIntValue("slow", out var slowMode))
				{
					SlowMode = slowMode;
				}

				if (msg.TryGetIntValue("subs-only", out var subsOnly))
				{
					SubOnly = subsOnly == 1;
				}
			}
		}
	}
}