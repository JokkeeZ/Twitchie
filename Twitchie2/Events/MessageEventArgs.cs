using System;
using System.Collections.Generic;

namespace Twitchie2.Events
{
	public class MessageEventArgs : TwitchieEventArgs
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
		public string RawMessage { get; }

		public MessageEventArgs(Twitchie sender, string message) : base(sender)
		{
			RawMessage = message;

			var msg = KeyValueMessage.Parse(message);

			if (msg.TryGetValue("badge-info", out var badgeInfo))
				BadgeInfo = badgeInfo;

			if (msg.TryGetValue("badges", out var badges) && (badges.Contains("/")))
			{
				if (!badges.Contains(","))
				{
					var split = badges.Split('/');
					Badges.Add(new TwitchBadge(split[0], split[1]));
				}

				foreach (var badge in badges.Split(','))
				{
					var split = badge.Split('/');
					Badges.Add(new TwitchBadge(split[0], split[1]));
				}
			}

			if (msg.TryGetValue("bits", out var bits))
				Bits = bits;

			if (msg.TryGetValue("color", out var color))
				Color = color;

			if (msg.TryGetValue("display-name", out var displayName))
				DisplayName = displayName;

			if (msg.TryGetValue("emotes", out var emotes))
				Emotes = emotes;

			if (msg.TryGetValue("id", out var id))
				MessageId = id;

			if (msg.TryGetIntValue("mod", out var mod))
				Mod = mod == 1;

			if (msg.TryGetValue("room-id", out var roomId))
				RoomId = roomId;

			if (msg.TryGetValue("user-id", out var userId))
				UserId = userId;

			Channel = message.Split(' ')[3];
			Message = message.Split(new[] { $" PRIVMSG {Channel} :" }, StringSplitOptions.None)[1];
		}
	}
}