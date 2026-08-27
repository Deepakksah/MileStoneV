using System;
using System.IO;
using VideoOS.Common.Crypto;
using VideoOS.Common.Integration;
using VideoOS.IdentityServer.Common.Exceptions;
using VideoOS.IdentityServer.Common.Handlers;

namespace VideoOS.Management.Server;

public class TokenCacheService : IGeminiServerService, IDisposable, ITokenCacheService
{
	private bool _isDisposed;

	private static ITokenCache _managementServerTokenCache;

	private static ITokenCacheFactory _tokenCacheFactory;

	public ITokenCache IdpTokenCache { get; set; }

	public ITokenCache ManagementServerTokenCache
	{
		get
		{
			return _managementServerTokenCache;
		}
		set
		{
			_managementServerTokenCache = value;
		}
	}

	public static ITokenCache ManagementServerTokenCacheInstance => _managementServerTokenCache;

	public ITokenCache DataCollectorTokenCache { get; set; }

	public ITokenCache ServerLogsTokenCache { get; set; }

	private static Uri AuthorizationServerUri => new Uri(Config.Instance.GetAppSetting("AuthorizationServerUri", "http://localhost:80/IDP"));

	public TokenCacheService()
		: this(new TokenCacheFactory())
	{
	}

	public TokenCacheService(ITokenCacheFactory tokenCacheFactory)
	{
		_tokenCacheFactory = tokenCacheFactory;
	}

	public void StartService(IGeminiRequestContext requestContext)
	{
		FileSecretProvider provider = new FileSecretProvider(Path.Combine(OemResources.Instance.Server.CommonApplicationDataFolder, "IIS", "ManagementServer"), "idp.tmp", "idp.sec");
		string clientId = ReadClientRegistrationId();
		using (SecretHandler secretHandler = new SecretHandler(provider))
		{
			Initialize(secretHandler, AuthorizationServerUri, clientId);
		}
		EnsureIsRunning();
	}

	public void StopService(IGeminiRequestContext requestContext)
	{
	}

	public void Initialize(ISecretHandler secretHandler, Uri authorizationServerUri, string clientId)
	{
		if (secretHandler == null)
		{
			throw new ArgumentNullException("secretHandler");
		}
		if (authorizationServerUri == null)
		{
			throw new ArgumentNullException("authorizationServerUri");
		}
		if (clientId == null)
		{
			throw new ArgumentNullException("clientId");
		}
		try
		{
			secretHandler.EnsureUserEncryptedSecret();
			string secret = new string(secretHandler.ReadSecret());
			IdpTokenCache = _tokenCacheFactory.CreateTokenCache(secret, authorizationServerUri, clientId, "client_credentials", "idp_management");
			IdpTokenCache.Start().ConfigureAwait(continueOnCapturedContext: false).GetAwaiter()
				.GetResult();
			ManagementServerTokenCache = _tokenCacheFactory.CreateTokenCache(secret, authorizationServerUri, clientId, "client_credentials", "managementserver");
			ManagementServerTokenCache.Start().ConfigureAwait(continueOnCapturedContext: false).GetAwaiter()
				.GetResult();
			DataCollectorTokenCache = _tokenCacheFactory.CreateTokenCache(secret, authorizationServerUri, clientId, "client_credentials", "datacollector");
			DataCollectorTokenCache.Start().ConfigureAwait(continueOnCapturedContext: false).GetAwaiter()
				.GetResult();
			ServerLogsTokenCache = _tokenCacheFactory.CreateTokenCache(secret, authorizationServerUri, clientId, "client_credentials", "serverlogs");
			ServerLogsTokenCache.Start().ConfigureAwait(continueOnCapturedContext: false).GetAwaiter()
				.GetResult();
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (RemoveFileSecretException ex2)
		{
			GeminiTrace.Error("Unable to remove secret file", ex2);
			throw;
		}
		catch (Exception ex3)
		{
			GeminiTrace.Error("An error has occured when trying to contact Authorization Server", ex3);
			throw;
		}
	}

	public void EnsureIsRunning()
	{
		try
		{
			IdpTokenCache.RefreshToken().ConfigureAwait(continueOnCapturedContext: false).GetAwaiter()
				.GetResult();
			ManagementServerTokenCache.RefreshToken().ConfigureAwait(continueOnCapturedContext: false).GetAwaiter()
				.GetResult();
			DataCollectorTokenCache.RefreshToken().ConfigureAwait(continueOnCapturedContext: false).GetAwaiter()
				.GetResult();
			ServerLogsTokenCache.RefreshToken().ConfigureAwait(continueOnCapturedContext: false).GetAwaiter()
				.GetResult();
			GeminiTrace.Info("Initializing connection to Authorization Server");
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (IdpException ex2)
		{
			GeminiTrace.Error("Failed to get token from Authorization Server, contact support", ex2);
			throw new AuthorizationServerException("Authorization server replied with a non successful status code.", ex2);
		}
		catch (Exception ex3)
		{
			GeminiTrace.Error("Failed to get token from Authorization Server, contact support", ex3);
			throw new AuthorizationServerException("It was not possible to contact Authorization Server", ex3);
		}
		GeminiTrace.Info("Connected to Authorization Server");
	}

	private static string ReadClientRegistrationId()
	{
		string appSetting = Config.Instance.GetAppSetting("ClientRegistrationId", null);
		Guid result = Guid.Empty;
		if (appSetting == null || !Guid.TryParse(appSetting, out result))
		{
			GeminiTrace.Error("Invalid client id for ManagementServer");
			throw new AuthorizationServerException("Management Server configuration is invalid. Invalid client id");
		}
		return result.ToString();
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
			if (IdpTokenCache != null)
			{
				IdpTokenCache.Dispose();
			}
			if (ManagementServerTokenCache != null)
			{
				ManagementServerTokenCache.Dispose();
			}
			if (DataCollectorTokenCache != null)
			{
				DataCollectorTokenCache.Dispose();
			}
			if (ServerLogsTokenCache != null)
			{
				ServerLogsTokenCache.Dispose();
			}
		}
		_isDisposed = true;
	}
}
