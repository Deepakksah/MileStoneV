using System;
using VideoOS.Mobile.Portable.MetaChannel.CommEntries;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface INotificationsSync
{
	RegisterForNotificationsResponse RegisterForNotifications(NotificationParams notificationParams, TimeSpan timeout);

	SendNotificationResponse SendNotification(NotificationParams notificationParams, TimeSpan timeout);

	SendHeartbeatResponse SendHeartbeat(HeartbeatContent content, TimeSpan timeout);
}
