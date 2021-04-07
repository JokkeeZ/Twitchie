namespace Twitchie2
{
	public record ChatAccount
	{
		public string Nickname { get; init; }
		public string OauthToken { get; init; }
		public bool LoggedIn { get; internal set; }
		public string OwnChannel => $"#{Nickname}";

		public ChatAccount(string nickname, string oauthToken)
			=> (Nickname, OauthToken) = (nickname.ToLower(), oauthToken);
	}
}
