using Twitchie2.Messages;

namespace Twitchie2.Events
{
	public class ClearChatEventArgs : TwitchieEventArgs
	{
		public int BanDuration { get; }
		public int RoomId { get; }
		public int TargetUserId { get; }
		public string TargetUsername { get; }

		public ClearChatEventArgs(Twitchie twitchie, TwitchIrcMessage message) : base(twitchie)
		{
			var arg = message.PopDictionaryArgument();

			BanDuration = arg.GetValue<int>("ban-duration");
			RoomId = arg.GetValue<int>("room-id");
			TargetUserId = arg.GetValue<int>("target-user-id");

			//:tmi.twitch.tv CLEARCHAT
			message.SkipArguments(2);

			Channel = message.PopArgument();
			TargetUsername = message.GetRemainingMessage(true);
		}
	}
}