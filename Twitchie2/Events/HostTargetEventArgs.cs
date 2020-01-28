using System;
using Twitchie2.Messages;

namespace Twitchie2.Events
{
	public class HostTargetEventArgs : EventArgs
	{
		public int Viewers { get; }
		public string TargetChannel { get; }
		public bool IsStarting { get; }
		public TwitchIrcChannel Channel { get; }

		public HostTargetEventArgs(TwitchIrcMessage message)
		{
			message.SkipArguments(2);

			var channel = message.PopArgument();
			Channel = Twitchie.Instance.Channels.Find(x => x.Name == channel);

			var targetChannel = message.PopArgument();
			IsStarting = targetChannel != ":-";
			TargetChannel = IsStarting ? targetChannel.Substring(1) : string.Empty;
			Viewers = message.GetRemainingArgument<int>();
		}
	}
}