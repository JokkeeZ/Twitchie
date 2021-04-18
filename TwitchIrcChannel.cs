using System;
using System.Threading.Tasks;

namespace Twitchie2
{
	public class TwitchIrcChannel
	{
		public string Name { get; }

		public bool Joined { get; private set; }

		public TwitchIrcChannel(string channel)
		{
			if (string.IsNullOrWhiteSpace(channel))
				throw new ArgumentException("Invalid channel! Channel can not be null or whitespace.");

			Name = channel[0] == '#' ? channel : '#' + channel;
		}

		public async Task SendMessageAsync(string message)
			=> await Twitchie.Instance.ChatAsync(Name, message);

		public async Task SendActionAsync(string action)
			=> await Twitchie.Instance.ActionAsync(Name, action);

		public async Task SendMentionAsync(string user, string message)
			=> await Twitchie.Instance.MentionAsync(Name, user, message);

		public async Task SendWhisperAsync(string user, string message)
			=> await Twitchie.Instance.WhisperAsync(Name, user, message);

		public async Task JoinAsync()
		{
			if (!Joined)
			{
				await Twitchie.Instance.SendAsync($"JOIN {Name}");
				Joined = true;
			}
		}

		public async Task PartAsync()
		{
			if (Joined)
			{
				await Twitchie.Instance.SendAsync($"PART {Name}");
				Joined = false;
			}
		}
	}
}
