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
	// Initialize Twitchie instance, it will be disposed after ListenAsync ends.
	using var twitchie = new Twitchie();
	
	// Subscribe to events.
	twitchie.OnMessage += OnMessage;

	// Connect to Twitch IRC.
	await twitchie.ConnectAsync();
	
	// Login to Twitch IRC with lowercase username and OAuth token.
	await twitchie.LoginAsync(new("username", "oauth:password"));

	// Join some channel.
	await twitchie.JoinChannelAsync(new("#channel"));

	// Start loop which will receive messages until disposed/cancelled.
	await twitchie.ListenAsync();
}

static void OnMessage(object sender, MessageEventArgs e)
	=> Console.WriteLine($"User: {e.DisplayName} on channel: {e.Channel}: {e.Message}");
```
# Dependencies
Twitchie doesn't use any external dependencies.
