using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.IdentityServer.Common.Handlers;

public interface IOpenIdTokenHandler
{
	void RequestRefresh();

	Task<ClaimsPrincipal> ValidateToken(string token);

	Task<ClaimsPrincipal> ValidateToken(string token, CancellationToken cancellationToken);

	bool CanReadToken(string token);
}
