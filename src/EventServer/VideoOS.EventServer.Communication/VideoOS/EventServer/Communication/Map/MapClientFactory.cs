using System;
using VideoOS.Platform;
using VideoOS.Platform.Login;

namespace VideoOS.EventServer.Communication.Map;

public class MapClientFactory
{
	public MapClient Create(ServerId serverId, string address, Version eventServerVersion)
	{
		if (eventServerVersion == null)
		{
			throw new Exception("This version of the event server is not supported in the plugin");
		}
		if (LoginSettingsCache.GetLoginSettings(serverId).IsOAuthConnection)
		{
			return new MapClientToken2(serverId, address, eventServerVersion);
		}
		return new MapClientCredentials(serverId, address, eventServerVersion);
	}
}
