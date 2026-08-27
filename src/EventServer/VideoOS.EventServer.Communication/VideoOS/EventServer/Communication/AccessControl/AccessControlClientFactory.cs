using System;
using VideoOS.Platform;
using VideoOS.Platform.Login;

namespace VideoOS.EventServer.Communication.AccessControl;

public static class AccessControlClientFactory
{
	public static AccessControlClient Create(ServerId serverId, Version eventServerVersion, Uri uri)
	{
		return Create(serverId, eventServerVersion, uri, TimeSpan.FromMinutes(1.0));
	}

	public static AccessControlClient Create(ServerId serverId, Version eventServerVersion, Uri uri, TimeSpan requestTimeout)
	{
		if (LoginSettingsCache.GetLoginSettings(serverId).IsOAuthConnection)
		{
			return new AccessControlClientToken(serverId, eventServerVersion, uri, requestTimeout);
		}
		return new AccessControlClientCredentials(serverId, eventServerVersion, uri, requestTimeout);
	}
}
