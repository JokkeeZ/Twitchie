namespace Twitchiedll.IRC.Events
{
    public class ModeEventArgs
    {
        public bool AddingMode { get; internal set; }
        public string Username { get; internal set; }
        public string Channel { get; internal set; }

        public ModeEventArgs(string IrcMessage)
        {
            string[] SplittedMessage = IrcMessage.Split(' ');

            if (SplittedMessage[2].StartsWith("#"))
                Channel = SplittedMessage[2];

            if (SplittedMessage[3].Equals("+o"))
                AddingMode = true;
            else
                AddingMode = false;

            Username = SplittedMessage[4];
        }
    }
}