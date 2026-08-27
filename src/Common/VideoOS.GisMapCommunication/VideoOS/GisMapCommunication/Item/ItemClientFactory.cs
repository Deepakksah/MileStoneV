using System;
using VideoOS.GisMapCommunication.Exceptions;
using VideoOS.Platform;
using VideoOS.Platform.Login;

namespace VideoOS.GisMapCommunication.Item;

public static class ItemClientFactory
{
	public static IItemClient Create(ServerId serverId, TimeSpan requestTimeout, Uri uri)
	{
		if ((LoginSettingsCache.GetLoginSettings(serverId) ?? throw new LoginSettingsNotFoundException(serverId)).IsOAuthConnection)
		{
			return new ItemClientToken(serverId, requestTimeout, uri);
		}
		return new ItemClientCredentials(serverId, requestTimeout, uri);
	}
}
