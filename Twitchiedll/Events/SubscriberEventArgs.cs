namespace Twitchiedll.IRC.Events
{
    public class SubscriberEventArgs
    {
        public string Username { get; set; }
        public string Channel { get; set; }
        public int Months { get; set; }
        public string Message { get; set; }

        public SubscriberEventArgs(string IrcMessage)
        {
            string[] SplittedMessage = IrcMessage.Split(' ');
            int months;

            Username = SplittedMessage[3].Remove(0, 1);
            Channel = SplittedMessage[2];
            Message = IrcMessage.Split(':')[2];

            if (SplittedMessage[4].Equals("just") && SplittedMessage[5].Equals("subscribed!"))
                Months = 0;
            else if (SplittedMessage[4].Equals("subscribed") && SplittedMessage[6].Equals("for"))
                if (int.TryParse(SplittedMessage[7], out months))
                    Months = months;
        }
    }
}