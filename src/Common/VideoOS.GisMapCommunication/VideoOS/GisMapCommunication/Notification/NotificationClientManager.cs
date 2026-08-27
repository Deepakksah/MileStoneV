using System;
using VideoOS.Platform;

namespace VideoOS.GisMapCommunication.Notification;

public class NotificationClientManager : ClientManagerBase<INotificationClient>
{
	protected override INotificationClient CreateClient(ServerId serverId, TimeSpan requestTimeout, Uri uri)
	{
		return NotificationClientFactory.Create(serverId, requestTimeout, uri);
	}
}
