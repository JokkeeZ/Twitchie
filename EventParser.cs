using Twitchie2.Enums;

namespace Twitchie2
{
	internal static class EventParser
	{
		public static EventType ParseEventType(string buffer) =>
		ParseCommand(buffer) switch
		{
			"001" => EventType.WelcomeMessage,
			"002" => EventType.Ignored,
			"004" => EventType.Ignored,
			"375" => EventType.Ignored,
			"372" => EventType.Ignored,
			"376" => EventType.Ignored,
			"CAP" => EventType.Ignored,
			"PRIVMSG" => EventType.Message,
			"ROOMSTATE" => EventType.RoomState,
			"NOTICE" => EventType.Notice,
			"HOSTTARGET" => EventType.HostTarget,
			"CLEARCHAT" => EventType.ClearChat,
			"CLEARMSG" => EventType.ClearMessage,
			"PING" => EventType.Ping,
			"MODE" => EventType.Mode,
			"353" => EventType.NameListing,
			"366" => EventType.NameListingEnd,
			"JOIN" => EventType.Join,
			"PART" => EventType.Part,
			"USERNOTICE" => EventType.UserNotice,
			"USERSTATE" => EventType.UserState,
			_ => EventType.NotImplemented
		};

		static string ParseCommand(string buffer)
		{
			var splitted = buffer.Split(' ');

			if (buffer[0] == '@')
				return splitted[2];

			return splitted[0] == "PING" ? splitted[0] : splitted[1];
		}
	}
}
