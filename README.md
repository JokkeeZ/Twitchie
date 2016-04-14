# Twitchie
C# library for parsing Twitch.tv IRC chat messages.



# How to use it?
1. Download .zip
2. Build it with Visual Studio 2015 (C# 6.0)
3. Add .dll to your project
4. Have fun.

# Example? ([See also this](https://github.com/JokkeeZ/Twitchie/blob/master/Twichiedll.Example/Program.cs))
```cs
Twitchie twitchie = new Twitchie();

twitchie.Connect("irc.chat.twitch.tv", 6667);
twitchie.Login("jokkeez", "jokkeez", new[] { "#jokkeez" }, "oauth:password");

twitchie.OnPing += OnPing;
twitchie.OnMessage += OnMessage;

twitchie.Listen();
```
