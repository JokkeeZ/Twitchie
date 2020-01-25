using System;
using System.Threading.Tasks;
using Twitchie2.Events;

namespace Twitchie2.Example
{
	class Program
	{
		static async Task Main()
		{
			using var twitchie = new Twitchie();

			await twitchie.ConnectAsync();
			twitchie.Login("jokkeez", "oauth:password");

			twitchie.AddChannel("#jokkeez");

			twitchie.OnMessage += OnMessage;

			await twitchie.ListenAsync();
		}

		static void OnMessage(object sender, MessageEventArgs e)
		{
			Console.WriteLine($"User: {e.DisplayName} on channel: {e.Channel.Name}: {e.Message}");
		}
	}
}
