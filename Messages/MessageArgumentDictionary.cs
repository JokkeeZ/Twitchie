using System;
using System.Collections.Generic;

namespace Twitchie2.Messages
{
	internal class MessageArgumentDictionary
	{
		private readonly Dictionary<string, object> collection;

		public MessageArgumentDictionary(string[] arguments)
		{
			collection = new();

			foreach (var arg in arguments)
			{
				var split = arg.Split('=');
				collection.Add(split[0], split[1]);
			}
		}

		internal T GetValue<T>(string key) where T : IEquatable<T>
		{
			if (!collection.TryGetValue(key, out var value))
			{
				return default;
			}

			return MessageArgumentConverter.ConvertValue<T>(value);
		}

		public List<TwitchBadge> PopBadges()
		{
			var badges = GetValue<string>("badges").Split(',');

			if (badges.Length == 0)
			{
				return new();
			}

			var list = new List<TwitchBadge>();

			//<badge>/<version>,<badge>/<version>
			foreach (var badge in badges)
			{
				var split = badge.Split('/');
				list.Add(new(split[0], split[1]));
			}

			return list;
		}
	}
}
