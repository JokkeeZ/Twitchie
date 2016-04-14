namespace Twitchiedll.IRC.Events
{
    public class JoinEventArgs
    {
        public string Username { get; internal set; }
        public string Channel { get; internal set; }

        public JoinEventArgs(string IrcMessage)
        {
            Username = IrcMessage.Split(' ')[0].Split(':')[1].Split('!')[0];
            Channel = IrcMessage.Split(' ')[2];
        }
    }
}