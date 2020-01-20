using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Twitchie2.Events;

namespace Twitchie2
{
	public class Twitchie : OutputMessageHandler
	{
		private CancellationTokenSource cts;
		private TcpClient tcpClient;

		private string nickname;

		public List<string> Channels { get; }

		public string OwnChannel => $"#{nickname}";

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
		public event EventHandler<MentionEventArgs> OnMention;

		public Twitchie() : base() => Channels = new List<string>();

		public async Task ConnectAsync()
		{
			try
			{
				cts = new CancellationTokenSource();
				tcpClient = new TcpClient();

				await tcpClient.ConnectAsync("irc.chat.twitch.tv", 6667);

				InitializeStream(new StreamWriter(tcpClient.GetStream()));
			}
			catch (SocketException ex)
			{
#if DEBUG
				Debug.WriteLine(ex.Message);
#endif
			}
		}

		public void SetDefaultChannels(IEnumerable<string> channels)
		{
			Channels.AddRange(channels);
		}

		public void Login(string nickname, string password)
		{
			this.nickname = nickname.ToLower();

			try
			{
				WriteRawMessage($"PASS {password}");
				WriteRawMessage($"NICK {this.nickname}");

				WriteRawMessage("CAP REQ :twitch.tv/membership");
				WriteRawMessage("CAP REQ :twitch.tv/commands");
				WriteRawMessage("CAP REQ :twitch.tv/tags");

				OnMessage += (sender, args) =>
				{
					if (args.Message.ToLower().Contains($"@{this.nickname}"))
					{
						OnMention?.Invoke(this, new MentionEventArgs(args.RawMessage));
					}
				};
			}
			catch (IOException ex)
			{
#if DEBUG
				Debug.WriteLine(ex.Message);
#endif
			}
		}

		public async Task ListenAsync()
		{
			using var input = new StreamReader(tcpClient.GetStream());

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

		private void HandleIrcEvent(EventType eventType, string buffer)
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
					WriteRawMessage("PONG :tmi.twitch.tv");
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

			WriteRawMessage($"JOIN {channel}");
		}

		public void PartChannel(string channel)
		{
			if (!Channels.Contains(channel))
			{
				return;
			}

			Channels.Remove(channel);
			WriteRawMessage($"PART {channel}");
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
			PartFromAllChannels();
			cts?.Cancel();
		}

		public new void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected new virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				cts?.Dispose();
				tcpClient?.Dispose();

				Channels.Clear();

				base.Dispose();
			}
		}
	}
}