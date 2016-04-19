namespace Twitchiedll.Utils
{
    internal class Utilities
    {
        internal static bool ConvertToBoolean(string Str) => Str.Equals("1");

        internal static bool IsChannelMessage(string Buffer) => Buffer.Contains("PRIVMSG");
    }
}