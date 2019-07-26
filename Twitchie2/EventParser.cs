namespace Twitchie2
{
	public class EventParser
	{
		public static EventType ParseEventType(string buffer)
		{
			var command = ParseCommand(buffer);

			if (command == "PRIVMSG")
			{
				return EventType.Message;
			}

			if (command == "ROOMSTATE")
			{
				return EventType.RoomState;
			}

			if (command == "NOTICE")
			{
				return EventType.Notice;
			}

			if (command == "HOSTTARGET")
			{
				return EventType.HostTarget;
			}

			if (command == "CLEARCHAT")
			{
				return EventType.ClearChat;
			}

			if (command == "PING")
			{
				return EventType.Ping;
			}

			if (command == "MODE")
			{
				return EventType.Mode;
			}

			// Names starting
			if (command == "353")
			{
				return EventType.NotImplemented;
			}

			// Names ending
			if (command == "366")
			{
				return EventType.NotImplemented;
			}

			if (command == "JOIN")
			{
				return EventType.Join;
			}

			if (command == "PART")
			{
				return EventType.Part;
			}

			return EventType.NotImplemented;
		}

		static string ParseCommand(string buffer)
		{
			var splitted = buffer.Split(' ');
			if (buffer[0] == '@')
			{
				return splitted[2];
			}

			return splitted[0] == "PING" ? splitted[0] : splitted[1];
		}
	}
}
