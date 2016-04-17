using System.IO;
using System.Linq;
using System.Net.Sockets;

using Twitchiedll.IRC.Exceptions;
using Twitchiedll.IRC.Events;
using Twitchiedll.Utils;

namespace Twitchiedll.IRC
{
    public class Twitchie
    {
        private string Nick, Buffer;
        private string[] Channels;

        private TcpClient ClientSocket;
        private TextReader Input;
        private TextWriter Output;

        private MessageHandler MessageHandler;
        private NamesEventArgs NamesEventArgs = new NamesEventArgs();

        public event IrcEvents.NewRawMessageHandler OnRawMessage;
        public event IrcEvents.NewMessageHandler OnMessage;
        public event IrcEvents.PingHandler OnPing;
        public event IrcEvents.RoomStateHandler OnRoomState;
        public event IrcEvents.ModeHandler OnMode;
        public event IrcEvents.NamesHandler OnNames;
        public event IrcEvents.JoinHandler OnJoin;
        public event IrcEvents.PartHandler OnPart;
        public event IrcEvents.NoticeHandler OnNotice;
        public event IrcEvents.SubscriberHandler OnSubscribe;
        public event IrcEvents.HostTargetHandler OnHostTarget;
        public event IrcEvents.ClearChatHandler OnClearChat;

        public bool IsConnected => ClientSocket.Connected;

        public bool Connect(string server, int port)
        {
            ClientSocket = new TcpClient();
            ClientSocket.Connect(server, port);

            if (!IsConnected)
                return false;

            Input = new StreamReader(ClientSocket.GetStream());
            Output = new StreamWriter(ClientSocket.GetStream());
            MessageHandler = new MessageHandler(Output);

            return true;
        }

        public void Login(string Nick, string[] Channels, string Password)
        {
            this.Nick = Nick.ToLower();
            this.Channels = Channels;

            try
            {
                MessageHandler.WriteRawMessage(new string[]
                {
                    $"USER {Nick}",
                    $"PASS {Password}",
                    $"NICK {Nick}"
                });

                MessageHandler.WriteRawMessage(new string[]
                {
                    "CAP REQ :twitch.tv/membership",
                    "CAP REQ :twitch.tv/commands",
                    "CAP REQ :twitch.tv/tags"
                });
            }
            catch (LoginException ex)
            {
                throw new LoginException(ex.Message);
            }
        }

        public bool Listen()
        {
            while ((Buffer = Input.ReadLine()) != null)
            {
                if (Buffer != null)
                {
                    HandleEvents();

                    if (Buffer[0] != ':')
                        continue;

                    if (Buffer.Split(' ')[1] == "001")
                        foreach (var Channel in Channels)
                            GetMessageHandler().WriteRawMessage($"JOIN {Channel}");
                }
            }
            DisconnectFromAll();
            return false;
        }

        public void Disconnect(string Channel)
        {
            GetMessageHandler().WriteRawMessage($"PART {Channel}");
        }

        public void DisconnectFromAll()
        {
            foreach (var channel in Channels)
                Disconnect(channel);
        }

        public MessageHandler GetMessageHandler() => MessageHandler;

        private void HandleEvents()
        {
            NewRawMessage(Buffer);

            if (Buffer.Contains("PRIVMSG"))
                NewMessage(new MessageEventArgs(Buffer));

            if (Buffer.StartsWith(":twitchnotify!twitchnotify@twitchnotify.tmi.twitch.tv"))
                NewSubscribe(new SubscriberEventArgs(Buffer));

            if (Utilities.BufferElementEquals(Buffer, 0, "PING"))
                NewPing(Buffer);

            if (Buffer.Contains("ROOMSTATE") && !Utilities.IsPrivMsg(Buffer))
                NewRoomState(new RoomStateEventArgs(Buffer));

            if (Utilities.BufferElementEquals(Buffer, 1, "MODE"))
                NewMode(new ModeEventArgs(Buffer));

            if (Utilities.BufferElementEquals(Buffer, 1, "353"))
                NamesEventArgs.Names.AddRange(Buffer.Split(':').Last().Split(' '));

            if (Utilities.BufferElementEquals(Buffer, 1, "366"))
                NewNames(NamesEventArgs);

            if (Utilities.BufferElementEquals(Buffer, 1, "JOIN"))
                NewJoin(new JoinEventArgs(Buffer));

            if (Utilities.BufferElementEquals(Buffer, 1, "PART"))
                NewPart(new PartEventArgs(Buffer));

            if (Buffer.Contains("NOTICE") && !Utilities.IsPrivMsg(Buffer))
                NewNotice(new NoticeEventArgs(Buffer));

            if (Buffer.Contains("HOSTTARGET") && !Utilities.IsPrivMsg(Buffer))
                NewHostTarget(new HostTargetEventArgs(Buffer));

            if (Buffer.Contains("CLEARCHAT") && !Utilities.IsPrivMsg(Buffer))
                NewClearChat(new ClearChatEventArgs(Buffer));
        }

        protected virtual void NewRawMessage(string RawMessage)
            => OnRawMessage?.Invoke(RawMessage);

        protected virtual void NewMessage(MessageEventArgs e)
            => OnMessage?.Invoke(e);

        protected virtual void NewPing(string RawMessage)
            => OnPing?.Invoke(RawMessage);

        protected virtual void NewRoomState(RoomStateEventArgs e) 
            => OnRoomState?.Invoke(e);

        protected virtual void NewMode(ModeEventArgs e) 
            => OnMode?.Invoke(e);

        protected virtual void NewNames(NamesEventArgs e) 
            => OnNames?.Invoke(e);

        protected virtual void NewJoin(JoinEventArgs e) 
            => OnJoin?.Invoke(e);

        protected virtual void NewPart(PartEventArgs e) 
            => OnPart?.Invoke(e);

        protected virtual void NewNotice(NoticeEventArgs e) 
            => OnNotice?.Invoke(e);

        protected virtual void NewSubscribe(SubscriberEventArgs e) 
            => OnSubscribe?.Invoke(e);

        protected virtual void NewHostTarget(HostTargetEventArgs e) 
            => OnHostTarget?.Invoke(e);

        protected virtual void NewClearChat(ClearChatEventArgs e) 
            => OnClearChat?.Invoke(e);
    }
}