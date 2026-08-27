using System;
using VideoOS.GisMapCommunication.Exceptions;
using VideoOS.Platform;
using VideoOS.Platform.Login;

namespace VideoOS.GisMapCommunication.Map;

public static class MapClientFactory
{
	public static IMapClient Create(ServerId serverId, TimeSpan requestTimeout, Uri uri)
	{
		if ((LoginSettingsCache.GetLoginSettings(serverId) ?? throw new LoginSettingsNotFoundException(serverId)).IsOAuthConnection)
		{
			return new MapClientToken(serverId, requestTimeout, uri);
		}
		return new MapClientCredentials(serverId, requestTimeout, uri);
	}
}
