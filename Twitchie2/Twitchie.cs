using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Twitchie2.Events;
using Twitchie2.Messages;

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
		public event EventHandler<MessageEventArgs> OnMention;

		public Twitchie() : base() => Channels = new List<string>();

		public async Task ConnectAsync()
		{
			try
			{
				cts = new CancellationTokenSource();
				tcpClient = new TcpClient();

				await tcpClient.ConnectAsync("irc.chat.twitch.tv", 6667);

				if (tcpClient.Connected)
					InitializeStream(tcpClient.GetStream());

			}
			catch (SocketException ex)
			{
#if DEBUG
				Debug.WriteLine(ex.Message);
#endif
			}
		}

		public void SetDefaultChannels(IEnumerable<string> channels) => Channels.AddRange(channels);

		public void SetDefaultChannel(string channel) => Channels.Add(channel);

		public void Login(string nickname, string password)
		{
			this.nickname = nickname.ToLower();

			if (writer == null)
				throw new Exception($"Twitchie needs to be connected to the Twitch IRC, before login is available.");

			WriteIrcMessage($"PASS {password}");
			WriteIrcMessage($"NICK {this.nickname}");

			WriteIrcMessage("CAP REQ :twitch.tv/membership");
			WriteIrcMessage("CAP REQ :twitch.tv/commands");
			WriteIrcMessage("CAP REQ :twitch.tv/tags");

			OnMessage += (sender, args) =>
			{
				if (args.Message.ToLower().Contains($"@{this.nickname}"))
					OnMention?.Invoke(this, new MessageEventArgs(this, new TwitchIrcMessage(args.RawMessage)));
			};
		}

		public async Task ListenAsync()
		{
			using var input = new StreamReader(tcpClient.GetStream());

			while (!cts.IsCancellationRequested)
			{
				var buffer = await input.ReadLineAsync();
				if (buffer == null)
					return;

				OnRawMessage?.Invoke(this, new RawMessageEventArgs(buffer));

				var eventType = EventParser.ParseEventType(buffer);
				HandleIrcEvent(eventType, new TwitchIrcMessage(buffer));

				if (buffer.Split(' ')[1] == "001" && Channels.Count > 0)
					Channels.ForEach(channel => JoinChannel(channel));
			}
		}

		private void HandleIrcEvent(EventType eventType, TwitchIrcMessage msg)
		{
			switch (eventType)
			{
				case EventType.ClearChat:
					OnClearChat?.Invoke(this, new ClearChatEventArgs(this, msg));
					break;

				case EventType.HostTarget:
					OnHostTarget?.Invoke(this, new HostTargetEventArgs(this, msg));
					break;

				case EventType.Join:
					OnJoin?.Invoke(this, new JoinEventArgs(this, msg));
					break;

				case EventType.Message:
					OnMessage?.Invoke(this, new MessageEventArgs(this, msg));
					break;

				case EventType.Mode:
					OnMode?.Invoke(this, new ModeEventArgs(this, msg));
					break;

				case EventType.Notice:
					OnNotice?.Invoke(this, new NoticeEventArgs(this, msg));
					break;

				case EventType.Part:
					OnPart?.Invoke(this, new PartEventArgs(this, msg));
					break;

				case EventType.RoomState:
					OnRoomState?.Invoke(this, new RoomStateEventArgs(this, msg));
					break;

				case EventType.UserNotice:
					OnUserNotice?.Invoke(this, new UserNoticeEventArgs(this, msg));
					break;

				case EventType.Ping:
					WriteIrcMessage("PONG :tmi.twitch.tv");
					break;
			}
		}

		public void JoinChannel(string channel)
		{
			if (channel[0] != '#')
				channel = $"#{channel}";

			if (!Channels.Contains(channel))
				Channels.Add(channel);

			WriteIrcMessage($"JOIN {channel}");
		}

		public void PartChannel(string channel)
		{
			if (Channels.Remove(channel))
				WriteIrcMessage($"PART {channel}");
		}

		public void PartFromAllChannels() => Channels.ForEach(x => PartChannel(x));

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