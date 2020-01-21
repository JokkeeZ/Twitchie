using Twitchie2.Messages;

namespace Twitchie2.Events
{
	public class RoomStateEventArgs : TwitchieEventArgs
	{
		public bool EmoteOnly { get; }
		public bool FollowersOnly { get; }
		public bool R9k { get; }
		public int SlowMode { get; }
		public bool SubOnly { get; }

		public RoomStateEventArgs(Twitchie twitchie, TwitchIrcMessage message) : base(twitchie)
		{
			var arg = message.PopDictionaryArgument();

			EmoteOnly = arg.GetValue<int>("emote-only") == 1;
			FollowersOnly = arg.GetValue<int>("followers-only") == 1;
			R9k = arg.GetValue<int>("r9k") == 1;
			SlowMode = arg.GetValue<int>("slow");
			SubOnly = arg.GetValue<int>("subs-only") == 1;

			// :tmi.twitch.tv ROOMSTATE
			message.SkipArguments(2);

			Channel = message.GetRemainingMessage();
		}
	}
}