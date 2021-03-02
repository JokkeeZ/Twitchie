using System;
using System.Collections.Generic;

namespace Twitchie2.Messages
{
	internal class MessageArgumentDictionary
	{
		private readonly Dictionary<string, object> collection;

		public MessageArgumentDictionary(string[] args)
		{
			collection = new();

			foreach (var arg in args)
			{
				var split = arg.Split('=');
				collection.Add(split[0], split[1]);
			}
		}

		internal T GetValue<T>(string key) where T : IEquatable<T>
		{
			if (!collection.TryGetValue(key, out var value))
				return default;

			return MessageArgumentConverter.ConvertValue<T>(value);
		}
	}
}
