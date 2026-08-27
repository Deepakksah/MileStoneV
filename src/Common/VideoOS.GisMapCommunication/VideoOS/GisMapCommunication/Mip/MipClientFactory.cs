using System;
using VideoOS.GisMapCommunication.Exceptions;
using VideoOS.Platform;
using VideoOS.Platform.Login;

namespace VideoOS.GisMapCommunication.Mip;

public class MipClientFactory
{
	public static IMipClient Create(ServerId serverId, TimeSpan requestTimeout, Uri uri)
	{
		if ((LoginSettingsCache.GetLoginSettings(serverId) ?? throw new LoginSettingsNotFoundException(serverId)).IsOAuthConnection)
		{
			return new MipClientToken(serverId, requestTimeout, uri);
		}
		return new MipClientCredentials(serverId, requestTimeout, uri);
	}
}
