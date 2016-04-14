using System.Linq;
using Twitchiedll.Utils;

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
            if (IrcMessage.Split(';').Count() > 3)
            {
                if (IrcMessage.Split(';')[0].Split('=').Count() > 1)
                    BroadcasterLanguage = IrcMessage.Split(';')[0].Split('=')[1];

                if (IrcMessage.Split(';')[1].Split('=').Count() > 1)
                    r9k = Utilities.ConvertToBoolean(IrcMessage.Split(';')[1].Split('=')[1]);

                if (IrcMessage.Split(';')[2].Split('=').Count() > 1)
                    SlowMode = Utilities.ConvertToBoolean(IrcMessage.Split(';')[2].Split('=')[1]);

                if (IrcMessage.Split(';')[3].Split('=').Count() > 1)
                    SubOnly = Utilities.ConvertToBoolean(IrcMessage.Split(';')[3].Split('=')[1]);

                Channel = "#" + IrcMessage.Split('#')[1];
            }
        }
    }
}