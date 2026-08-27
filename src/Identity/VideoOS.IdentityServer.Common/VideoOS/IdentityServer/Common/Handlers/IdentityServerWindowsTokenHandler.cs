using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.Common.EnvironmentServices.Web;

namespace VideoOS.IdentityServer.Common.Handlers;

public class IdentityServerWindowsTokenHandler : IIdentityServerTokenHandler, IDisposable
{
	private readonly IdpTokenClient _idpClient;

	private bool _disposedValue;

	public IIdentityServerClientSettings IdentityServerClientSettings { get; }

	public IdentityServerWindowsTokenHandler(IIdentityServerClientSettings identityServerClientSettings)
		: this(null, identityServerClientSettings)
	{
	}

	public IdentityServerWindowsTokenHandler(ICredentials credentials, IIdentityServerClientSettings identityServerClientSettings)
	{
		if (identityServerClientSettings == null)
		{
			throw new ArgumentNullException("identityServerClientSettings");
		}
		IdentityServerClientSettings = identityServerClientSettings;
		_idpClient = new IdpTokenClient(credentials, identityServerClientSettings);
	}

	public IdentityServerWindowsTokenHandler(ICredentials credentials, IIdentityServerClientSettings identityServerClientSettings, IHttpClientFactory httpClientFactory)
	{
		IdentityServerClientSettings = identityServerClientSettings ?? throw new ArgumentNullException("identityServerClientSettings");
		if (httpClientFactory == null)
		{
			throw new ArgumentNullException("httpClientFactory");
		}
		_idpClient = new IdpTokenClient(credentials, identityServerClientSettings, httpClientFactory);
	}

	public Task<TokenResponse> GetTokenResponseAsync()
	{
		return GetTokenResponseAsync(CancellationToken.None);
	}

	public Task<TokenResponse> GetTokenResponseAsync(CancellationToken cancellationToken)
	{
		return _idpClient.WindowsAuthenticationAsync(null, cancellationToken);
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
