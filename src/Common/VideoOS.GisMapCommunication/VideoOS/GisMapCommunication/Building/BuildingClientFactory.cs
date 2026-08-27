using System;
using VideoOS.GisMapCommunication.Exceptions;
using VideoOS.Platform;
using VideoOS.Platform.Login;

namespace VideoOS.GisMapCommunication.Building;

public class BuildingClientFactory
{
	public static IBuildingClient Create(ServerId serverId, TimeSpan requestTimeout, Uri uri)
	{
		if ((LoginSettingsCache.GetLoginSettings(serverId) ?? throw new LoginSettingsNotFoundException(serverId)).IsOAuthConnection)
		{
			return new BuildingClientToken(serverId, requestTimeout, uri);
		}
		return new BuildingClientCredentials(serverId, requestTimeout, uri);
	}
}
