using System.Collections.Generic;

namespace Twitchie2
{
	class KeyValueMessage
	{
		private readonly Dictionary<string, string> keyValuePairs;

		public KeyValueMessage()
		{
			keyValuePairs = new Dictionary<string, string>();
		}

		public bool TryGetValue(string key, out string value)
		{
			if (keyValuePairs.TryGetValue(key, out var val))
			{
				value = val;
				return true;
			}

			value = null;
			return false;
		}

		public bool TryGetIntValue(string key, out int value)
		{
			if (TryGetValue(key, out var val))
			{
				value = int.Parse(val);
				return true;
			}

			value = 0;
			return false;
		}

		public static KeyValueMessage Parse(string message)
		{
			var msg = new KeyValueMessage();
			foreach (var part in message.Split(' ')[0].Split(';'))
			{
				var split = part.Split('=');
				if (split[0].StartsWith("@"))
				{
					split[0] = split[0].Substring(1);
				}

				msg.keyValuePairs.Add(split[0], split[1]);
			}

			return msg;
		}
	}
}