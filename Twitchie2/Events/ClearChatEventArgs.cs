using Twitchie2.Messages;

namespace Twitchie2.Events
{
	public class ClearChatEventArgs
	{
		public int BanDuration { get; }
		public int RoomId { get; }
		public int TargetUserId { get; }
		public string TargetUsername { get; }
		public TwitchIrcChannel Channel { get; }

		public ClearChatEventArgs(TwitchIrcMessage message)
		{
			var arg = message.PopDictionaryArgument();

			BanDuration = arg.GetValue<int>("ban-duration");
			RoomId = arg.GetValue<int>("room-id");
			TargetUserId = arg.GetValue<int>("target-user-id");

			//:tmi.twitch.tv CLEARCHAT
			message.SkipArguments(2);

			var channel = message.PopArgument();
			Channel = Twitchie.Instance.Channels.Find(x => x.Name == channel);
			TargetUsername = message.GetRemainingMessage(true);
		}
	}
}