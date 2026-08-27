using System;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.Common.EnvironmentServices.Web;
using VideoOS.IdentityServer.Common.Exceptions;
using VideoOS.IdentityServer.Common.Utilities.Tokens;

namespace VideoOS.IdentityServer.Common.Handlers;

public abstract class RefreshTokenHandlerBase : IDisposable, IIdentityServerTokenHandler
{
	private bool _disposedValue;

	protected IdpTokenClient IdpClient { get; }

	public IIdentityServerClientSettings IdentityServerClientSettings { get; }

	protected IIdpTokens IdpTokens { get; set; }

	protected RefreshTokenHandlerBase(IHttpClient client, IIdentityServerClientSettings identityServerClientSettings)
	{
		IdentityServerClientSettings = identityServerClientSettings ?? throw new ArgumentNullException("identityServerClientSettings");
		IdpClient = new IdpTokenClient(client, identityServerClientSettings);
	}

	protected RefreshTokenHandlerBase(IHttpClient client, IIdpTokens idpTokens, IIdentityServerClientSettings identityServerClientSettings)
	{
		IdpTokens = idpTokens ?? throw new ArgumentNullException("idpTokens");
		IdpClient = new IdpTokenClient(client, identityServerClientSettings ?? throw new ArgumentNullException("identityServerClientSettings"));
	}

	public Task<TokenResponse> GetTokenResponseAsync()
	{
		return GetTokenResponseAsync(CancellationToken.None);
	}

	protected abstract Task<TokenResponse> InitialLoginAsync(CancellationToken cancellationToken);

	public async Task<TokenResponse> GetTokenResponseAsync(CancellationToken cancellationToken)
	{
		TokenResponse tokenResponse = ((IdpTokens != null) ? (await IdpClient.RefreshTokenRequestAsync(IdpTokens.RefreshToken, cancellationToken).ConfigureAwait(continueOnCapturedContext: false)) : (await InitialLoginAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false)));
		IdpTokens = new IdpTokens(tokenResponse.Refresh_Token, tokenResponse.Access_Token, tokenResponse.Id_Token);
		if (string.IsNullOrEmpty(IdpTokens.RefreshToken))
		{
			throw new IdpInvalidRefreshTokenException();
		}
		return tokenResponse;
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!_disposedValue && disposing)
		{
			IdpClient.Dispose();
			_disposedValue = true;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
