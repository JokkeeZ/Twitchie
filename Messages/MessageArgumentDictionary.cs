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

		public List<TwitchBadge> PopBadges()
		{
			var list = new List<TwitchBadge>();
			var badges = GetValue<string>("badges");

			//<badge>/<version>,<badge>/<version>
			if (!string.IsNullOrWhiteSpace(badges))
			{
				foreach (var badge in badges.Split(','))
				{
					var split = badge.Split('/');
					list.Add(new(split[0], split[1]));
				}
			}

			return list;
		}
	}
}
