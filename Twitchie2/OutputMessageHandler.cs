using System;
using System.IO;

namespace Twitchie2
{
	public abstract class OutputMessageHandler : IDisposable
	{
		protected TextWriter writer;

		public void InitializeStream(StreamWriter w) => writer = w;

		public void WriteRawMessage(string message)
		{
			writer.Write($"{message}\r\n");
			writer.Flush();
		}

		public void SendMessage(string channel, string message)
			=> WriteRawMessage($"PRIVMSG {channel} :{message}");

		public void SendAction(string channel, string message)
			=> WriteRawMessage($"PRIVMSG {channel} :/me {message}");

		public void SendMention(string channel, string user, string message)
			=> SendMessage(channel, $"@{user}, {message}");

		public void SendWhisper(string channel, string receiver, string message)
			=> SendMessage(channel, $"/w {receiver} {message}");

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
				writer?.Dispose();
		}
	}
}