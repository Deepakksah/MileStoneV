using System;
using VideoOS.GisMapCommunication.Notification.Client;
using VideoOS.Platform;

namespace VideoOS.GisMapCommunication.Notification;

public abstract class NotificationClientBase : AbstractClient<GisNotificationCommandClient, IGisNotificationCommand>, INotificationClient, IClientBase, IDisposable
{
	protected override string Name => "GisNotificationService";

	protected NotificationClientBase(ServerId serverId, TimeSpan requestTimeout, Uri uri)
		: base(serverId, requestTimeout, uri)
	{
	}

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);
	}
}
