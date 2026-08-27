using System;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.Common.EnvironmentServices.Web;
using VideoOS.IdentityServer.Common.Utilities.Tokens;

namespace VideoOS.IdentityServer.Common.Handlers;

public class RefreshTokenHandler : RefreshTokenHandlerBase
{
	public RefreshTokenHandler(IIdpTokens idpTokens, IIdentityServerClientSettings identityServerClientSettings)
		: this(new HttpClientFactory().Create(), idpTokens, identityServerClientSettings)
	{
	}

	public RefreshTokenHandler(IHttpClient client, IIdpTokens idpTokens, IIdentityServerClientSettings identityServerClientSettings)
		: base(client, idpTokens, identityServerClientSettings)
	{
	}

	protected override Task<TokenResponse> InitialLoginAsync(CancellationToken cancellationToken)
	{
		throw new NotSupportedException();
	}
}
