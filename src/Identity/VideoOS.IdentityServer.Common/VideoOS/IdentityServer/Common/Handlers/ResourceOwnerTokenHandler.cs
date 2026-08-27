using System;
using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.IdentityServer.Common.Handlers;

public class ResourceOwnerTokenHandler : IIdentityServerTokenHandler, IDisposable
{
	private readonly string _username;

	private readonly string _password;

	private bool _disposedValue;

	private IdpTokenClient _client;

	public IIdentityServerClientSettings IdentityServerClientSettings { get; }

	public ResourceOwnerTokenHandler(string username, string password, IIdentityServerClientSettings identityServerClientSettings)
	{
		_username = username ?? throw new ArgumentNullException("username");
		_password = password ?? throw new ArgumentNullException("password");
		IdentityServerClientSettings = identityServerClientSettings ?? throw new ArgumentNullException("identityServerClientSettings");
		_client = new IdpTokenClient(identityServerClientSettings);
	}

	public Task<TokenResponse> GetTokenResponseAsync()
	{
		return GetTokenResponseAsync(CancellationToken.None);
	}

	public Task<TokenResponse> GetTokenResponseAsync(CancellationToken cancellationToken)
	{
		return _client.ResourceOwnerAuthenticationAsync(_username, _password, cancellationToken);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!_disposedValue && disposing)
		{
			_client.Dispose();
			_disposedValue = true;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
