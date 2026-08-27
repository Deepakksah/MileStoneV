namespace VideoOS.IdentityServer.Common;

public class CreateLoginModel
{
	public string UserId { get; set; }

	public string LoginProviderId { get; set; }

	public string ProviderKey { get; set; }

	public string ProviderDisplayName { get; set; }
}
