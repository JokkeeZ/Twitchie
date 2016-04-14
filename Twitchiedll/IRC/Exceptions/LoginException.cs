using System.IO;

namespace Twitchiedll.IRC.Exceptions
{
    public class LoginException : IOException
    {
        public LoginException(string Message) : base(Message) { }
    }
}