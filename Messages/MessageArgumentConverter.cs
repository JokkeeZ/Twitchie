using System;

namespace Twitchie2.Messages
{
	internal static class MessageArgumentConverter
	{
		internal static T ConvertValue<T>(object argument)
		{
			try
			{
				return (T)Convert.ChangeType(argument, typeof(T));
			}
			catch (Exception ex) when (ex is InvalidCastException or ArgumentNullException)
			{
				return default;
			}
		}
	}
}
