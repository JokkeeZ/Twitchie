namespace Twitchiedll.IRC.Events
{
    public class DisconnectEventArgs
    {
        public string Channel { get; internal set; }
        public string User { get; internal set; }
    }
}