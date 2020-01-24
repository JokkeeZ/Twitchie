using System.Collections.Generic;
using Twitchie2.Messages;

namespace Twitchie2.Events
{
	public class MessageEventArgs : TwitchieEventArgs
	{
		public string BadgeInfo { get; }
		public List<TwitchBadge> Badges { get; } = new List<TwitchBadge>();
		public string Bits { get; }
		public string Color { get; }
		public string DisplayName { get; }
		public string Username { get; }
		public string Emotes { get; }
		public string MessageId { get; }
		public string Message { get; }
		public bool Mod { get; }
		public int RoomId { get; }
		public int UserId { get; }
		public string RawMessage { get; }
		public string Timestamp { get; }

		public MessageEventArgs(Twitchie sender, TwitchIrcMessage message) : base(sender)
		{
			RawMessage = message.Content;

			var arg = message.PopDictionaryArgument();

			BadgeInfo = arg.GetValue<string>("badge-info");

			//<badge>/<version>,<badge>/<version>
			var badges = arg.GetValue<string>("badges");
			if (!string.IsNullOrWhiteSpace(badges))
			{
				foreach (var badge in badges.Split(','))
				{
					var split = badge.Split('/');
					Badges.Add(new TwitchBadge(split[0], split[1]));
				}
			}

			Color = arg.GetValue<string>("color");
			DisplayName = arg.GetValue<string>("display-name");
			Emotes = arg.GetValue<string>("emotes");
			MessageId = arg.GetValue<string>("id");
			Mod = arg.GetValue<int>("mod") == 1;
			RoomId = arg.GetValue<int>("room-id");
			Timestamp = arg.GetValue<string>("tmi-sent-ts");
			UserId = arg.GetValue<int>("user-id");

			// :<user>!<user>@<user>.tmi.twitch.tv 
			Username = message.PopUserHostArgument().username;

			// PRIVMSG
			message.SkipArguments(1);

			Channel = message.PopArgument();
			Message = message.GetRemainingMessage(true);
		}
	}
}