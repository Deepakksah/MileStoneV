using System.Security.Claims;
using System.Threading.Tasks;
using VideoOS.IdentityServer.Common;

namespace VideoOS.Management.Server;

public interface IIdpTokenService
{
	Task<TokenResponse> ResourceOwnerAuthenticationAsync(string username, string password);

	Task<ClaimsPrincipal> ValidateTokenAsync(string token);
}
