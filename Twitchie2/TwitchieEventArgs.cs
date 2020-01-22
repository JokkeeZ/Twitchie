using System;
using System.ComponentModel;

namespace Twitchie2
{
	public abstract class TwitchieEventArgs : EventArgs
	{
		private readonly Twitchie twitchie;

		public string Channel { get; protected set; }

		protected TwitchieEventArgs(Twitchie twitchie) => this.twitchie = twitchie;

		public void SendMessage(string message)
			=> twitchie.SendMessage(Channel, message);

		public void SendAction(string action)
			=> twitchie.SendAction(Channel, action);

		public void SendMention(string user, string message)
			=> twitchie.SendMention(Channel, user, message);

		public void SendWhisper(string user, string message)
			=> twitchie.SendWhisper(Channel, user, message);

		void PrintProperties()
		{
			foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(this))
				Console.WriteLine($"{prop.Name}=#{prop.GetValue(this)}#");
		}
	}
}