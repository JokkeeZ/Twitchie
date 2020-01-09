﻿using System;
using System.IO;

namespace Twitchie2
{
	public class OutputMessageHandler : IDisposable
	{
		private readonly TextWriter writer;

		public OutputMessageHandler(TextWriter writer)
		{
			this.writer = writer;
		}

		public void WriteRawMessage(string message)
		{
			writer.Write($"{message}\r\n");
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

		public void Whisper(string channel, string receiver, string message)
		{
			SendMessage(MessageType.Message, channel, $"/w {receiver} {message}");
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				writer?.Dispose();
			}
		}
	}
}