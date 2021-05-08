using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Twitchie2.Enums;
using Twitchie2.Events;
using Twitchie2.Messages;

namespace Twitchie2
{
	public class Twitchie : IDisposable
	{
		private CancellationTokenSource cts;
		private TcpClient tcpClient;

		private TextReader reader;
		private TextWriter writer;

		internal static Twitchie Instance { get; private set; }

		public List<TwitchIrcChannel> Channels { get; private set; }
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
		public event EventHandler<ClearMessageEventArgs> OnClearMessage;
		public event EventHandler<UserNoticeEventArgs> OnUserNotice;
		public event EventHandler<MessageEventArgs> OnMention;
		public event EventHandler<UserStateEventArgs> OnUserState;
		public event EventHandler<ConnectionErrorEventArgs> OnConnectionError;
		#endregion

		public Twitchie() => Channels = new();

		public async Task ConnectAsync()
		{
			cts = new();

			await CreateConnectionAsync();

			Instance = this;
		}

		private async Task CreateConnectionAsync()
		{
			try
			{
				tcpClient = new();
				await tcpClient.ConnectAsync("irc.chat.twitch.tv", 6667);

				if (tcpClient.Connected)
				{
					reader = new StreamReader(tcpClient.GetStream());
					writer = new StreamWriter(tcpClient.GetStream());
				}
			}
			catch (SocketException ex)
			{
				OnConnectionError?.Invoke(this, new(ex));
			}
		}

		public async Task LoginAsync(ChatAccount account)
		{
			if (Account != null && Account.LoggedIn)
				throw new Exception($"Twitchie is already logged in.");

			Account ??= account ?? throw new ArgumentNullException(nameof(account));

			await SendAsync($"PASS {Account.OauthToken}");
			await SendAsync($"NICK {Account.Nickname}");

			await CapAsync("membership");
			await CapAsync("commands");
			await CapAsync("tags");

			Account.LoggedIn = true;

			OnMessage += (sender, args) =>
			{
				if (args.Message.ToLower().Contains($"@{Account.Nickname}"))
				{
					OnMention?.Invoke(this, new(new(args.RawMessage)));
				}
			};
		}

		public async Task ListenAsync()
		{
			while (!cts.IsCancellationRequested)
			{
				string buffer;

				try
				{
					buffer = await reader.ReadLineAsync();
				}
				catch
				{
					continue;
				}

				if (buffer == null)
				{
					return;
				}

				OnRawMessage?.Invoke(this, new(buffer));

				var eventType = EventParser.ParseEventType(buffer);
				await HandleIrcEventAsync(eventType, new(buffer));
			}
		}

		private async Task HandleIrcEventAsync(EventType eventType, TwitchIrcMessage msg)
		{
			switch (eventType)
			{
				case EventType.WelcomeMessage:
					foreach (var channel in Channels)
						await channel.JoinAsync();
					break;

				case EventType.ClearChat:
					OnClearChat?.Invoke(this, new(msg));
					break;

				case EventType.ClearMessage:
					OnClearMessage?.Invoke(this, new(msg));
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
					await SendAsync("PONG :tmi.twitch.tv");
					break;
			}
		}

		public async Task PartAllAsync()
		{
			foreach (var channel in Channels)
			{
				await channel.PartAsync();
			}
		}

		public async Task JoinChannelAsync(TwitchIrcChannel channel)
		{
			if (!Channels.Contains(channel))
			{
				await channel.JoinAsync();
				Channels.Add(channel);
			}
		}

		public async Task<bool> PartChannelAsync(TwitchIrcChannel channel)
		{
			await channel.PartAsync();
			return Channels.Remove(channel);
		}

		public async Task ReconnectAsync()
		{
			await DisconnectAsync();

			await CreateConnectionAsync();
			await LoginAsync(Account);
		}

		public async Task DisconnectAsync()
		{
			await PartAllAsync();

			reader.Close();
			writer.Close();
			tcpClient.Close();

			Account.LoggedIn = false;
		}

		internal async Task SendAsync(string message)
		{
			await writer.WriteAsync($"{message}\r\n");
			await writer.FlushAsync();
		}

		internal async Task CapAsync(string capabilities)
			=> await SendAsync($"CAP REQ :twitch.tv/{capabilities}");

		public async Task ChatAsync(string channel, string message)
			=> await SendAsync($"PRIVMSG {channel} :{message}");

		public async Task ActionAsync(string channel, string message)
			=> await ChatAsync(channel, $"/me {message}");

		public async Task MentionAsync(string channel, string user, string message)
			=> await ChatAsync(channel, $"@{user}, {message}");

		public async Task WhisperAsync(string channel, string receiver, string message)
			=> await ChatAsync(channel, $"/w {receiver} {message}");

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

				reader?.Dispose();
				writer?.Dispose();
				tcpClient?.Dispose();

				Channels.Clear();
			}
		}
	}
}
