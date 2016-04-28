namespace Twitchiedll.IRC.Events
{
    public class NoticeEventArgs
    {
        public string Channel { get; internal set; }
        public string Message { get; internal set; }
        public Noticetype NoticeType { get; internal set; }

        public NoticeEventArgs(string IrcMessage)
        {
            string[] SplittedMessage = IrcMessage.Split(' ');

            switch (SplittedMessage[0].Split('=')[1])
            {
                case "subs_on":
                    NoticeType = Noticetype.SUBS_ON;
                    break;

                case "subs_off":
                    NoticeType = Noticetype.SUBS_OFF;
                    break;

                case "slow_on":
                    NoticeType = Noticetype.SLOW_ON;
                    break;

                case "slow_off":
                    NoticeType = Noticetype.SLOW_OFF;
                    break;

                case "r9k_on":
                    NoticeType = Noticetype.R9K_ON;
                    break;

                case "r9k_off":
                    NoticeType = Noticetype.R9K_OFF;
                    break;

                case "host_on":
                    NoticeType = Noticetype.HOST_ON;
                    break;

                case "host_off":
                    NoticeType = Noticetype.HOST_OFF;
                    break;
            }

            Channel = SplittedMessage[3];
            Message = IrcMessage.Split(':')[2];
        }
    }
}