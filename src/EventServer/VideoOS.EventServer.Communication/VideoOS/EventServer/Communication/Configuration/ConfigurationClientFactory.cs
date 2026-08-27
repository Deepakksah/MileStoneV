using System;
using VideoOS.Platform;
using VideoOS.Platform.Login;

namespace VideoOS.EventServer.Communication.Configuration;

public class ConfigurationClientFactory
{
	public ConfigurationClient Create(ServerId serverId, string address, Version eventServerVersion)
	{
		if (LoginSettingsCache.GetLoginSettings(serverId).IsOAuthConnection)
		{
			return new ConfigurationClientToken(serverId, address, eventServerVersion);
		}
		return new ConfigurationClientCredentials(serverId, address, eventServerVersion);
	}
}
