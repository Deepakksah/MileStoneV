using System;
using VideoOS.GisMapCommunication.Exceptions;
using VideoOS.Platform;
using VideoOS.Platform.Login;

namespace VideoOS.GisMapCommunication.Notification;

public static class NotificationClientFactory
{
	public static INotificationClient Create(ServerId serverId, TimeSpan requestTimeout, Uri uri)
	{
		if ((LoginSettingsCache.GetLoginSettings(serverId) ?? throw new LoginSettingsNotFoundException(serverId)).IsOAuthConnection)
		{
			return new NotificationClientToken(serverId, requestTimeout, uri);
		}
		return new NotificationClientCredentials(serverId, requestTimeout, uri);
	}
}
