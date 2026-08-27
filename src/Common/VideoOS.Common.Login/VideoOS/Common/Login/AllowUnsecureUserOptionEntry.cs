namespace VideoOS.Common.Login;

public class AllowUnsecureUserOptionEntry
{
	public bool AllowUnsecureConnection { get; set; }

	public string Username { get; set; }

	public AllowUnsecureUserOptionEntry(string username, bool allowUnsecureConnection)
	{
		Username = username.ToLowerInvariant();
		AllowUnsecureConnection = allowUnsecureConnection;
	}
}
