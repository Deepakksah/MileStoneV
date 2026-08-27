using System;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.Common.EnvironmentServices.Web;

namespace VideoOS.IdentityServer.Common.Handlers;

public class IdentityServerTokenHandler : IIdentityServerTokenHandler, IDisposable
{
	private readonly IdpTokenClient _idpClient;

	private readonly string _scope;

	private readonly string _secret;

	private bool _disposedValue;

	public IIdentityServerClientSettings IdentityServerClientSettings { get; }

	public IdentityServerTokenHandler(IIdentityServerClientSettings identityServerClientSettings, string scope, string secret)
		: this(new HttpClientFactory().Create(), identityServerClientSettings, scope, secret)
	{
	}

	public IdentityServerTokenHandler(IHttpClient httpCLient, IIdentityServerClientSettings identityServerClientSettings, string scope, string secret)
	{
		if (identityServerClientSettings == null)
		{
			throw new ArgumentNullException("identityServerClientSettings");
		}
		if (scope == null)
		{
			throw new ArgumentNullException("scope");
		}
		if (string.IsNullOrWhiteSpace(scope))
		{
			throw new ArgumentException("scope can't be an empty string", "scope");
		}
		if (secret == null)
		{
			throw new ArgumentNullException("secret");
		}
		if (string.IsNullOrWhiteSpace(secret))
		{
			throw new ArgumentException("secret can't be an empty string", "secret");
		}
		IdentityServerClientSettings = identityServerClientSettings;
		_scope = scope;
		_secret = secret;
		_idpClient = new IdpTokenClient(httpCLient, identityServerClientSettings);
	}

	public Task<TokenResponse> GetTokenResponseAsync()
	{
		return GetTokenResponseAsync(CancellationToken.None);
	}

	public Task<TokenResponse> GetTokenResponseAsync(CancellationToken cancellationToken)
	{
		return _idpClient.ClientCredentialsAuthenticationAsync(_secret, _scope, cancellationToken);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!_disposedValue && disposing)
		{
			_idpClient.Dispose();
			_disposedValue = true;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
