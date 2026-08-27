using System;
using VideoOS.Common.EnvironmentServices.Web;
using VideoOS.IdentityServer.Common;
using VideoOS.Management.Server.DataComponents.IDP;

namespace VideoOS.Management.Server;

public class IdpClientService : IGeminiServerService, IDisposable
{
	private bool _isDisposed;

	private IIdpAdministrationClient _idpAdministrationClient;

	private ITokenCacheService tokenCacheService;

	private static Uri AuthorizationServerUri => new Uri(Config.Instance.GetAppSetting("AuthorizationServerUri", "http://localhost:80/IDP"));

	public IdpClientService()
		: this(new IdpAdministrationClient(AuthorizationServerUri, new HttpClientFactory().Create()))
	{
	}

	public IdpClientService(IIdpAdministrationClient idpAdministrationClient)
	{
		_idpAdministrationClient = idpAdministrationClient;
	}

	public IIdpAdministrationClient GetAdministrationClient()
	{
		return _idpAdministrationClient;
	}

	public IUserIdpComponent CreateUserIdpComponent()
	{
		return new UserIdpComponent(this, tokenCacheService);
	}

	public ILoginProviderIdpComponent CreateLoginProviderIdpComponent()
	{
		return new LoginProviderIdpComponent(this, tokenCacheService);
	}

	public ITrustedIssuerIdpComponent CreateTrustedIssuerIdpComponent()
	{
		return new TrustedIssuerIdpComponent(this, tokenCacheService);
	}

	public void StartService(IGeminiRequestContext requestContext)
	{
		tokenCacheService = requestContext.GetService<TokenCacheService>();
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
			_idpAdministrationClient?.Dispose();
		}
		_isDisposed = true;
	}
}
