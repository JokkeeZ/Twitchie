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

		public int ParameterCount => Content.Split(' ').Length;

		public string PopArgument()
		{
			var arg = "";

			int c;
			while ((c = reader.Read()) != ' ' && c != '\r' && c != '\n')
				arg += (char)c;

			return arg.TrimEnd('\r', '\n');
		}

		public T PopArgument<T>()
			=> MessageArgumentConverter.ConvertValue<T>(PopArgument());

		public (string key, string value) PopKeyValueArgument()
		{
			var arg = PopArgument().Split('=');
			return (arg[0], arg[1]);
		}

		public void SkipArguments(int count)
		{
			for (var i = 0; i < count; ++i)
				PopArgument();
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

		public MessageArgumentDictionary PopDictionaryArgument(char separator = ';')
		{
			// Removes '@' from the start
			var args = PopArgument().Substring(1).Split(separator);
			return new MessageArgumentDictionary(args);
		}
	}
}
