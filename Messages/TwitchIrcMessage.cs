using System.IO;
using System.Text;

namespace Twitchie2.Messages
{
	public class TwitchIrcMessage
	{
		private readonly TextReader reader;

		public string Content { get; }

		public TwitchIrcMessage(string content)
			=> (Content, reader) = (content, new StringReader(content));

		public string PopArgument()
		{
			var sb = new StringBuilder();

			int c;
			while ((c = reader.Read()) != ' ' && c != -1)
			{
				sb.Append((char)c);
			}

			return sb.ToString().TrimEnd('\r', '\n');
		}

		public void SkipArguments(int count)
		{
			for (var i = 0; i < count; ++i)
			{
				_ = PopArgument();
			}
		}

		public string GetRemainingMessage(bool removeColon = false)
		{
			var message = reader.ReadToEnd().TrimEnd('\r', '\n');

			if (string.IsNullOrEmpty(message))
			{
				return null;
			}

			return removeColon ? message[1..] : message;
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
			var args = PopArgument()[1..].Split(';');
			return new(args);
		}
	}
}
