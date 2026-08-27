using System;
using VideoOS.GisMapCommunication.Exceptions;
using VideoOS.Platform;
using VideoOS.Platform.Login;

namespace VideoOS.GisMapCommunication.Layer;

public static class LayerClientFactory
{
	public static ILayerClient Create(ServerId serverId, TimeSpan requestTimeout, Uri uri)
	{
		if ((LoginSettingsCache.GetLoginSettings(serverId) ?? throw new LoginSettingsNotFoundException(serverId)).IsOAuthConnection)
		{
			return new LayerClientToken(serverId, requestTimeout, uri);
		}
		return new LayerClientCredentials(serverId, requestTimeout, uri);
	}
}
