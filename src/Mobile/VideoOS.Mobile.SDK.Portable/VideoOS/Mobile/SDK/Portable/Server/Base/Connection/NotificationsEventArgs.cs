using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.MetaChannel.CommEntries;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.Connection;

public class NotificationsEventArgs : EventArgs
{
	public IEnumerable<NotificationData> NotificationData { get; private set; }

	public NotificationsEventArgs(IEnumerable<NotificationData> notificationData)
	{
		NotificationData = notificationData;
	}
}
