using System;
using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.IdentityServer.Common.Utilities.Tokens;

public interface IIdpTokenRetriever : IDisposable
{
	Task<TokenResponse> RetrieveTokensAsync(IdpTokenClient idpTokenClient, string externalProvider, string clientId);

	Task<TokenResponse> RetrieveTokensAsync(IdpTokenClient idpTokenClient, string externalProvider, string clientId, CancellationToken cancellationToken);
}
