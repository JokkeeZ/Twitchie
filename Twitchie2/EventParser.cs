using System.Collections.Generic;
using Twitchie2.Enums;

namespace Twitchie2
{
	internal static class EventParser
	{
		static readonly Dictionary<string, EventType> events =
		new Dictionary<string, EventType>
		{
			{ "PRIVMSG", EventType.Message },
			{ "ROOMSTATE",EventType.RoomState },
			{ "NOTICE", EventType.Notice },
			{ "HOSTTARGET", EventType.HostTarget },
			{ "CLEARCHAT", EventType.ClearChat },
			{ "PING", EventType.Ping },
			{ "MODE", EventType.Mode },
			{ "353", EventType.NotImplemented },
			{ "366", EventType.NotImplemented },
			{ "JOIN", EventType.Join },
			{ "PART", EventType.Part },
			{ "USERNOTICE", EventType.UserNotice }
		};

		public static EventType ParseEventType(string buffer)
		{
			var command = ParseCommand(buffer);

			if (events.TryGetValue(command, out var eventType))
				return eventType;

			return EventType.NotImplemented;
		}

		static string ParseCommand(string buffer)
		{
			var splitted = buffer.Split(' ');

			if (buffer[0] == '@')
				return splitted[2];

			return splitted[0] == "PING" ? splitted[0] : splitted[1];
		}
	}
}
