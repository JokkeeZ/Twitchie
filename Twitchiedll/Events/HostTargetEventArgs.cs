namespace Twitchiedll.IRC.Events
{
    public class HostTargetEventArgs
    {
        public int Viewers { get; internal set; }
        public string Channel { get; internal set; }
        public string TargetChannel { get; internal set; }
        public bool IsStarting { get; internal set; }

        public HostTargetEventArgs(string IrcMessage)
        {
            string[] SplittedMsg = IrcMessage.Split(' ');

            int viewers;

            if (int.TryParse(SplittedMsg[4], out viewers))
                Viewers = viewers;
            else
                Viewers = 0;

            Channel = SplittedMsg[2];

            if (SplittedMsg[3] != ":-")
            {
                TargetChannel = SplittedMsg[3].Replace(":", "");
                IsStarting = true;
            }
            else
            {
                TargetChannel = "";
                IsStarting = false;
            }
        }
    }
}