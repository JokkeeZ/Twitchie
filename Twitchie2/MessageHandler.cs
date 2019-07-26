using System.IO;

namespace Twitchie2
{
	public class MessageHandler
	{
		private readonly TextWriter writer;

		public MessageHandler(TextWriter writer)
		{
			this.writer = writer;
		}

		public void WriteRawMessage(string message)
		{
			writer.Write(message + "\r\n");
			writer.Flush();
		}

		public void SendMessage(MessageType messageType, string channel, string message)
		{
			switch (messageType)
			{
				case MessageType.Action:
					WriteRawMessage($"PRIVMSG {channel} :/me {message}");
					break;

				case MessageType.Message:
					WriteRawMessage($"PRIVMSG {channel} :{message}");
					break;
			}
		}

	}
}