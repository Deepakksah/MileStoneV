using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Security.Principal;

namespace VideoOS.IdentityServer.Common.Handlers;

public class SystemClaimsPrinciple : ClaimsPrincipal
{
	public SystemClaimsPrinciple()
	{
	}

	public SystemClaimsPrinciple(IEnumerable<ClaimsIdentity> identities)
		: base(identities)
	{
	}

	public SystemClaimsPrinciple(IIdentity identity)
		: base(identity)
	{
	}

	public SystemClaimsPrinciple(IPrincipal principal)
		: base(principal)
	{
	}

	public SystemClaimsPrinciple(BinaryReader reader)
		: base(reader)
	{
	}
}
