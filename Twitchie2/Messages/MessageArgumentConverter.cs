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
			catch (Exception ex) when (ex is InvalidCastException || ex is ArgumentNullException)
			{
				return default;
			}
		}
	}
}
