using System;
using System.Threading.Tasks;
using Twitchie2.Events;

namespace Twitchie2.Example
{
	class Program
	{
		static async Task Main(string[] args)
		{
			using (var twitchie = new Twitchie())
			{
				twitchie.Connect();
				twitchie.Login("jokkeez", "oauth:password");

				twitchie.SetDefaultChannels(new[] { "#jokkeez" });

				twitchie.OnRawMessage += OnRawMessage;

				await twitchie.ListenAsync();
			}
		}

		static void OnRawMessage(object sender, RawMessageEventArgs e)
		{
			Console.WriteLine(e.Message);
		}
	}
}
