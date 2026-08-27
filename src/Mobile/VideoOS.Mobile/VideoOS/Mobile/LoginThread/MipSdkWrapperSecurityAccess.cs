using System;
using System.Threading;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Platform;
using VideoOS.Platform.Login;
using VideoOS.Platform.Util;

namespace VideoOS.Mobile.LoginThread;

public static class MipSdkWrapperSecurityAccess
{
	public enum Result
	{
		Yes,
		No,
		DontKnow
	}

	public static readonly TimeSpan DefaultBetweenRetriesTimeout = TimeSpan.FromSeconds(1.0);

	public static Result IsInAdministratorRole(UserContext userContext, uint retriesMaxCount = 1u, uint betweenRetriesTimeoutMs = 0u)
	{
		uint num = 0u;
		bool flag = false;
		Result result = Result.DontKnow;
		while (!flag && num < retriesMaxCount)
		{
			try
			{
				ServerId serverId = MipSdkWrapperServer.Instance.UserContext.Configuration.ServerFQID.ServerId;
				LoginSettings loginSettings = userContext.GetLoginSettings(userContext.Configuration.ServerFQID.ServerId);
				result = ((!SecurityAccess.IsMember(serverId, loginSettings.UserIdentity, SecurityAccess.AdministratorRoleId.ToString())) ? Result.No : Result.Yes);
				flag = true;
			}
			catch (Exception exception)
			{
				Log.Exception(exception);
				Thread.Sleep((int)betweenRetriesTimeoutMs);
			}
			num++;
		}
		return result;
	}
}
