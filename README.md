# Twitchie
C# library for handling Twitch.tv IRC events.

# How to use it?
1. Clone or download repository
2. Build solution with Visual Studio
3. Add built library to your project
4. Have fun

# Example? ([See also this](https://github.com/JokkeeZ/Twitchie/blob/Twitchie2/Twitchie2.Example/Program.cs))
```cs
using (var twitchie = new Twitchie())
{
    twitchie.Connect();
    twitchie.Login("jokkeez", "oauth:password");

    twitchie.SetDefaultChannels(new[] { "#jokkeez" });

    twitchie.OnRawMessage += OnRawMessage;

    await twitchie.ListenAsync();
}
```
