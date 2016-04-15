using System;
using Twitchiedll.IRC;
using Twitchiedll.IRC.Events;

namespace Example
{
    class Program
    {
        // Create new twitchie object with default values.
        static Twitchie twitchie = new Twitchie();

        static void Main(string[] args)
        {
            // Array containing channels where client should join.
            string[] Channels = new string[]
            {
                "#jokkeez",
                "#monstercat",
                "#summit1g"
            };

            // Connect to twitch chat server.
            twitchie.Connect("irc.chat.twitch.tv", 6667);

            // Login to twitch chat server with your twitch name and oauth password.
            twitchie.Login("jokkeez", "jokkeez", Channels, "oauth:password");

            // Make new eventhandlers for events you need
            // OnPing is needed, see this:
            // https://github.com/justintv/Twitch-API/blob/master/IRC.md#upon-a-successful-connection
            twitchie.OnPing += OnPing;
            twitchie.OnMessage += OnMessage;

            //Finally listen for incoming messages and events, returns false if connection is dropped.
            twitchie.Listen();
        }

        static void OnMessage(MessageEventArgs e)
        {
            Console.WriteLine($"{e.Username} on channel {e.Channel}: {e.Message}");
        }

        static void OnPing(string Buffer)
        {
            twitchie.GetMessageHandler().WriteRawMessage(Buffer.Replace("PING", "PONG"));
        }
    }
}