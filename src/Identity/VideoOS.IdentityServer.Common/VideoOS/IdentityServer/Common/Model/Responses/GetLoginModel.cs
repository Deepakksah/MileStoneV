namespace VideoOS.IdentityServer.Common.Model.Responses;

public class GetLoginModel
{
	public string UserId { get; set; }

	public string LoginProviderId { get; set; }

	public string ProviderKey { get; set; }

	public string ProviderDisplayName { get; set; }
}
