namespace VideoOS.IdentityServer.Common.Model.Responses;

public class GetUserProviderModel
{
	public virtual string LoginProviderId { get; set; }

	public virtual string ProviderKey { get; set; }

	public virtual string ProviderDisplayName { get; set; }
}
