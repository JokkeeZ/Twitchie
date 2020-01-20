using System;

namespace Twitchie2.Events
{
	public class HostTargetEventArgs : EventArgs
	{
		public int Viewers { get; }
		public string Channel { get; }
		public string TargetChannel { get; }
		public bool IsStarting { get; }

		public HostTargetEventArgs(string message)
		{
			var splittedMessage = message.Split(' ');

			if (int.TryParse(splittedMessage[4], out var viewers))
				Viewers = viewers;

			Channel = splittedMessage[2].Substring(1);

			if (splittedMessage[3] != ":-")
			{
				TargetChannel = splittedMessage[3].Replace(":", "");
				IsStarting = true;
			}
		}
	}
}