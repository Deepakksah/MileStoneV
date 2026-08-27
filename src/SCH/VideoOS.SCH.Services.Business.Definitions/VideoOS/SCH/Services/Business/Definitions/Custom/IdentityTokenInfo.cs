namespace VideoOS.SCH.Services.Business.Definitions.Custom;

public class IdentityTokenInfo
{
	public string IdentityToken;

	public string AccountName;

	public string DomainName;

	public string Sid;

	public IdentityTokenInfo()
	{
	}

	public IdentityTokenInfo(string identityToken, string accountName, string domainName, string sid)
	{
		IdentityToken = identityToken;
		AccountName = accountName;
		DomainName = domainName;
		Sid = sid;
	}
}
