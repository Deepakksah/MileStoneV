namespace VideoOS.IdentityServer.Common.Model.Responses;

public class GetUserClaimModel
{
	public virtual string ClaimType { get; set; }

	public virtual string ClaimValue { get; set; }

	public string LoginProviderId { get; set; }
}
