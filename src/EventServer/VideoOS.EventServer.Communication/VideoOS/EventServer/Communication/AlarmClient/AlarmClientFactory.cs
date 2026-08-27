using System;
using VideoOS.Platform;
using VideoOS.Platform.Login;

namespace VideoOS.EventServer.Communication.AlarmClient;

public class AlarmClientFactory : IAlarmClientFactory
{
	public AlarmClient Create(ServerId serverId, string eventServerAddress, Version eventServerVersion, string alarmServerAddress)
	{
		if (LoginSettingsCache.GetLoginSettings(serverId).IsOAuthConnection)
		{
			return new AlarmClientToken(serverId, eventServerAddress, eventServerVersion, alarmServerAddress);
		}
		return new AlarmClientCredentials(serverId, eventServerAddress, eventServerVersion);
	}
}
