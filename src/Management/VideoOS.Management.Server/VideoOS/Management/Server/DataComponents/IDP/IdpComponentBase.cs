using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using VideoOS.IdentityServer.Common;
using VideoOS.IdentityServer.Common.Exceptions;
using VideoOS.IdentityServer.Common.Handlers;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.DataComponents.Idp;

public abstract class IdpComponentBase
{
	private IdpClientService _idpClientService;

	private ITokenCache _tokenCache;

	protected IdpComponentBase(IdpClientService idpClientService, ITokenCacheService tokenCacheService)
	{
		if (idpClientService == null)
		{
			throw new ArgumentNullException("idpClientService");
		}
		if (tokenCacheService == null)
		{
			throw new ArgumentNullException("tokenCacheService");
		}
		_idpClientService = idpClientService;
		_tokenCache = tokenCacheService.IdpTokenCache;
	}

	protected T CallIdp<T>(Func<IIdpAdministrationClient, string, Task<T>> call, Func<IdpException, Exception> handleException)
	{
		try
		{
			JwtSecurityToken result = _tokenCache.RefreshToken().ConfigureAwait(continueOnCapturedContext: false).GetAwaiter()
				.GetResult();
			return call(_idpClientService.GetAdministrationClient(), result.RawData).ConfigureAwait(continueOnCapturedContext: false).GetAwaiter().GetResult();
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			if (ex2 is IdpException arg)
			{
				throw handleException(arg);
			}
			throw new GenericIdpException(VmoResources.VMO68000, ex2);
		}
	}

	protected void CallIdp(Func<IIdpAdministrationClient, string, System.Threading.Tasks.Task> call, Func<IdpException, Exception> handleException)
	{
		CallIdp(async delegate(IIdpAdministrationClient idpAdminClient, string token)
		{
			await call(idpAdminClient, token).ConfigureAwait(continueOnCapturedContext: false);
			return System.Threading.Tasks.Task.CompletedTask;
		}, handleException).ConfigureAwait(continueOnCapturedContext: false);
	}
}
