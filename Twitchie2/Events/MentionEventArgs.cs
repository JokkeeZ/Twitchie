namespace Twitchie2.Events
{
	public class MentionEventArgs : MessageEventArgs
	{
		public MentionEventArgs(string message) : base(message)
		{
		}
	}
}