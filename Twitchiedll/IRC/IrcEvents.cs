using Twitchiedll.IRC.Events;

namespace Twitchiedll.IRC
{
    public class IrcEvents
    {
        public delegate void NewRawMessageHandler(string RawMessage);

        public delegate void NewMessageHandler(MessageEventArgs e);

        public delegate void PingHandler(string Buffer);

        public delegate void DisconnectHandler(DisconnectEventArgs e);

        public delegate void RoomStateHandler(RoomStateEventArgs e);

        public delegate void ModeHandler(ModeEventArgs e);

        public delegate void NamesHandler(NamesEventArgs e);

        public delegate void JoinHandler(JoinEventArgs e);

        public delegate void PartHandler(PartEventArgs e);

        public delegate void NoticeHandler(NoticeEventArgs e);

        public delegate void SubscriberHandler(SubscriberEventArgs e);
    }
}