using System;
using VideoOS.Mobile.Portable.MetaChannel.CommEntries;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface INotificationsAsync
{
	IAsyncResult RegisterForNotifications(NotificationParams notificationParams, OnResponse<RegisterForNotificationsResponse> onSuccess = null, OnResponse<RegisterForNotificationsResponse> onFail = null);

	IAsyncResult SendNotification(NotificationParams notificationParams, OnResponse<SendNotificationResponse> onSuccess = null, OnResponse<SendNotificationResponse> onFail = null);

	IAsyncResult SendHeartbeat(HeartbeatContent content, OnResponse<SendHeartbeatResponse> onSuccess = null, OnResponse<SendHeartbeatResponse> onFail = null);
}
