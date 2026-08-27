using System;
using System.ServiceModel;
using VideoOS.GisMapCommunication.Notification.Client;
using VideoOS.Platform;

namespace VideoOS.GisMapCommunication.Notification;

public class NotificationClientToken : NotificationClientBase
{
	protected override SecurityMode CurrentSecurityMode
	{
		get
		{
			if (!(base.Uri.Scheme == Uri.UriSchemeHttps))
			{
				return SecurityMode.None;
			}
			return SecurityMode.Transport;
		}
	}

	protected override WSMessageEncoding CurrentMessageEncoding => WSMessageEncoding.Text;

	public NotificationClientToken(ServerId serverId, TimeSpan requestTimeout, Uri uri)
		: base(serverId, requestTimeout, uri)
	{
	}

	protected override GisNotificationCommandClient CreateClient()
	{
		GisNotificationCommandClient gisNotificationCommandClient = new GisNotificationCommandClient(CreateWSHttpBindingCredentials(), new EndpointAddress(base.FinalUri, CreateSpnIdentity(Name)));
		AddClientHeaderBehavior(gisNotificationCommandClient);
		AbstractClient<GisNotificationCommandClient, IGisNotificationCommand>.EnsureLargeObjectGraphsAllowed(gisNotificationCommandClient);
		return gisNotificationCommandClient;
	}
}
