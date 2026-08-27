using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.Common.EnvironmentServices.Web;
using VideoOS.IdentityServer.Common;
using VideoOS.IdentityServer.Common.Handlers;

namespace VideoOS.Management.Server;

public class IdpTokenService : IGeminiServerService, IDisposable, IIdpTokenService
{
	private bool _isDisposed;

	private readonly IdpTokenClient _idpTokenClient;

	private readonly IOpenIdTokenHandler _openIdTokenHandler;

	private static Uri AuthorizationServerUri => new Uri(Config.Instance.GetAppSetting("AuthorizationServerUri", "http://localhost:80/IDP"));

	public IdpTokenService()
		: this(new IdpTokenClient(new HttpClientFactory().Create(), new IdentityServerClientSettings(AuthorizationServerUri, "GrantValidatorClient", "password")), new OpenIdTokenHandler(AuthorizationServerUri))
	{
	}

	public IdpTokenService(IdpTokenClient idpTokenClient, IOpenIdTokenHandler openIdTokenHandler)
	{
		_idpTokenClient = idpTokenClient ?? throw new ArgumentNullException("idpTokenClient");
		_openIdTokenHandler = openIdTokenHandler ?? throw new ArgumentNullException("openIdTokenHandler");
	}

	public Task<TokenResponse> ResourceOwnerAuthenticationAsync(string username, string password)
	{
		return _idpTokenClient.ResourceOwnerAuthenticationAsync(username, password);
	}

	public async Task<ClaimsPrincipal> ValidateTokenAsync(string token)
	{
		return await _openIdTokenHandler.ValidateToken(token, CancellationToken.None);
	}

	public void StartService(IGeminiRequestContext requestContext)
	{
	}

	public void StopService(IGeminiRequestContext requestContext)
	{
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!_isDisposed && disposing)
		{
			((IDisposable)_idpTokenClient)?.Dispose();
		}
		_isDisposed = true;
	}
}
