using System;
using System.Collections.Generic;
using Twitchie2.Messages;

namespace Twitchie2.Events
{
	public class UserStateEventArgs : EventArgs
	{
		public string BadgeInfo { get; }
		public List<TwitchBadge> Badges { get; } = new List<TwitchBadge>();
		public string Color { get; }
		public string DisplayName { get; }
		public int EmoteSets { get; }
		public bool Mod { get; }
		public TwitchIrcChannel Channel { get; }

		public UserStateEventArgs(TwitchIrcMessage message)
		{
			var arg = message.PopDictionaryArgument();

			BadgeInfo = arg.GetValue<string>("badge-info");

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
			EmoteSets = arg.GetValue<int>("emote-sets");
			Mod = arg.GetValue<int>("mod") == 1;

			//:tmi.twitch.tv USERSTATE
			message.SkipArguments(2);

			var channel = message.GetRemainingMessage();
			Channel = Twitchie.Instance.Channels.Find(x => x.Name == channel);
		}
	}
}
