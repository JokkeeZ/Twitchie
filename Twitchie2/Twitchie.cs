using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using Twitchie2.Events;

namespace Twitchie2
{
	public partial class Twitchie : IDisposable
	{
		private readonly List<string> channels;

		private TextReader input;
		private TextWriter output;
		private readonly TcpClient tcpClient;

		public bool IsConnected => tcpClient.Connected;

		public MessageHandler MessageHandler { get; private set; }

		internal string Buffer { get; private set; }

		public event EventHandler<RawMessageEventArgs> OnRawMessage;
		public event EventHandler<MessageEventArgs> OnMessage;
		public event EventHandler<RoomStateEventArgs> OnRoomState;
		public event EventHandler<ModeEventArgs> OnMode;
		public event EventHandler<JoinEventArgs> OnJoin;
		public event EventHandler<PartEventArgs> OnPart;
		public event EventHandler<NoticeEventArgs> OnNotice;
		public event EventHandler<HostTargetEventArgs> OnHostTarget;
		public event EventHandler<ClearChatEventArgs> OnClearChat;

		public Twitchie()
		{
			channels = new List<string>();
			tcpClient = new TcpClient();
		}

		public void Connect()
		{
			try
			{
				tcpClient.Connect("irc.chat.twitch.tv", 6667);

				input = new StreamReader(tcpClient.GetStream());
				output = new StreamWriter(tcpClient.GetStream());
				MessageHandler = new MessageHandler(output);
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
			this.channels.AddRange(channels);
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

		public async Task<bool> ListenAsync()
		{
			while ((Buffer = await input.ReadLineAsync()) != null)
			{
				if (Buffer == null)
				{
					return false;
				}

				OnRawMessage?.Invoke(this, new RawMessageEventArgs(Buffer));

				var eventType = EventParser.ParseEventType(Buffer);
				HandleEvent(eventType);

				if (Buffer.Split(' ')[1] == "001" && channels.Count > 0)
				{
					channels.ForEach(channel => JoinChannel(channel));
				}
			}
			return false;
		}

		public void HandleEvent(EventType e)
		{
			switch (e)
			{
				case EventType.ClearChat:
					OnClearChat?.Invoke(this, new ClearChatEventArgs(Buffer));
					break;

				case EventType.HostTarget:
					OnHostTarget?.Invoke(this, new HostTargetEventArgs(Buffer));
					break;

				case EventType.Join:
					OnJoin?.Invoke(this, new JoinEventArgs(Buffer));
					break;

				case EventType.Message:
					OnMessage?.Invoke(this, new MessageEventArgs(Buffer));
					break;

				case EventType.Mode:
					OnMode?.Invoke(this, new ModeEventArgs(Buffer));
					break;

				case EventType.Notice:
					OnNotice?.Invoke(this, new NoticeEventArgs(Buffer));
					break;

				case EventType.Part:
					OnPart?.Invoke(this, new PartEventArgs(Buffer));
					break;

				case EventType.RoomState:
					OnRoomState?.Invoke(this, new RoomStateEventArgs(Buffer));
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

			if (!channels.Contains(channel))
			{
				channels.Add(channel);
			}

			MessageHandler.WriteRawMessage($"JOIN {channel}");
		}

		public void PartChannel(string channel)
		{
			if (!channels.Contains(channel))
			{
				return;
			}

			channels.Remove(channel);
			MessageHandler.WriteRawMessage($"PART {channel}");
		}

		public void PartFromAllChannels()
		{
			channels.ForEach(channel => PartChannel(channel));
			channels.Clear();
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
				input?.Dispose();
				output?.Dispose();
				tcpClient?.Dispose();
			}
		}
	}
}