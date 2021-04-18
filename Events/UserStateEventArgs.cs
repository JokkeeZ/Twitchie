using System;
using System.Collections.Generic;
using Twitchie2.Messages;

namespace Twitchie2.Events
{
	public sealed class UserStateEventArgs : EventArgs
	{
		public string BadgeInfo { get; }
		public List<TwitchBadge> Badges { get; }
		public string Color { get; }
		public string DisplayName { get; }
		public int EmoteSets { get; }
		public bool Mod { get; }
		public TwitchIrcChannel Channel { get; }

		public UserStateEventArgs(TwitchIrcMessage message)
		{
			var arg = message.PopDictionaryArgument();

			BadgeInfo = arg.GetValue<string>("badge-info");

			Badges = arg.PopBadges();

			Color = arg.GetValue<string>("color");
			DisplayName = arg.GetValue<string>("display-name");
			EmoteSets = arg.GetValue<int>("emote-sets");
			Mod = arg.GetValue<int>("mod") == 1;

			//:tmi.twitch.tv USERSTATE
			message.SkipArguments(2);

			Channel = Twitchie.Instance.Channels.Find(x => x.Name == message.GetRemainingMessage());
		}
	}
}
