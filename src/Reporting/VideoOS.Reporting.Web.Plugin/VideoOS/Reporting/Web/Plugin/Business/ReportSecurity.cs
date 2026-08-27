using System;
using VideoOS.Common.Crypto;
using VideoOS.IdentityServer.Common;
using VideoOS.IdentityServer.Common.Handlers;
using VideoOS.Reporting.Web.Plugin.Alarm;
using VideoOS.Reporting.Web.Plugin.Base;
using VideoOS.Reporting.Web.Plugin.Business.Model;
using VideoOS.Reporting.Web.Plugin.Communication;
using VideoOS.Reporting.Web.Plugin.Settings;
using VideoOS.WebService.Common.SecurityService;

namespace VideoOS.Reporting.Web.Plugin.Business;

public static class ReportSecurity
{
	private static ITokenCache _tokenCache = null;

	private static ITokenCache _failoverTokenCache = null;

	private static readonly object _locker = new object();

	public static ITokenCache SecurityTokenCache
	{
		get
		{
			if (_tokenCache == null)
			{
				lock (_locker)
				{
					if (_tokenCache == null)
					{
						using SecretHandler secretHandler = new SecretHandler(new FileSecretProvider(ConfigSettings.ServerLogFolder, "idp.tmp", "idp.sec"));
						secretHandler.EnsureUserEncryptedSecret();
						string secret = new string(secretHandler.ReadSecret());
						_tokenCache = new TokenCache(new IdentityServerTokenHandler(new IdentityServerClientSettings(ConfigSettings.AuthorizationServerUri, ConfigSettings.ClientRegistrationId.ToString(), "client_credentials"), "managementserver", secret));
						_tokenCache.Start();
					}
				}
			}
			return _tokenCache;
		}
	}

	public static ITokenCache FailoverTokenCache
	{
		get
		{
			if (_failoverTokenCache == null)
			{
				lock (_locker)
				{
					if (_failoverTokenCache == null)
					{
						using SecretHandler secretHandler = new SecretHandler(new FileSecretProvider(ConfigSettings.ServerLogFolder, "idp.tmp", "idp.sec"));
						secretHandler.EnsureUserEncryptedSecret();
						string secret = new string(secretHandler.ReadSecret());
						_failoverTokenCache = new TokenCache(new IdentityServerTokenHandler(new IdentityServerClientSettings(ConfigSettings.AuthorizationServerUri, ConfigSettings.ClientRegistrationId.ToString(), "client_credentials"), "failover", secret));
						_failoverTokenCache.Start();
					}
				}
			}
			return _failoverTokenCache;
		}
	}

	public static IdentityTokenInfo AuthenticateToken(string userToken)
	{
		return AuthenticateToken(userToken, ServiceLogger.Instance);
	}

	public static bool AuthorizedToRead(IdentityTokenInfo identityInformation, string namespaceIdentifier, string securityAction, string securityObject)
	{
		if (identityInformation == null)
		{
			throw new ArgumentNullException("identityInformation");
		}
		try
		{
			using ISecurityProxy securityProxy = SecurityProxy.CreateNew(ConfigSettings.AuthorizationServerUri.Host, ConfigSettings.AuthorizationServerUri.Port, ConfigSettings.Secure, SecurityTokenCache);
			return securityProxy.HasPermissionByActionList(namespaceIdentifier, identityInformation.Sid, new string[1] { securityAction }, securityObject)[0];
		}
		catch (Exception ex)
		{
			ServiceLogger.Instance.LogToServerLog(ex, null, LogType.Error);
			throw new ReportServerException("Connection error");
		}
	}

	public static IdentityTokenInfo AuthenticateToken(string userToken, IServiceLogger logger)
	{
		if (string.IsNullOrEmpty(userToken))
		{
			throw new InvalidParameterException("userToken");
		}
		IdentityTokenInfo identityTokenInfo = null;
		logger.LogToServerLog("Validating the token now.", LogType.Information);
		identityTokenInfo = ServerCommandServiceTokenAndUser.GetCorporateUserInfo(userToken, logger);
		if (identityTokenInfo == null || string.IsNullOrEmpty(identityTokenInfo.FullUserName))
		{
			logger.LogToServerLog("Token validation failed.", LogType.Information);
			return null;
		}
		logger.LogToServerLog("Token was validated; AuthType=Token;Name=" + identityTokenInfo.FullUserName + ";Token=" + identityTokenInfo?.ToString() + ";", LogType.Information);
		return identityTokenInfo;
	}
}
