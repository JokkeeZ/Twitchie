namespace Twitchie2
{
	public class TwitchBadge
	{
		public string Name { get; set; }

		public string Version { get; set; }

		public TwitchBadge() { }

		public TwitchBadge(string badge) => Name = badge;

		public TwitchBadge(string badge, string version)
		{
			Name = badge;
			Version = version;
		}
	}
}