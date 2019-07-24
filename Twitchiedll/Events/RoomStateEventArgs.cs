namespace Twitchiedll.IRC.Events
{
    public class RoomStateEventArgs
    {
        public bool r9k { get; internal set; }
        public bool SubOnly { get; internal set; }
        public bool SlowMode { get; internal set; }
        public string BroadcasterLanguage { get; internal set; }
        public string Channel { get; internal set; }

        public RoomStateEventArgs(string IrcMessage)
        {
            if (IrcMessage.Split(';').Length > 3)
            {
                if (IrcMessage.Split(';')[0].Split('=').Length > 1)
                    BroadcasterLanguage = IrcMessage.Split(';')[0].Split('=')[1];

                if (IrcMessage.Split(';')[1].Split('=').Length > 1)
                    r9k = ToBoolean(IrcMessage.Split(';')[1].Split('=')[1]);

                if (IrcMessage.Split(';')[2].Split('=').Length > 1)
                    SlowMode = ToBoolean(IrcMessage.Split(';')[2].Split('=')[1]);

                if (IrcMessage.Split(';')[3].Split('=').Length > 1)
                    SubOnly = ToBoolean(IrcMessage.Split(';')[3].Split('=')[1]);

                Channel = "#" + IrcMessage.Split('#')[1];
            }
        }

        private bool ToBoolean(string str)
            => str == "1";
    }
}