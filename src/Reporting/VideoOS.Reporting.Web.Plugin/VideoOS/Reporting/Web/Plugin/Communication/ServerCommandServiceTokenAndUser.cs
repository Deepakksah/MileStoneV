using System;
using System.Threading.Tasks;
using VideoOS.Reporting.Web.Plugin.Base;
using VideoOS.Reporting.Web.Plugin.Business;
using VideoOS.Reporting.Web.Plugin.Business.Model;
using VideoOS.Reporting.Web.Plugin.Settings;
using VideoOS.WebService.Common.ServerCommandService;

namespace VideoOS.Reporting.Web.Plugin.Communication;

public class ServerCommandServiceTokenAndUser
{
	private static object _appPoolSecurityTokenLock = new object();

	private static SecurityToken _appPoolSecurityToken = EmptySecurityToken;

	private static SecurityToken EmptySecurityToken => new SecurityToken(string.Empty, DateTime.MinValue, TimeSpan.FromSeconds(1.0));

	public static SecurityToken GetAppPoolSecurityToken(IServiceLogger serviceLogger)
	{
		lock (_appPoolSecurityTokenLock)
		{
			if (_appPoolSecurityToken.IsCloseToExpire)
			{
				RefreshAppPoolSecurityToken(serviceLogger);
			}
			return _appPoolSecurityToken;
		}
	}

	public static void RefreshAppPoolSecurityToken(IServiceLogger serviceLogger)
	{
		lock (_appPoolSecurityTokenLock)
		{
			if (!_appPoolSecurityToken.UpdatePending)
			{
				_appPoolSecurityToken.UpdatePending = true;
				Task.Factory.StartNew(delegate
				{
					GetSecurityToken(serviceLogger);
				}, "GetSecurityToken task", TaskCreationOptions.LongRunning);
			}
		}
	}

	public static IdentityTokenInfo GetCorporateUserInfo(string token, IServiceLogger serviceLogger)
	{
		using IServerCommandServiceProxy serverCommandServiceProxy = ServerCommandServiceProxy.CreateNew(ConfigSettings.AuthorizationServerUri.Host, ConfigSettings.AuthorizationServerUri.Port, ConfigSettings.Secure, ReportSecurity.SecurityTokenCache);
		UserInfo userInfo = null;
		try
		{
			userInfo = serverCommandServiceProxy.GetUserInfo(token);
		}
		catch (Exception ex)
		{
			serverCommandServiceProxy.Abort();
			serviceLogger.LogToServerLog(ex, null, LogType.Error);
			throw new ReportServerException("Connection error");
		}
		if (userInfo == null)
		{
			return new IdentityTokenInfo(null, null, null);
		}
		return new IdentityTokenInfo(token, userInfo.name, userInfo.sid);
	}

	private static void GetSecurityToken(IServiceLogger serviceLogger)
	{
		serviceLogger.LogToServerLog("Getting a new security token.", LogType.Information);
		using IServerCommandServiceProxy serverCommandServiceProxy = ServerCommandServiceProxy.CreateNew(ConfigSettings.AuthorizationServerUri.Host, ConfigSettings.AuthorizationServerUri.Port, ConfigSettings.Secure, ReportSecurity.SecurityTokenCache);
		SecurityToken securityToken = null;
		try
		{
			LoginInfo loginInfo = serverCommandServiceProxy.Login(Guid.Empty, string.Empty);
			securityToken = new SecurityToken(loginInfo.Token, loginInfo.RegistrationTime, TimeSpan.FromTicks(loginInfo.TimeToLive.MicroSeconds * 10));
		}
		catch (Exception ex)
		{
			serviceLogger.LogToServerLog(ex, null, LogType.Error);
			serverCommandServiceProxy.Abort();
		}
		lock (_appPoolSecurityTokenLock)
		{
			if (securityToken == null)
			{
				_appPoolSecurityToken = EmptySecurityToken;
			}
			else
			{
				_appPoolSecurityToken = securityToken;
			}
			_appPoolSecurityToken.UpdatePending = false;
		}
	}
}
