using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace VideoOS.IdentityServer.Attributes;

public class ClaimRequirementFilter : IAuthorizationFilter, IFilterMetadata
{
	private readonly Claim _claim;

	public ClaimRequirementFilter(Claim claim)
	{
		_claim = claim;
	}

	public void OnAuthorization(AuthorizationFilterContext context)
	{
		if (!context.HttpContext.User.Claims.Any((Claim c) => c.Type == _claim.Type && c.Value == _claim.Value))
		{
			context.Result = new ForbidResult();
		}
	}
}
