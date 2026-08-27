using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;

namespace VideoOS.IdentityServer.Services.Interfaces;

public interface IAuthenticateResult
{
	IEnumerable<string> Groups { get; }

	bool IsAuthenticated { get; }

	IEnumerable<Claim> Claims { get; }

	string Name { get; }

	string Sid { get; }

	bool IsInRole(WindowsBuiltInRole windowsBuiltInRole);
}
