using System.Collections.Generic;

namespace Twitchie2.Messages
{
	public class MessageArgumentDictionary
	{
		private readonly Dictionary<string, object> collection;

		public MessageArgumentDictionary(string[] args)
		{
			collection = new Dictionary<string, object>();

			foreach (var arg in args)
			{
				var split = arg.Split('=');
				collection.Add(split[0], split[1]);
			}
		}

		public T GetValue<T>(string key)
			=> MessageArgumentConverter.ConvertValue<T>(collection[key]);
	}
}
