using System;
using Twitchie2.Messages;

namespace Twitchie2.Events
{
	public class ClearChatEventArgs : EventArgs
	{
		public int BanDuration { get; }
		public int RoomId { get; }
		public int TargetUserId { get; }
		public string TargetUsername { get; }
		public string Timestamp { get; }
		public TwitchIrcChannel Channel { get; }

		public ClearChatEventArgs(TwitchIrcMessage message)
		{
			var arg = message.PopDictionaryArgument();

			BanDuration = arg.GetValue<int>("ban-duration");
			RoomId = arg.GetValue<int>("room-id");
			TargetUserId = arg.GetValue<int>("target-user-id");
			Timestamp = arg.GetValue<string>("tmi-sent-ts");

			//:tmi.twitch.tv CLEARCHAT
			message.SkipArguments(2);

			Channel = Twitchie.Instance.Channels.Find(x => x.Name == message.PopArgument());
			TargetUsername = message.GetRemainingMessage(true);
		}
	}
}
