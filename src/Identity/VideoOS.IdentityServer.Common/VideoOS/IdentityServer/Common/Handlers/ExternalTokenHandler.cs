using System;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.Common.EnvironmentServices.Web;
using VideoOS.IdentityServer.Common.Utilities.Tokens;

namespace VideoOS.IdentityServer.Common.Handlers;

public class ExternalTokenHandler : RefreshTokenHandlerBase, IDisposable
{
	private readonly IIdpTokenRetriever _idpTokenRetriever;

	private readonly string _provider;

	private static string[] Scopes = new string[6] { "openid", "profile", "managementserver", "offline_access", "incidentmanagerserver", "provider" };

	private bool _disposedValue;

	public ExternalTokenHandler(string provider, IIdentityServerClientSettings identityServerClientSettings, string[] supportedScopes)
		: this(new IdpTokenRetriever(identityServerClientSettings, supportedScopes, Scopes), provider, identityServerClientSettings)
	{
	}

	public ExternalTokenHandler(IHttpClient client, string provider, IIdentityServerClientSettings identityServerClientSettings, string[] supportedScopes)
		: this(client, new IdpTokenRetriever(identityServerClientSettings, supportedScopes, Scopes), provider, identityServerClientSettings)
	{
	}

	public ExternalTokenHandler(IIdpTokenRetriever idpTokenRetriever, string provider, IIdentityServerClientSettings identityServerClientSettings)
		: this(new HttpClientFactory().Create(), idpTokenRetriever, provider, identityServerClientSettings)
	{
	}

	public ExternalTokenHandler(IHttpClient client, IIdpTokenRetriever idpTokenRetriever, string provider, IIdentityServerClientSettings identityServerClientSettings)
		: base(client, identityServerClientSettings)
	{
		if (idpTokenRetriever == null)
		{
			throw new ArgumentNullException("idpTokenRetriever");
		}
		if (string.IsNullOrEmpty(provider))
		{
			throw new ArgumentNullException("provider");
		}
		_idpTokenRetriever = idpTokenRetriever;
		_provider = provider;
	}

	protected override async Task<TokenResponse> InitialLoginAsync(CancellationToken cancellationToken)
	{
		return await _idpTokenRetriever.RetrieveTokensAsync(base.IdpClient, _provider, base.IdentityServerClientSettings.ClientId, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);
		if (!_disposedValue && disposing)
		{
			_idpTokenRetriever?.Dispose();
			_disposedValue = true;
		}
	}
}
