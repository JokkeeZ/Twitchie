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
	public class Twitchie : OutputMessageHandler, IDisposable
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
		public event EventHandler<UserStateEventArgs> OnUserState;
		public event EventHandler<ConnectionErrorEventArgs> OnConnectionError;
		#endregion

		public Twitchie() => (Channels, Instance) = (new(), this);

		public async Task ConnectAsync()
		{
			try
			{
				cts = new();
				tcpClient = new();

				await tcpClient.ConnectAsync("irc.chat.twitch.tv", 6667);

				if (tcpClient.Connected)
					InitializeStream(tcpClient.GetStream());

			}
			catch (SocketException ex)
			{
				OnConnectionError?.Invoke(this, new(ex));
			}
		}

		public void AddChannels(IEnumerable<TwitchIrcChannel> channels)
		{
			foreach (var channel in channels)
				AddChannel(channel);
		}

		public void AddChannel(TwitchIrcChannel channel)
		{
			if (!Channels.Any(x => x.Name == channel.Name))
				Channels.Add(channel);
		}

		public void AddChannel(string channel) => AddChannel(new TwitchIrcChannel(channel));

		public void Login(ChatAccount account)
			=> Login(account.Nickname, account.OauthToken);

		public void Login(string nickname, string password)
		{
			if (Account != null && Account.LoggedIn)
				return;

			Account ??= new ChatAccount
			{
				Nickname = nickname.ToLower(),
				OauthToken = password
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
					OnMention?.Invoke(this, new(new(args.RawMessage)));
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

				OnRawMessage?.Invoke(this, new(buffer));

				var eventType = EventParser.ParseEventType(buffer);
				HandleIrcEvent(eventType, new(buffer));
			}
		}

		private void HandleIrcEvent(EventType eventType, TwitchIrcMessage msg)
		{
			switch (eventType)
			{
				case EventType.WelcomeMessage:
				Channels.ForEach(channel => channel.Join());
				break;

				case EventType.ClearChat:
				OnClearChat?.Invoke(this, new(msg));
				break;

				case EventType.HostTarget:
				OnHostTarget?.Invoke(this, new(msg));
				break;

				case EventType.Join:
				OnJoin?.Invoke(this, new(msg));
				break;

				case EventType.Message:
				OnMessage?.Invoke(this, new(msg));
				break;

				case EventType.Mode:
				OnMode?.Invoke(this, new(msg));
				break;

				case EventType.Notice:
				OnNotice?.Invoke(this, new(msg));
				break;

				case EventType.Part:
				OnPart?.Invoke(this, new(msg));
				break;

				case EventType.RoomState:
				OnRoomState?.Invoke(this, new(msg));
				break;

				case EventType.UserNotice:
				OnUserNotice?.Invoke(this, new(msg));
				break;

				case EventType.UserState:
				OnUserState?.Invoke(this, new(msg));
				break;

				case EventType.Ping:
				WriteIrcMessage("PONG :tmi.twitch.tv");
				break;
			}
		}

		public void PartFromAllChannels()
			=> Channels.ForEach(channel => channel.Part());

		public bool RemoveChannel(string name)
		{
			var channel = Channels.FirstOrDefault(x => x.Name == (name[0] == '#' ? name : '#' + name));
			if (channel == null)
				return false;

			channel.Part();
			Channels.Remove(channel);

			return true;
		}

		public bool JoinChannel(string name)
		{
			var channel = Channels.FirstOrDefault(x => x.Name == (name[0] == '#' ? name : '#' + name));
			if (channel == null)
			{
				channel = new(name);

				channel.Join();
				Channels.Add(channel);
				return true;
			}

			if (!channel.Joined)
			{
				channel.Join();
				return true;
			}

			return false;
		}

		public void Disconnect()
		{
			PartFromAllChannels();
			cts?.Cancel();
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
				cts?.Dispose();
				tcpClient?.Dispose();

				Channels.Clear();

				writer.Dispose();
			}
		}
	}
}