using System.IO;
using System.Text;

namespace Twitchiedll.IRC
{
    public class MessageHandler
    {
        protected TextWriter Writer;

        public MessageHandler(TextWriter writer)
        {
            Writer = writer;
        }

        public void WriteRawMessage(string RawMessage)
        {
            Writer.Write(RawMessage + "\r\n");
            Writer.Flush();
        }

        public void WriteRawMessage(object[] RawMessage)
        {
            StringBuilder builder = new StringBuilder();

            foreach (var data in RawMessage)
                builder.Append(data + "\r\n");

            WriteRawMessage(builder.ToString());
        }

        public void SendMessage(MessageType MessageType, string Channel, string Message)
        {
            switch (MessageType)
            {
                case MessageType.ACTION:
                    WriteRawMessage($"PRIVMSG {Channel} :/me {Message}");
                    break;

                case MessageType.MESSAGE:
                    WriteRawMessage($"PRIVMSG {Channel} :{Message}");
                    break;
            }
        }
            
    }
}