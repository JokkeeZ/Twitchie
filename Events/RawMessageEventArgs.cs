using System;

namespace Twitchie2.Events
{
	public sealed class RawMessageEventArgs : EventArgs
	{
		public string Message { get; }

		public RawMessageEventArgs(string message) => Message = message;
	}
}
