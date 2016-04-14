using System;
using Twitchiedll.IRC;
using Twitchiedll.IRC.Events;

namespace Example
{
    class Program
    {
        static Twitchie twitchie = new Twitchie();

        static void Main(string[] args)
        {
            string[] Channels = new string[]
            {
                "#jokkeez",
                "#monstercat",
                "#summit1g"
            };

            twitchie.Connect("irc.chat.twitch.tv", 6667);
            twitchie.Login("jokkeez", "jokkeez", Channels, "oauth:password");

            twitchie.OnPing += OnPing;
            twitchie.OnMessage += OnMessage;

            twitchie.Listen();
        }

        static void OnMessage(MessageEventArgs e)
        {
            Console.WriteLine($"{e.Username} on channel {e.Channel}: {e.Message}");
        }

        static void OnPing(string Buffer)
        {
            twitchie.GetMessageHandler().WriteRawMessage(
                Buffer.Replace("PING", "PONG")
            );
        }
    }
}