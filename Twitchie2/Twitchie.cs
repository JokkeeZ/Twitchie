using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Twitchie2.Events;

namespace Twitchie2
{
	public class Twitchie : IDisposable
	{
		private CancellationTokenSource cts;
		private TcpClient tcpClient;

		public List<string> Channels { get; }
		public OutputMessageHandler MessageHandler { get; private set; }

		public event EventHandler<RawMessageEventArgs> OnRawMessage;
		public event EventHandler<MessageEventArgs> OnMessage;
		public event EventHandler<RoomStateEventArgs> OnRoomState;
		public event EventHandler<ModeEventArgs> OnMode;
		public event EventHandler<JoinEventArgs> OnJoin;
		public event EventHandler<PartEventArgs> OnPart;
		public event EventHandler<NoticeEventArgs> OnNotice;
		public event EventHandler<HostTargetEventArgs> OnHostTarget;
		public event EventHandler<ClearChatEventArgs> OnClearChat;
		public event EventHandler<UserNoticeEventArgs> OnUserNotice;

		public Twitchie()
		{
			Channels = new List<string>();
		}

		public void Connect()
		{
			try
			{
				cts = new CancellationTokenSource();

				tcpClient = new TcpClient();
				tcpClient.Connect("irc.chat.twitch.tv", 6667);

				MessageHandler = new OutputMessageHandler(new StreamWriter(tcpClient.GetStream()));
			}
			catch (SocketException ex)
			{
#if DEBUG
				Console.WriteLine(ex.Message);
#endif
			}
		}

		public void SetDefaultChannels(IEnumerable<string> channels)
		{
			Channels.AddRange(channels);
		}

		public void Login(string nickname, string password)
		{
			try
			{
				MessageHandler.WriteRawMessage($"PASS {password}");
				MessageHandler.WriteRawMessage($"NICK {nickname.ToLower()}");

				MessageHandler.WriteRawMessage("CAP REQ :twitch.tv/membership");
				MessageHandler.WriteRawMessage("CAP REQ :twitch.tv/commands");
				MessageHandler.WriteRawMessage("CAP REQ :twitch.tv/tags");
			}
			catch (IOException ex)
			{
#if DEBUG
				Console.WriteLine(ex.Message);
#endif
			}
		}

		public async Task ListenAsync()
		{
			using (var input = new StreamReader(tcpClient.GetStream()))
			{
				while (!cts.IsCancellationRequested)
				{
					var buffer = await input?.ReadLineAsync();
					if (buffer == null)
					{
						return;
					}

					OnRawMessage?.Invoke(this, new RawMessageEventArgs(buffer));

					var eventType = EventParser.ParseEventType(buffer);
					HandleIrcEvent(eventType, buffer);

					if (buffer.Split(' ')[1] == "001" && Channels.Count > 0)
					{
						Channels.ForEach(channel => JoinChannel(channel));
					}
				}
			}
		}

		public void HandleIrcEvent(EventType eventType, string buffer)
		{
			switch (eventType)
			{
				case EventType.ClearChat:
					OnClearChat?.Invoke(this, new ClearChatEventArgs(buffer));
					break;

				case EventType.HostTarget:
					OnHostTarget?.Invoke(this, new HostTargetEventArgs(buffer));
					break;

				case EventType.Join:
					OnJoin?.Invoke(this, new JoinEventArgs(buffer));
					break;

				case EventType.Message:
					OnMessage?.Invoke(this, new MessageEventArgs(buffer));
					break;

				case EventType.Mode:
					OnMode?.Invoke(this, new ModeEventArgs(buffer));
					break;

				case EventType.Notice:
					OnNotice?.Invoke(this, new NoticeEventArgs(buffer));
					break;

				case EventType.Part:
					OnPart?.Invoke(this, new PartEventArgs(buffer));
					break;

				case EventType.RoomState:
					OnRoomState?.Invoke(this, new RoomStateEventArgs(buffer));
					break;

				case EventType.UserNotice:
					OnUserNotice?.Invoke(this, new UserNoticeEventArgs(buffer));
					break;

				case EventType.Ping:
					MessageHandler.WriteRawMessage("PONG :tmi.twitch.tv");
					break;
			}
		}

		public void JoinChannel(string channel)
		{
			if (channel[0] != '#')
			{
				channel = $"#{channel}";
			}

			if (!Channels.Contains(channel))
			{
				Channels.Add(channel);
			}

			MessageHandler.WriteRawMessage($"JOIN {channel}");
		}

		public void PartChannel(string channel)
		{
			if (!Channels.Contains(channel))
			{
				return;
			}

			Channels.Remove(channel);
			MessageHandler.WriteRawMessage($"PART {channel}");
		}

		public void PartFromAllChannels()
		{
			for (var i = 0; i < Channels.Count; ++i)
			{
				PartChannel(Channels[i]);
			}
		}

		public void Disconnect()
		{
			try
			{
				PartFromAllChannels();
				cts?.Cancel();
			}
			catch (ObjectDisposedException ex)
			{
#if DEBUG
				Console.WriteLine(ex.Message);
#endif
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				cts?.Dispose();
				tcpClient?.Dispose();
				MessageHandler?.Dispose();

				Channels.Clear();
			}
		}
	}
}