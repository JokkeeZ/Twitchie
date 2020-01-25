using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Twitchie2.Enums;
using Twitchie2.Events;
using Twitchie2.Messages;

namespace Twitchie2
{
	public class Twitchie : OutputMessageHandler
	{
		private CancellationTokenSource cts;
		private TcpClient tcpClient;

		internal static Twitchie Instance { get; private set; }

		public List<TwitchIrcChannel> Channels { get; }
		public ChatAccount Account { get; private set; }

		#region Events
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
		#endregion

		public Twitchie() : base()
		{
			Channels = new List<TwitchIrcChannel>();
			Instance = this;
		}

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
				throw ex;
			}
		}

		public void AddChannels(IEnumerable<TwitchIrcChannel> channels) => Channels.AddRange(channels);

		public void AddChannel(TwitchIrcChannel channel) => Channels.Add(channel);

		public void AddChannel(string channel) => Channels.Add(new TwitchIrcChannel(channel));

		public void Login(ChatAccount account)
			=> Login(account.Nickname, account.OauthToken);

		public void Login(string nickname, string password)
		{
			if (Account != null && Account.LoggedIn)
				return;

			Account ??= new ChatAccount
			{
				Nickname = nickname.ToLower(),
				OauthToken = password,
			};

			if (writer == null)
				throw new Exception($"Twitchie needs to be connected to the Twitch IRC, before login is available.");

			WriteIrcMessage($"PASS {Account.OauthToken}");
			WriteIrcMessage($"NICK {Account.Nickname}");

			RequestCapabilities("membership");
			RequestCapabilities("commands");
			RequestCapabilities("tags");

			Account.LoggedIn = true;

			OnMessage += (sender, args) =>
			{
				if (args.Message.ToLower().Contains($"@{Account.Nickname}"))
					OnMention?.Invoke(this, new MessageEventArgs(new TwitchIrcMessage(args.RawMessage)));
			};
		}

		public async Task ListenAsync()
		{
			using var input = new StreamReader(tcpClient.GetStream());

			while (!cts.IsCancellationRequested)
			{
				var buffer = await input.ReadLineAsync();
				if (buffer == null || (buffer?.Length == 0))
					return;

				OnRawMessage?.Invoke(this, new RawMessageEventArgs(buffer));

				var eventType = EventParser.ParseEventType(buffer);
				HandleIrcEvent(eventType, new TwitchIrcMessage(buffer));

				if (buffer.Split(' ')[1] == "001")
					Channels.ForEach(channel => channel.Join());
			}
		}

		private void HandleIrcEvent(EventType eventType, TwitchIrcMessage msg)
		{
			switch (eventType)
			{
				case EventType.ClearChat:
					OnClearChat?.Invoke(this, new ClearChatEventArgs(msg));
					break;

				case EventType.HostTarget:
					OnHostTarget?.Invoke(this, new HostTargetEventArgs(msg));
					break;

				case EventType.Join:
					OnJoin?.Invoke(this, new JoinEventArgs(msg));
					break;

				case EventType.Message:
					OnMessage?.Invoke(this, new MessageEventArgs(msg));
					break;

				case EventType.Mode:
					OnMode?.Invoke(this, new ModeEventArgs(msg));
					break;

				case EventType.Notice:
					OnNotice?.Invoke(this, new NoticeEventArgs(msg));
					break;

				case EventType.Part:
					OnPart?.Invoke(this, new PartEventArgs(msg));
					break;

				case EventType.RoomState:
					OnRoomState?.Invoke(this, new RoomStateEventArgs(msg));
					break;

				case EventType.UserNotice:
					OnUserNotice?.Invoke(this, new UserNoticeEventArgs(msg));
					break;

				case EventType.Ping:
					WriteIrcMessage("PONG :tmi.twitch.tv");
					break;
			}
		}

		public void PartFromAllChannels() => Channels.ForEach(channel => channel.Part());

		public void RemoveChannel(string name)
		{
			var channel = Channels.FirstOrDefault(x => x.Name == name);
			if (channel == null)
				return;

			channel.Part();
			Channels.Remove(channel);
		}

		public void JoinChannel(string name)
		{
			var channel = Channels.FirstOrDefault(x => x.Name == name);
			if (channel == null)
			{
				channel = new TwitchIrcChannel(name);

				channel.Join();
				Channels.Add(channel);
				return;
			}

			if (!channel.Joined)
				channel.Join();
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