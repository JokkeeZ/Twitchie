using System;
using System.Collections.Generic;

namespace Twitchie2.Events
{
	public class MessageEventArgs : EventArgs
	{
		public string BadgeInfo { get; }
		public List<TwitchBadge> Badges { get; } = new List<TwitchBadge>();
		public string Bits { get; }
		public string Color { get; }
		public string DisplayName { get; }
		public string Emotes { get; }
		public string MessageId { get; }
		public string Message { get; }
		public bool Mod { get; }
		public string RoomId { get; }
		public string UserId { get; }
		public string Channel { get; }
		public string RawMessage { get; }

		public MessageEventArgs(string message)
		{
			RawMessage = message;

			var msg = KeyValueMessage.Parse(message);

			if (msg.TryGetValue("badge-info", out var badgeInfo))
			{
				BadgeInfo = badgeInfo;
			}

			if (msg.TryGetValue("badges", out var badges))
			{
				if (badges.Contains("/"))
				{
					if (!badges.Contains(","))
					{
						Badges.Add(new TwitchBadge
						{
							Badge = badges.Split('/')[0],
							Version = badges.Split('/')[1]
						});
					}

					foreach (var badge in badges.Split(','))
					{
						Badges.Add(new TwitchBadge
						{
							Badge = badge.Split('/')[0],
							Version = badge.Split('/')[1]
						});
					}
				}
			}

			if (msg.TryGetValue("bits", out var bits))
			{
				Bits = bits;
			}

			if (msg.TryGetValue("color", out var color))
			{
				Color = color;
			}

			if (msg.TryGetValue("display-name", out var displayName))
			{
				DisplayName = displayName;
			}

			if (msg.TryGetValue("emotes", out var emotes))
			{
				Emotes = emotes;
			}

			if (msg.TryGetValue("id", out var id))
			{
				MessageId = id;
			}

			if (msg.TryGetIntValue("mod", out var mod))
			{
				Mod = mod == 1;
			}

			if (msg.TryGetValue("room-id", out var roomId))
			{
				RoomId = roomId;
			}

			if (msg.TryGetValue("user-id", out var userId))
			{
				UserId = userId;
			}

			Channel = message.Split(' ')[3];
			Message = message.Split(new string[] { $" PRIVMSG {Channel} :" }, StringSplitOptions.None)[1];
		}
	}
}