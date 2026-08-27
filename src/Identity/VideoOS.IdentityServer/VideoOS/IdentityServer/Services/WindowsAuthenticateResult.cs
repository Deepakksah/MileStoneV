using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Authentication;
using VideoOS.IdentityServer.Services.Interfaces;

namespace VideoOS.IdentityServer.Services;

public class WindowsAuthenticateResult : IAuthenticateResult
{
	private readonly AuthenticateResult _authenticateResult;

	public IEnumerable<string> Groups
	{
		get
		{
			IIdentity? obj = _authenticateResult?.Principal.Identity;
			IIdentity? obj2 = ((obj is WindowsIdentity) ? obj : null);
			IdentityReferenceCollection source = ((obj2 != null) ? ((WindowsIdentity)obj2).Groups : null);
			return from identityReference in (IEnumerable<IdentityReference>)source
				where identityReference != (IdentityReference)null
				select identityReference.Value;
		}
	}

	public bool IsAuthenticated => _authenticateResult?.Principal is WindowsPrincipal;

	public IEnumerable<Claim> Claims => _authenticateResult.Principal?.Claims;

	public string Name => _authenticateResult.Principal?.Identity?.Name;

	public string Sid => Claims?.FirstOrDefault((Claim claim) => claim.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/primarysid")?.Value;

	public WindowsAuthenticateResult(AuthenticateResult authenticateResult)
	{
		_authenticateResult = authenticateResult;
	}

	public bool IsInRole(WindowsBuiltInRole windowsBuiltInRole)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		ClaimsPrincipal? principal = _authenticateResult.Principal;
		WindowsPrincipal val = (WindowsPrincipal)(object)((principal is WindowsPrincipal) ? principal : null);
		if (val != null)
		{
			return val.IsInRole(windowsBuiltInRole);
		}
		return false;
	}
}
