using System;

namespace Twitchiedll.IRC.Events
{
    public class MessageEventArgs
    {
        public int UserID { get; internal set; }
        public string Username { get; internal set; }
        public string DisplayName { get; internal set; }
        public string ColorHEX { get; internal set; }
        public string Message { get; internal set; }
        public UserType UserType { get; internal set; }
        public string Channel { get; internal set; }
        public bool Subscriber { get; internal set; }
        public bool Turbo { get; internal set; }
        public bool ModFlag { get; internal set; }
        public string RawIRCMessage { get; internal set; }
        public string EmoteSet { get; internal set; }

        public MessageEventArgs(string IRCString)
        {
            RawIRCMessage = IRCString;

            foreach (string part in IRCString.Split(';'))
            {
                if (part.Contains("!"))
                {
                    if (Channel == null)
                        Channel = part.Split('#')[1].Split(' ')[0];

                    if (Username == null)
                        Username = part.Split('!')[1].Split('@')[0];

                    continue;
                }

                if (part.Contains("@color="))
                {
                    if (ColorHEX == null)
                        ColorHEX = part.Split('=')[1];

                    continue;
                }

                if (part.Contains("display-name"))
                {
                    if (DisplayName == null)
                        DisplayName = part.Split('=')[1];

                    continue;
                }

                if (part.Contains("emotes="))
                {
                    if (EmoteSet == null)
                        EmoteSet = part.Split('=')[1];

                    continue;
                }

                if (part.Contains("subscriber="))
                {
                    if (part.Split('=')[1] == "1")
                        Subscriber = true;
                    else
                        Subscriber = false;

                    continue;
                }

                if (part.Contains("turbo="))
                {
                    if (part.Split('=')[1] == "1")
                        Turbo = true;
                    else
                        Turbo = false;

                    continue;
                }

                if (part.Contains("user-id="))
                {
                    UserID = int.Parse(part.Split('=')[1]);
                    continue;
                }

                if (part.Contains("user-type="))
                {
                    switch (part.Split('=')[1].Split(' ')[0])
                    {
                        case "mod":
                            UserType = UserType.MODERATOR;
                            break;
                        case "global_mod":
                            UserType = UserType.GLOBALMODERATOR;
                            break;
                        case "admin":
                            UserType = UserType.ADMIN;
                            break;
                        case "staff":
                            UserType = UserType.STAFF;
                            break;
                        default:
                            UserType = UserType.VIEWER;
                            break;
                    }
                    continue;
                }

                if (part.Contains("mod="))
                {
                    if (part.Split('=')[1] == "1")
                        ModFlag = true;
                    else
                        ModFlag = false;

                    continue;
                }
            }
            Message = IRCString.Split(new string[] { string.Format(" PRIVMSG #{0} :", Channel) }, StringSplitOptions.None)[1];
        }
    }
}