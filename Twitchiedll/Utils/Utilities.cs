namespace Twitchiedll.Utils
{
    internal class Utilities
    {
        internal static bool ConvertToBoolean(string str) => str.Equals("1");

        internal static bool BufferElementEquals(string buffer, int element, string str) => buffer.Split(' ')[element] == str;
    }
}