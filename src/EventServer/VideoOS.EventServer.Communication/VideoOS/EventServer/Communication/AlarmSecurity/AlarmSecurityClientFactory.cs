using System;
using VideoOS.Platform;
using VideoOS.Platform.Login;

namespace VideoOS.EventServer.Communication.AlarmSecurity;

public class AlarmSecurityClientFactory
{
	public AlarmSecurityClient Create(ServerId serverId, string address, Version eventServerVersion)
	{
		if (LoginSettingsCache.GetLoginSettings(serverId).IsOAuthConnection)
		{
			return new AlarmSecurityClientToken(serverId, address, eventServerVersion);
		}
		return new AlarmSecurityClientCredentials(serverId, address, eventServerVersion);
	}
}
