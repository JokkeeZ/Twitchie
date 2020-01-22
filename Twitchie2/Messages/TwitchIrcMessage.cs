using System.IO;

namespace Twitchie2.Messages
{
	public class TwitchIrcMessage
	{
		private readonly TextReader reader;

		public string Content { get; }

		public TwitchIrcMessage(string content)
		{
			Content = content;
			reader = new StringReader(content);
		}

		public string PopArgument()
		{
			var arg = "";

			int c;
			while ((c = reader.Read()) != ' ' && c != '\r' && c != '\n')
				arg += (char)c;

			return arg.TrimEnd('\r', '\n');
		}

		public void SkipArguments(int count)
		{
			for (var i = 0; i < count; ++i)
				_ = PopArgument();
		}

		public string GetRemainingMessage(bool removeColon = false)
		{
			var message = reader.ReadToEnd().TrimEnd('\r', '\n');

			if (removeColon)
				message = message.Substring(1);

			return message;
		}

		public T GetRemainingArgument<T>()
		{
			var message = reader.ReadToEnd().TrimEnd('\r', '\n');
			return MessageArgumentConverter.ConvertValue<T>(message);
		}

		public (string username, string host) PopUserHostArgument()
		{
			var message = PopArgument().Split('@');

			var username = message[0].Split('!')[1];
			var host = message[1];

			return (username, host);
		}

		internal MessageArgumentDictionary PopDictionaryArgument()
		{
			var args = PopArgument().Substring(1).Split(';');
			return new MessageArgumentDictionary(args);
		}
	}
}
