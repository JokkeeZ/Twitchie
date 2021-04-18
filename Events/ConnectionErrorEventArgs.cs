using System;

namespace Twitchie2.Events
{
	public sealed class ConnectionErrorEventArgs : EventArgs
	{
		public Exception Exception { get; }

		public ConnectionErrorEventArgs(Exception ex) => Exception = ex;
	}
}
