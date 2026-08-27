using System;
using System.ServiceModel;
using VideoOS.GisMapCommunication.Exceptions;
using VideoOS.GisMapCommunication.Notification.Client;
using VideoOS.Platform;
using VideoOS.Platform.Login;
using VideoOS.Platform.Util;

namespace VideoOS.GisMapCommunication.Notification;

public class NotificationClientCredentials : NotificationClientBase
{
	protected override SecurityMode CurrentSecurityMode
	{
		get
		{
			if (!(base.Uri.Scheme == Uri.UriSchemeHttps))
			{
				return SecurityMode.Message;
			}
			return SecurityMode.TransportWithMessageCredential;
		}
	}

	protected override WSMessageEncoding CurrentMessageEncoding => WSMessageEncoding.Text;

	public NotificationClientCredentials(ServerId serverId, TimeSpan requestTimeout, Uri uri)
		: base(serverId, requestTimeout, uri)
	{
	}

	protected override GisNotificationCommandClient CreateClient()
	{
		GisNotificationCommandClient gisNotificationCommandClient = new GisNotificationCommandClient(CreateWSHttpBindingCredentials(), new EndpointAddress(base.FinalUri, CreateSpnIdentity(SpnFactory.GetSpn(base.FinalUri))));
		LoginSettings loginSettings = LoginSettingsCache.GetLoginSettings(base.ServerId);
		if (loginSettings == null)
		{
			throw new LoginSettingsNotFoundException(base.ServerId);
		}
		if (loginSettings.IsOAuthIdentity)
		{
			throw new NotImplementedException("MAP: NotificationClientCredential missing implementtion for OAuth");
		}
		gisNotificationCommandClient.ClientCredentials.Windows.ClientCredential = loginSettings.NetworkCredential;
		AbstractClient<GisNotificationCommandClient, IGisNotificationCommand>.EnsureLargeObjectGraphsAllowed(gisNotificationCommandClient);
		return gisNotificationCommandClient;
	}
}
