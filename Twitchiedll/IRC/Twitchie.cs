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
        #region Fields
        private string Server, Nick, Username, Buffer;
        private string[] Channels;

        private TcpClient ClientSocket;
        private TextReader Input;
        private TextWriter Output;

        private MessageHandler MessageHandler;
        private NamesEventArgs NamesEventArgs = new NamesEventArgs();

        public event IrcEvents.NewRawMessageHandler OnRawMessage;
        public event IrcEvents.NewMessageHandler OnMessage;
        public event IrcEvents.PingHandler OnPing;
        public event IrcEvents.DisconnectHandler OnDisconnect;
        public event IrcEvents.RoomStateHandler OnRoomState;
        public event IrcEvents.ModeHandler OnMode;
        public event IrcEvents.NamesHandler OnNames;
        public event IrcEvents.JoinHandler OnJoin;
        public event IrcEvents.PartHandler OnPart;
        public event IrcEvents.NoticeHandler OnNotice;
        public event IrcEvents.SubscriberHandler OnSubscribe;

        public bool IsConnected => ClientSocket.Connected;
        #endregion

        #region Methods
        public bool Connect(string server, int port)
        {
            Server = server;

            ClientSocket = new TcpClient();
            ClientSocket.Connect(Server, port);

            if (!IsConnected)
                return false;

            Input = new StreamReader(ClientSocket.GetStream());
            Output = new StreamWriter(ClientSocket.GetStream());
            MessageHandler = new MessageHandler(Output);

            return true;
        }

        public void Login(string Nick, string Username, string[] Channels, string Password)
        {
            this.Nick = Nick.ToLower();
            this.Username = Username.ToLower();
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
                            MessageHandler.WriteRawMessage($"JOIN {Channel}");
                }
            }
            DisconnectFromAll();
            return false;
        }

        public void Disconnect(string Channel)
        {
            MessageHandler.WriteRawMessage($"PART {Channel}");
            NewDisconnect(new DisconnectEventArgs() { Channel = Channel, User = Nick });
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

            if (Buffer.Contains("ROOMSTATE") && !Buffer.Contains("PRIVMSG"))
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

            if (Buffer.Contains("NOTICE") && !Buffer.Contains("PRIVMSG"))
                NewNotice(new NoticeEventArgs(Buffer));
        }

        protected virtual void NewRawMessage(string RawMessage)
        {
            if (OnRawMessage != null)
                OnRawMessage(RawMessage);
        }

        protected virtual void NewMessage(MessageEventArgs e)
        {
            if (OnMessage != null)
                OnMessage(e);
        }

        protected virtual void NewPing(string Buffer)
        {
            if (OnPing != null)
                OnPing(Buffer);
        }

        protected virtual void NewDisconnect(DisconnectEventArgs e)
        {
            if (OnDisconnect != null)
                OnDisconnect(e);
        }

        protected virtual void NewRoomState(RoomStateEventArgs e)
        {
            if (OnRoomState != null)
                OnRoomState(e);
        }

        protected virtual void NewMode(ModeEventArgs e)
        {
            if (OnMode != null)
                OnMode(e);
        }

        protected virtual void NewNames(NamesEventArgs e)
        {
            if (OnNames != null)
                OnNames(e);
        }

        protected virtual void NewJoin(JoinEventArgs e)
        {
            if (OnJoin != null)
                OnJoin(e);
        }

        protected virtual void NewPart(PartEventArgs e)
        {
            if (OnPart != null)
                OnPart(e);
        }

        protected virtual void NewNotice(NoticeEventArgs e)
        {
            if (OnNotice != null)
                OnNotice(e);
        }

        protected virtual void NewSubscribe(SubscriberEventArgs e)
        {
            if (OnSubscribe != null)
                OnSubscribe(e);
        }
        #endregion
    }
}