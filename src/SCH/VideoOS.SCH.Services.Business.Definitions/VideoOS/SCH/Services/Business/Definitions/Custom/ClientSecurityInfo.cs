namespace VideoOS.SCH.Services.Business.Definitions.Custom;

public class ClientSecurityInfo
{
	public string UserName { get; set; }

	public string UserDomain { get; set; }

	public bool IsAnonymous { get; set; }

	public ClientAuthenticationType AuthenticationType { get; set; }

	public ClientSecurityInfo()
	{
		UserName = null;
		UserDomain = null;
		IsAnonymous = true;
		AuthenticationType = ClientAuthenticationType.None;
	}
}
