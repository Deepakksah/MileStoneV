using System;
using System.Collections.Generic;
using System.Linq;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public class SerializableNotificationData
{
	public static SerializableItem GetItem(NotificationData source)
	{
		SerializableItem serializableItem = new SerializableItem
		{
			Id = source.DeviceId,
			IsFolder = true
		};
		serializableItem.SetItemType(SerializableItemTypes.Notification);
		serializableItem.Properties.Add("EventId", source.EventId.ToString());
		serializableItem.Properties.Add("loc-key", source.Message);
		foreach (string messageArgs in source.MessageArgsList)
		{
			SerializableItem serializableItem2 = new SerializableItem
			{
				Name = "loc-args",
				IsFolder = false
			};
			serializableItem2.Properties.Add("loc-args", messageArgs);
			serializableItem2.SetItemType(SerializableItemTypes.NotificationMessageArgument);
			serializableItem.Children.Add(serializableItem2);
		}
		serializableItem.Properties.Add("NotificationSourceType", source.SourceTypes.ToString());
		return serializableItem;
	}

	public static NotificationData GetNotificationData(SerializableItem item)
	{
		NotificationData result = null;
		if (item != null && item.GetItemType() == SerializableItemTypes.Notification)
		{
			Guid eventId = default(Guid);
			string message = "";
			List<string> list = new List<string>();
			SerializableProperties<string, string, StringStringFactory> properties = item.Properties;
			if (properties.ContainsKey("EventId"))
			{
				eventId = new Guid(properties["EventId"]);
			}
			if (properties.ContainsKey("loc-key"))
			{
				message = properties["loc-key"];
			}
			list.AddRange(from messageArg in item.Children
				where messageArg.GetItemType() == SerializableItemTypes.NotificationMessageArgument
				where messageArg.Properties.ContainsKey("loc-args")
				select messageArg.Properties["loc-args"]);
			result = new NotificationData(item.Id, message, list, eventId, item.Properties);
		}
		return result;
	}
}
