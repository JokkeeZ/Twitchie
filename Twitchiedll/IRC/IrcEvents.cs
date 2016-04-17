using Twitchiedll.IRC.Events;

namespace Twitchiedll.IRC
{
    public class IrcEvents
    {
        public delegate void NewRawMessageHandler(string RawMessage);

        public delegate void NewMessageHandler(MessageEventArgs e);

        public delegate void PingHandler(string RawMessage);

        public delegate void RoomStateHandler(RoomStateEventArgs e);

        public delegate void ModeHandler(ModeEventArgs e);

        public delegate void NamesHandler(NamesEventArgs e);

        public delegate void JoinHandler(JoinEventArgs e);

        public delegate void PartHandler(PartEventArgs e);

        public delegate void NoticeHandler(NoticeEventArgs e);

        public delegate void SubscriberHandler(SubscriberEventArgs e);

        public delegate void HostTargetHandler(HostTargetEventArgs e);

        public delegate void ClearChatHandler(ClearChatEventArgs e);
    }
}