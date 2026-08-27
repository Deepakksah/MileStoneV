using System;
using System.IdentityModel.Tokens.Jwt;
using VideoOS.IdentityServer.Common;
using VideoOS.IdentityServer.Common.Handlers;

namespace VideoOS.Server;

public sealed class IdpAdminClient : IIdpAdminClient, IDisposable
{
	private bool _isDisposed;

	private IIdpAdministrationClient _idpAdministrationClient;

	private ITokenCache _tokenCache;

	public IdpAdminClient(ITokenCache tokenCache, Uri authorizationServerUrl)
		: this(tokenCache, new IdpAdministrationClient(authorizationServerUrl))
	{
	}

	public IdpAdminClient(ITokenCache tokenCache, IIdpAdministrationClient communicationClient)
	{
		_idpAdministrationClient = communicationClient;
		_tokenCache = tokenCache;
	}

	public void AddAdministrator(string userId)
	{
		CreateAdminModel createAdminModel = new CreateAdminModel
		{
			Identity = userId
		};
		JwtSecurityToken result = _tokenCache.RefreshToken().ConfigureAwait(continueOnCapturedContext: false).GetAwaiter()
			.GetResult();
		_idpAdministrationClient.CreateAdmin(result.RawData, createAdminModel).ConfigureAwait(continueOnCapturedContext: false).GetAwaiter()
			.GetResult();
	}

	public void DeleteAdministrator(string userId)
	{
		JwtSecurityToken result = _tokenCache.RefreshToken().ConfigureAwait(continueOnCapturedContext: false).GetAwaiter()
			.GetResult();
		_idpAdministrationClient.DeleteAdmin(result.RawData, userId).ConfigureAwait(continueOnCapturedContext: false).GetAwaiter()
			.GetResult();
	}

	public void DeleteUser(string userId)
	{
		JwtSecurityToken result = _tokenCache.RefreshToken().ConfigureAwait(continueOnCapturedContext: false).GetAwaiter()
			.GetResult();
		_idpAdministrationClient.DeleteUser(result.RawData, userId).ConfigureAwait(continueOnCapturedContext: false).GetAwaiter()
			.GetResult();
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	private void Dispose(bool disposing)
	{
		if (!_isDisposed && disposing)
		{
			_idpAdministrationClient?.Dispose();
		}
		_isDisposed = true;
	}
}
