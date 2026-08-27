using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace VideoOS.IdentityServer.Attributes;

public class ClaimRequirementAttribute : TypeFilterAttribute
{
	public ClaimRequirementAttribute(string claimType, string claimValue)
		: base(typeof(ClaimRequirementFilter))
	{
		base.Arguments = new object[1]
		{
			new Claim(claimType, claimValue)
		};
	}
}
