using System;

namespace Twitchie2.Events
{
	public class NoticeEventArgs : EventArgs
	{
		public string MessageId { get; }
		public string Channel { get; }
		public string Message { get; }

		public NoticeEventArgs(string message)
		{
			var splittedMessage = message.Split(' ');

			MessageId = splittedMessage[0].Split('=')[1];
			Channel = splittedMessage[3];
			Message = message.Split(new[] { $"{Channel} :" }, StringSplitOptions.None)[1];
		}
	}
}