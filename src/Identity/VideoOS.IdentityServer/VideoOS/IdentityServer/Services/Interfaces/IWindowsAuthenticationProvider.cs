using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace VideoOS.IdentityServer.Services.Interfaces;

public interface IWindowsAuthenticationProvider
{
	Task<IAuthenticateResult> AuthenticateAsync(HttpContext context);
}
