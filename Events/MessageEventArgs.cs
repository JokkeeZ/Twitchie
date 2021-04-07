using System;
using System.Collections.Generic;
using Twitchie2.Messages;

namespace Twitchie2.Events
{
	public class MessageEventArgs : EventArgs
	{
		public string BadgeInfo { get; }
		public List<TwitchBadge> Badges { get; }
		public string Color { get; }
		public string DisplayName { get; }
		public string Username { get; }
		public string Emotes { get; }
		public string Flags { get; }
		public string Id { get; }
		public string Message { get; }
		public bool Mod { get; }
		public string MessageId { get; set; }
		public int RoomId { get; }
		public int UserId { get; }
		public string RawMessage { get; }
		public string Timestamp { get; }
		public TwitchIrcChannel Channel { get; }

		public MessageEventArgs(TwitchIrcMessage message)
		{
			RawMessage = message.Content;

			var arg = message.PopDictionaryArgument();

			BadgeInfo = arg.GetValue<string>("badge-info");

			Badges = arg.PopBadges();

			Color = arg.GetValue<string>("color");
			DisplayName = arg.GetValue<string>("display-name");
			Emotes = arg.GetValue<string>("emotes");
			Flags = arg.GetValue<string>("flags");
			Id = arg.GetValue<string>("id");
			Mod = arg.GetValue<int>("mod") == 1;
			MessageId = arg.GetValue<string>("msg-id");
			RoomId = arg.GetValue<int>("room-id");
			Timestamp = arg.GetValue<string>("tmi-sent-ts");
			UserId = arg.GetValue<int>("user-id");

			// :<user>!<user>@<user>.tmi.twitch.tv 
			Username = message.PopUserHostArgument().username;

			// PRIVMSG
			message.SkipArguments(1);

			Channel = Twitchie.Instance.Channels.Find(x => x.Name == message.PopArgument());

			Message = message.GetRemainingMessage(true);
		}
	}
}
