using System;
using System.Threading.Tasks;

namespace Twitchie2
{
	public class TwitchIrcChannel
	{
		public string Name { get; }

		public bool Joined { get; private set; }

		public Twitchie Instance => Twitchie.Instance;

		public TwitchIrcChannel(string channel)
		{
			if (string.IsNullOrWhiteSpace(channel))
				throw new ArgumentException("Invalid channel! Channel can not be null or whitespace.");

			Name = channel[0] == '#' ? channel : '#' + channel;
		}

		public async Task SendMessageAsync(string message)
			=> await Instance.ChatAsync(Name, message);

		public async Task SendActionAsync(string action)
			=> await Instance.ActionAsync(Name, action);

		public async Task SendMentionAsync(string user, string message)
			=> await Instance.MentionAsync(Name, user, message);

		public async Task SendWhisperAsync(string user, string message)
			=> await Instance.WhisperAsync(Name, user, message);

		public async Task JoinAsync()
		{
			if (!Joined)
			{
				await Instance.SendAsync($"JOIN {Name}");
				Joined = true;
			}
		}

		public async Task PartAsync()
		{
			if (Joined)
			{
				await Instance.SendAsync($"PART {Name}");
				Joined = false;
			}
		}
	}
}
