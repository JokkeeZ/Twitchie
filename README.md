# Twitchie
C# library for handling Twitch.tv IRC events.

[![MIT License](https://img.shields.io/static/v1?label=License&message=MIT&color=brightgreen)](https://github.com/JokkeeZ/Twitchie/blob/Twitchie2/LICENSE)

# How to use it?
1. Clone or download repository
2. Build solution with Visual Studio
3. Add built library to your project
4. Have fun

# Example? ([See also this](https://github.com/JokkeeZ/Twitchie/blob/Twitchie2/Twitchie2.Example/Program.cs))
```cs
static async Task Main()
{
	using var twitchie = new Twitchie();

	await twitchie.ConnectAsync();
	twitchie.Login("jokkeez", "oauth:password");

	twitchie.SetDefaultChannels(new[] { "#jokkeez" });

	twitchie.OnMessage += OnMessage;

	await twitchie.ListenAsync();
}

static void OnMessage(object sender, MessageEventArgs e)
{
	Console.WriteLine($"User: {e.DisplayName} on channel: {e.Channel}: {e.Message}");
}
```
