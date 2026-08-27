namespace VideoOS.IdentityServer.Model.Responses;

public class UserLoginDto
{
	public string LoginProviderId { get; set; }

	public string ProviderKey { get; set; }

	public string ProviderDisplayName { get; set; }
}
