using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using VideoOS.IdentityServer.Extensions;
using VideoOS.IdentityServer.Services.Interfaces;

namespace VideoOS.IdentityServer.Services;

public class WindowsAuthenticationProvider : IWindowsAuthenticationProvider
{
	public async Task<IAuthenticateResult> AuthenticateAsync(HttpContext context)
	{
		return new WindowsAuthenticateResult(await context.AuthenticateAsync("Windows").IgnoreContext());
	}
}
