using System.IO;
using System.Net.Sockets;
using Twitchiedll.IRC.Events;

namespace Twitchiedll.IRC
{
    public partial class Twitchie
    {
        private string Nickname, Buffer;
        private string[] Channels;

        private TextReader Input;
        private TextWriter Output;
        private MessageHandler MessageHandler;

        private TcpClient ClientSocket = new TcpClient();
        private NamesEventArgs NamesEventArgs = new NamesEventArgs();

        public bool IsConnected => ClientSocket.Connected;

        public void Connect(string Server, int Port)
        {
            try
            {
                ClientSocket.Connect(Server, Port);

                Input = new StreamReader(ClientSocket.GetStream());
                Output = new StreamWriter(ClientSocket.GetStream());
                MessageHandler = new MessageHandler(Output);
            }
            catch { }
        }

        public void Login(string Nick, string[] Channels, string Password)
        {
            Nickname = Nick.ToLower();
            this.Channels = Channels;

            try
            {
                GetMessageHandler().WriteRawMessage(new string[]
                {
                    $"USER {Nick}",
                    $"PASS {Password}",
                    $"NICK {Nick}"
                });

                GetMessageHandler().WriteRawMessage(new string[]
                {
                    "CAP REQ :twitch.tv/membership",
                    "CAP REQ :twitch.tv/commands",
                    "CAP REQ :twitch.tv/tags"
                });
            }
            catch (IOException ex)
            {
                throw ex;
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
                        foreach (string channel in Channels)
                            Join(channel);
                }
            }
            return false;
        }

        public void Join(string Channel)
            => GetMessageHandler().WriteRawMessage($"JOIN {Channel}");

        public void Disconnect(string Channel)
            => GetMessageHandler().WriteRawMessage($"PART {Channel}");

        public void DisconnectFromAll()
        {
            foreach (string channel in Channels)
                Disconnect(channel);
        }

        public MessageHandler GetMessageHandler() 
            => MessageHandler;
    }
}