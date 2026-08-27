using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Mobile.Portable.Log;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public class NotificationMapping : Dictionary<NotificationKey<string>, Dictionary<Guid, List<DeviceMappingData>>>
{
	public NotificationMapping(List<NotificationData> notifications)
	{
		if (notifications != null)
		{
			AddNotifications(notifications);
		}
	}

	public void AddNotifications(List<NotificationData> notifications)
	{
		try
		{
			foreach (NotificationData notification in notifications)
			{
				AddNotificationData(notification);
			}
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "NotificationMapping.AddNotifications", ex.Message);
		}
	}

	public void AddNotificationData(NotificationData data)
	{
		NotificationKey<string> key = new NotificationKey<string>(data.Message, data.MessageArgsList);
		if (!ContainsKey(key))
		{
			Add(key, new Dictionary<Guid, List<DeviceMappingData>>());
		}
		if (!base[key].ContainsKey(data.EventId))
		{
			base[key].Add(data.EventId, new List<DeviceMappingData>());
		}
		base[key][data.EventId].Add(new DeviceMappingData(data.DeviceId, data.SourceTypes));
	}

	public List<NotificationData> ToNotificationDataList()
	{
		List<NotificationData> list = new List<NotificationData>();
		foreach (NotificationKey<string> messageKey in base.Keys)
		{
			foreach (Guid eventIdKey in base[messageKey].Keys)
			{
				list.AddRange(base[messageKey][eventIdKey].Select((DeviceMappingData entry) => new NotificationData(entry.DeviceId, messageKey.Message, messageKey.ArgumentsList, eventIdKey, entry.SourceTypes)).ToList());
			}
		}
		return list;
	}
}
