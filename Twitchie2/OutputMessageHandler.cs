using System;
using System.IO;
using System.Net.Sockets;

namespace Twitchie2
{
	public abstract class OutputMessageHandler : IDisposable
	{
		protected TextWriter writer;

		internal void InitializeStream(NetworkStream stream) => writer = new StreamWriter(stream);

		internal void WriteIrcMessage(string message)
		{
			writer.Write($"{message}\r\n");
			writer.Flush();
		}

		internal void RequestCapabilities(string capabilities)
			=> WriteIrcMessage($"CAP REQ :twitch.tv/{capabilities}");

		public void SendMessage(string channel, string message)
			=> WriteIrcMessage($"PRIVMSG {channel} :{message}");

		public void SendAction(string channel, string message)
			=> SendMessage(channel, $"/me {message}");

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