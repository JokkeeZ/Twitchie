# Twitchie
Event driven Twitch IRC library written in .NET 5

[![MIT License](https://img.shields.io/static/v1?label=License&message=MIT&color=brightgreen)](https://github.com/JokkeeZ/Twitchie/blob/Twitchie2/LICENSE)

# How to use it?
1. Clone or download repository
2. Build solution with Visual Studio
3. Add built library to your project
4. Obtain required chat oauth token. You can get it easily from [here!](https://twitchapps.com/tmi/)
5. Have fun

# Example?
```cs
static async Task Main()
{
	using var twitchie = new Twitchie();

	twitchie.OnMessage += OnMessage;

	await twitchie.ConnectAsync();
	await twitchie.LoginAsync(new("username", "oauth:password"));

	await twitchie.JoinChannelAsync(new("#channel"));

	await twitchie.ListenAsync();
}

static void OnMessage(object sender, MessageEventArgs e)
{
	Console.WriteLine($"User: {e.DisplayName} on channel: {e.Channel}: {e.Message}");
}
```
# Dependencies
Twitchie doesn't use any external dependencies.
