using System;
using System.Collections.Generic;

namespace Twitchie2.Messages
{
	internal class MessageArgumentDictionary
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

		internal T GetValue<T>(string key) where T : IEquatable<T>
			=> MessageArgumentConverter.ConvertValue<T>(collection[key]);
	}
}
