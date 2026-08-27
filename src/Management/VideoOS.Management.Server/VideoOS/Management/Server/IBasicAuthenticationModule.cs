using System.Net.Http.Headers;
using System.Security.Claims;

namespace VideoOS.Management.Server;

public interface IBasicAuthenticationModule
{
	bool AuthenticateUser(AuthenticationHeaderValue authenticationHeaderValue, out ClaimsPrincipal principal);

	bool AuthenticateUser(string authenticationHeaderValue, out ClaimsPrincipal principal);
}
