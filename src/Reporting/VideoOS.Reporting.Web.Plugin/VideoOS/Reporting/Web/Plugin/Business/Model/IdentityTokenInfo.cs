namespace VideoOS.Reporting.Web.Plugin.Business.Model;

public class IdentityTokenInfo
{
	public string IdentityToken { get; set; }

	public string FullUserName { get; set; }

	public string Sid { get; set; }

	public IdentityTokenInfo()
	{
	}

	public IdentityTokenInfo(string identityToken, string fullUserName, string sid)
	{
		IdentityToken = identityToken;
		FullUserName = fullUserName;
		Sid = sid;
	}
}
