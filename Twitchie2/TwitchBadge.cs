namespace Twitchie2
{
	public class TwitchBadge
	{
		public string Name { get; }

		public string Version { get; }

		public TwitchBadge() { }

		public TwitchBadge(string badge, string version)
		{
			Name = badge;
			Version = version;
		}
	}
}