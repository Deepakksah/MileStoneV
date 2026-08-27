using System;
using System.Linq;
using System.Security.Claims;

namespace VideoOS.IdentityServer.Common.ServiceModel;

public class OpenIdClaimsAuthorizationManager : ClaimsAuthorizationManager
{
	private readonly IOperationContextHelper _contextHelper;

	public OpenIdClaimsAuthorizationManager()
		: this(new OperationContextHelper())
	{
	}

	public OpenIdClaimsAuthorizationManager(IOperationContextHelper contextHelper)
	{
		if (contextHelper == null)
		{
			throw new ArgumentNullException("contextHelper");
		}
		_contextHelper = contextHelper;
	}

	public override bool CheckAccess(AuthorizationContext context)
	{
		Claim[] requiredClaims = _contextHelper.CollectCurrentActionClaims(context.Action.First().Value, context.Resource.First().Value);
		return ValidateClaims(context.Principal, requiredClaims);
	}

	private static bool ValidateClaims(ClaimsPrincipal claimsPrincipal, Claim[] requiredClaims)
	{
		if (requiredClaims == null || !requiredClaims.Any())
		{
			return true;
		}
		return requiredClaims.All((Claim requiredClaim) => claimsPrincipal.HasClaim(requiredClaim.Type, requiredClaim.Value));
	}
}
