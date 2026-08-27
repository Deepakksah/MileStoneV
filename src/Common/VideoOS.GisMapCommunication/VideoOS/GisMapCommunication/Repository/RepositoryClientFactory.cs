using System;
using VideoOS.GisMapCommunication.Exceptions;
using VideoOS.Platform;
using VideoOS.Platform.Login;

namespace VideoOS.GisMapCommunication.Repository;

public static class RepositoryClientFactory
{
	public static IRepositoryClient Create(ServerId serverId, TimeSpan requestTimeout, Uri uri)
	{
		if ((LoginSettingsCache.GetLoginSettings(serverId) ?? throw new LoginSettingsNotFoundException(serverId)).IsOAuthConnection)
		{
			return new RepositoryClientToken(serverId, requestTimeout, uri);
		}
		return new RepositoryClientCredentials(serverId, requestTimeout, uri);
	}
}
