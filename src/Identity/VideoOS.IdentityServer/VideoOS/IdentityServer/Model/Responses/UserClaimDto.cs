namespace VideoOS.IdentityServer.Model.Responses;

public class UserClaimDto
{
	public virtual string ClaimType { get; set; }

	public virtual string ClaimValue { get; set; }

	public virtual string LoginProviderId { get; set; }
}
