using System;
using VideoOS.Platform;
using VideoOS.Platform.Login;

namespace VideoOS.EventServer.Communication.Status;

public class StatusClientFactory
{
	public StatusClient Create(ServerId serverId, string address, Version eventServerVersion)
	{
		if (eventServerVersion == null)
		{
			throw new Exception("This version of the event server is not supported in the plugin");
		}
		if (LoginSettingsCache.GetLoginSettings(serverId).IsOAuthConnection)
		{
			return new StatusClientToken2(serverId, address, eventServerVersion);
		}
		return new StatusClientCredentials(serverId, address, eventServerVersion);
	}
}
