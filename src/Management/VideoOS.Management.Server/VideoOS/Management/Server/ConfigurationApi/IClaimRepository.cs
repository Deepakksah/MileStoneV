namespace VideoOS.Management.Server.ConfigurationApi;

public interface IClaimRepository
{
	Claim[] QueryRoleClaims(IGeminiRequestContext requestContext, string roleId);

	Claim AddClaimToRole(IGeminiRequestContext requestContext, string roleId, string claimProvider, string claimName, string claimValue);

	void RemoveClaimFromRole(IGeminiRequestContext requestContext, string roleId, string claimProvider, string claimName, string claimValue);
}
