namespace Twitchie2
{
	public class ChatAccount
	{
		public string Nickname { get; set; }
		public string OauthToken { get; set; }
		internal bool LoggedIn { get; set; }
		public string OwnChannel => $"#{Nickname}";
	}
}