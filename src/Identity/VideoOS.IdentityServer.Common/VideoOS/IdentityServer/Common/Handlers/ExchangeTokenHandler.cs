using System;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.Common.EnvironmentServices.Web;
using VideoOS.IdentityServer.Common.Exceptions;

namespace VideoOS.IdentityServer.Common.Handlers;

public class ExchangeTokenHandler : RefreshTokenHandlerBase
{
	private readonly string _jwtTrustedAccessToken;

	private const string Scope = "openid profile managementserver offline_access incidentmanagerserver";

	public ExchangeTokenHandler(string jwtTrustedAccessToken, IIdentityServerClientSettings identityServerClientSettings)
		: this(new HttpClientFactory().Create(), jwtTrustedAccessToken, identityServerClientSettings)
	{
	}

	public ExchangeTokenHandler(IHttpClient client, string jwtTrustedAccessToken, IIdentityServerClientSettings identityServerClientSettings)
		: base(client, identityServerClientSettings)
	{
		_jwtTrustedAccessToken = jwtTrustedAccessToken ?? throw new ArgumentNullException("jwtTrustedAccessToken");
		if (string.IsNullOrEmpty(jwtTrustedAccessToken))
		{
			throw new ArgumentException("Trusted token can't be empty", "jwtTrustedAccessToken");
		}
	}

	protected override async Task<TokenResponse> InitialLoginAsync(CancellationToken cancellationToken)
	{
		TokenResponse tokenResponse = await base.IdpClient.ExchangeTrustedAccessTokenForTokensAsync(_jwtTrustedAccessToken, "openid profile managementserver offline_access incidentmanagerserver", cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		if (string.IsNullOrEmpty(tokenResponse.Refresh_Token))
		{
			throw new IdpInvalidRefreshTokenException();
		}
		return await base.IdpClient.RefreshTokenRequestAsync(tokenResponse.Refresh_Token, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}
}
