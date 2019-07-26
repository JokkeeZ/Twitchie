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

		public MessageEventArgs(string message)
		{
			foreach (var part in message.Split(';'))
			{
				if (part.Contains("!"))
				{
					Channel = part.Split('#')[1].Split(' ')[0];
					continue;
				}

				if (part.Contains("badge-info="))
				{
					BadgeInfo = part.Split('=')[1];
					continue;
				}

				if (part.Contains("badges="))
				{
					var badges = part.Split('=')[1];
					// There is no badges
					if (!badges.Contains("/"))
					{
						continue;
					}

					if (!badges.Contains(","))
					{
						Badges.Add(new TwitchBadge
						{
							Badge = badges.Split('/')[0],
							Version = badges.Split('/')[1]
						});

						continue;
					}

					foreach (var badge in badges.Split(','))
					{
						Badges.Add(new TwitchBadge
						{
							Badge = badge.Split('/')[0],
							Version = badge.Split('/')[1]
						});
					}

					continue;
				}

				if (part.Contains("bits="))
				{
					Bits = part.Split('=')[1];
					continue;
				}

				if (part.Contains("color="))
				{
					Color = part.Split('=')[1];
					continue;
				}

				if (part.Contains("display-name="))
				{
					DisplayName = part.Split('=')[1];
					continue;
				}

				if (part.Contains("emotes="))
				{
					Emotes = part.Split('=')[1];
					continue;
				}

				if (part.Contains("id="))
				{
					MessageId = part.Split('=')[1];
					continue;
				}

				if (part.Contains("mod="))
				{
					Mod = int.Parse(part.Split('=')[1]) == 1;
					continue;
				}

				if (part.Contains("room-id="))
				{
					RoomId = part.Split('=')[1];
					continue;
				}

				if (part.Contains("user-id="))
				{
					UserId = part.Split('=')[1];
					continue;
				}
			}

			Message = message.Split(new string[] { $" PRIVMSG #{Channel} :" }, StringSplitOptions.None)[1];
		}
	}
}