using System.Linq;
using Twitchiedll.IRC.Events;

namespace Twitchiedll.IRC
{
    public partial class Twitchie
    {
        public event RawMessageHandler OnRawMessage;
        public event PRIVMessageHandler OnMessage;
        public event PingHandler OnPing;
        public event RoomStateHandler OnRoomState;
        public event ModeHandler OnMode;
        public event NamesHandler OnNames;
        public event JoinHandler OnJoin;
        public event PartHandler OnPart;
        public event NoticeHandler OnNotice;
        public event SubscriberHandler OnSubscribe;
        public event HostTargetHandler OnHostTarget;
        public event ClearChatHandler OnClearChat;

        public void HandleEvent(EventType Event)
        {
            switch (Event)
            {
                case EventType.ON_RAWMESSAGE:
                    OnRawMessage?.Invoke(Buffer);
                    break;

                case EventType.ON_MESSAGE:
                    OnMessage?.Invoke(new MessageEventArgs(Buffer));
                    break;

                case EventType.ON_PING:
                    OnPing?.Invoke(Buffer);
                    break;

                case EventType.ON_ROOMSTATE:
                    OnRoomState?.Invoke(new RoomStateEventArgs(Buffer));
                    break;

                case EventType.ON_MODE:
                    OnMode?.Invoke(new ModeEventArgs(Buffer));
                    break;

                case EventType.ON_NAMES_STARTING:
                    NamesEventArgs.AddRange(Buffer.Split(':').Last().Split(' '));
                    break;

                case EventType.ON_NAMES_ENDING:
                    OnNames?.Invoke(NamesEventArgs);
                    break;

                case EventType.ON_JOIN:
                    OnJoin?.Invoke(new JoinEventArgs(Buffer));
                    break;

                case EventType.ON_PART:
                    OnPart?.Invoke(new PartEventArgs(Buffer));
                    break;

                case EventType.ON_NOTICE:
                    OnNotice?.Invoke(new NoticeEventArgs(Buffer));
                    break;

                case EventType.ON_SUBSCRIBE:
                    OnSubscribe?.Invoke(new SubscriberEventArgs(Buffer));
                    break;

                case EventType.ON_HOSTTARGET:
                    OnHostTarget?.Invoke(new HostTargetEventArgs(Buffer));
                    break;

                case EventType.ON_CLEARCHAT:
                    OnClearChat?.Invoke(new ClearChatEventArgs(Buffer));
                    break;
            }
        }

        private void HandleEvents()
        {
            HandleEvent(EventType.ON_RAWMESSAGE);

            if (Is("PRIVMSG"))
            {
                HandleEvent(EventType.ON_MESSAGE);

                if (Buffer.StartsWith(":twitchnotify!twitchnotify@twitchnotify.tmi.twitch.tv"))
                    HandleEvent(EventType.ON_SUBSCRIBE);
            }
            else
            {
                ParseActions();

                if (Is("PING"))
                    HandleEvent(EventType.ON_PING);

                if (Is("ROOMSTATE"))
                    HandleEvent(EventType.ON_ROOMSTATE);

                if (Is("NOTICE"))
                    HandleEvent(EventType.ON_NOTICE);

                if (Is("HOSTTARGET"))
                    HandleEvent(EventType.ON_HOSTTARGET);

                if (Is("CLEARCHAT"))
                    HandleEvent(EventType.ON_CLEARCHAT);
            }
        }

        public bool Is(string asd)
            => Buffer.Split(' ').Any(a => a.Contains(asd));

        private void ParseActions()
        {
            switch (Buffer.Split(' ')[1].Split(' ')[0])
            {
                case "MODE":
                    HandleEvent(EventType.ON_MODE);
                    break;

                case "353":
                    HandleEvent(EventType.ON_NAMES_STARTING);
                    break;

                case "366":
                    HandleEvent(EventType.ON_NAMES_ENDING);
                    break;

                case "JOIN":
                    HandleEvent(EventType.ON_JOIN);
                    break;

                case "PART":
                    HandleEvent(EventType.ON_PART);
                    break;
            }
        }
    }
}