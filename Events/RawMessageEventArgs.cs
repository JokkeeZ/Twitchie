using System;

namespace Twitchie2.Events
{
	public class RawMessageEventArgs : EventArgs
	{
		public string Message { get; }

		public RawMessageEventArgs(string message) => Message = message;
	}
}
