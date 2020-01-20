using System;

namespace Twitchie2.Events
{
	public class ClearChatEventArgs : EventArgs
	{
		public int RoomId { get; }
		public string Channel { get; }
		public string TimeoutUsername { get; }
		public int BanDuration { get; }
		public int TargetUserId { get; }

		public ClearChatEventArgs(string message)
		{
			var msg = KeyValueMessage.Parse(message);

			if (msg.TryGetIntValue("room-id", out var roomId))
				RoomId = roomId;

			if (msg.TryGetIntValue("ban-duration", out var duration))
				BanDuration = duration;

			if (msg.TryGetIntValue("target-user-id", out var userId))
				TargetUserId = userId;

			var split = message.Split(' ');
			Channel = split[3];

			if (split.Length > 3)
				TimeoutUsername = split[4].Substring(1);
		}
	}
}