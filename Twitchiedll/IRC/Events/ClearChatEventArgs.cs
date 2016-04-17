namespace Twitchiedll.IRC.Events
{
    public class ClearChatEventArgs
    {
        public bool IsTimeout { get; internal set; }
        public string Channel { get; internal set; }
        public string TimeoutUsername { get; internal set; }

        public ClearChatEventArgs(string IrcMessage)
        {
            string[] SplittedMsg = IrcMessage.Split(' ');

            Channel = SplittedMsg[2];

            if (SplittedMsg.Length > 3)
            {
                IsTimeout = true;
                TimeoutUsername = SplittedMsg[3].Replace(":", "");
            }
            else
            {
                IsTimeout = false;
                TimeoutUsername = "";
            }
        }
    }
}