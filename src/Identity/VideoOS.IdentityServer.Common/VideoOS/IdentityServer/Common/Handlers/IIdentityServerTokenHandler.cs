using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.IdentityServer.Common.Handlers;

public interface IIdentityServerTokenHandler
{
	IIdentityServerClientSettings IdentityServerClientSettings { get; }

	Task<TokenResponse> GetTokenResponseAsync();

	Task<TokenResponse> GetTokenResponseAsync(CancellationToken cancellationToken);
}
