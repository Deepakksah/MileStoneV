using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public class SerializableHeartbeat
{
	public static SerializableItem GetItem(HeartbeatContent source)
	{
		NotificationMapping notificationMapping = new NotificationMapping(source.Notifications);
		SerializableItem serializableItem = new SerializableItem
		{
			Id = source.ServerId,
			Name = "ServerId",
			IsFolder = true
		};
		serializableItem.SetItemType(SerializableItemTypes.Heartbeat);
		serializableItem.Properties.Add("Timestamp", source.Timestamp.ToMilisecString());
		serializableItem.Properties.Add("Timeout", source.Timeout.ToString());
		serializableItem.Properties.Add("S", source.ValidationSLC);
		serializableItem.Properties.Add("NotificationsEnabled", source.NotificationsEnabled ? "Yes" : "No");
		serializableItem.Properties.Add("SyncState", source.SyncState ? "Yes" : "No");
		foreach (KeyValuePair<NotificationKey<string>, Dictionary<Guid, List<DeviceMappingData>>> item in notificationMapping)
		{
			SerializableItem serializableItem2 = new SerializableItem
			{
				Name = "loc-key",
				IsFolder = true
			};
			serializableItem2.SetItemType(SerializableItemTypes.Notification);
			serializableItem2.Properties.Add("loc-key", item.Key.Message);
			foreach (string arguments in item.Key.ArgumentsList)
			{
				SerializableItem serializableItem3 = new SerializableItem
				{
					Name = "loc-args",
					IsFolder = false
				};
				serializableItem3.Properties.Add("loc-args", arguments);
				serializableItem3.SetItemType(SerializableItemTypes.NotificationMessageArgument);
				serializableItem2.Children.Add(serializableItem3);
			}
			foreach (KeyValuePair<Guid, List<DeviceMappingData>> item2 in item.Value)
			{
				SerializableItem serializableItem4 = new SerializableItem
				{
					Id = item2.Key,
					Name = "FirstEventId",
					IsFolder = true
				};
				serializableItem4.SetItemType(SerializableItemTypes.FirstEventId);
				foreach (SerializableItem item3 in item2.Value.Select((DeviceMappingData deviceMappingData) => new SerializableItem
				{
					Id = deviceMappingData.DeviceId,
					Name = "DeviceId",
					IsFolder = false,
					Properties = { 
					{
						"NotificationSourceType",
						deviceMappingData.SourceTypes.ToString()
					} }
				}))
				{
					item3.SetItemType(SerializableItemTypes.Device);
					serializableItem4.Children.Add(item3);
				}
				serializableItem2.Children.Add(serializableItem4);
			}
			serializableItem.Children.Add(serializableItem2);
		}
		foreach (KeyValuePair<DeviceState, List<Guid>> device in source.Configuration.Devices)
		{
			SerializableItem serializableItem5 = new SerializableItem
			{
				Name = "Enabled",
				IsFolder = true
			};
			string serializationValue = GetSerializationValue(device.Key);
			serializableItem5.Properties.Add("Enabled", serializationValue);
			serializableItem5.SetItemType(SerializableItemTypes.Configuration);
			foreach (SerializableItem item4 in device.Value.Select((Guid deviceId) => new SerializableItem
			{
				Id = deviceId,
				Name = "DeviceId",
				IsFolder = false
			}))
			{
				item4.SetItemType(SerializableItemTypes.Device);
				serializableItem5.Children.Add(item4);
			}
			serializableItem.Children.Add(serializableItem5);
		}
		return serializableItem;
	}

	private static bool IsNotificationEventType(SerializableItem item)
	{
		if (SerializableItemTypes.Alarm != item.GetItemType())
		{
			return SerializableItemTypes.FirstEventId == item.GetItemType();
		}
		return true;
	}

	public static HeartbeatContent GetHeartbeat(SerializableItem source)
	{
		if (source == null || SerializableItemTypes.Heartbeat != source.GetItemType())
		{
			return null;
		}
		HeartbeatContent heartbeatContent = new HeartbeatContent
		{
			ServerId = source.Id
		};
		List<NotificationData> list = new List<NotificationData>();
		foreach (SerializableItem child in source.Children)
		{
			switch (child.GetItemType())
			{
			case SerializableItemTypes.Notification:
			{
				SerializableProperties<string, string, StringStringFactory> properties2 = child.Properties;
				string message = "";
				if (properties2.ContainsKey("loc-key"))
				{
					message = properties2["loc-key"];
				}
				List<string> messageArguments = (from messageArgItem in child.Children
					where SerializableItemTypes.NotificationMessageArgument == messageArgItem.GetItemType()
					select messageArgItem.Properties["loc-args"]).ToList();
				list.AddRange(from eventItem in child.Children.Where(IsNotificationEventType)
					from deviceItems in eventItem.Children
					select new { eventItem, deviceItems } into t
					where SerializableItemTypes.Device == t.deviceItems.GetItemType()
					select new NotificationData(t.deviceItems.Id, message, messageArguments, t.eventItem.Id, t.deviceItems.Properties));
				break;
			}
			case SerializableItemTypes.Configuration:
			{
				DeviceState state = DeviceState.Disabled;
				SerializableProperties<string, string, StringStringFactory> properties = child.Properties;
				if (properties.ContainsKey("Enabled"))
				{
					state = GetDeviceState(properties["Enabled"]);
				}
				foreach (SerializableItem item in child.Children.Where((SerializableItem child) => SerializableItemTypes.Device == child.GetItemType()))
				{
					heartbeatContent.Configuration.AddDeviceConfiguration(item.Id, state);
				}
				break;
			}
			}
		}
		SerializableProperties<string, string, StringStringFactory> properties3 = source.Properties;
		if (properties3.ContainsKey("Timestamp"))
		{
			heartbeatContent.Timestamp = TimeConverter.FromString(properties3["Timestamp"]);
		}
		if (properties3.ContainsKey("Timeout"))
		{
			heartbeatContent.Timeout = Convert.ToUInt16(properties3["Timeout"]);
		}
		if (properties3.ContainsKey("S"))
		{
			heartbeatContent.ValidationSLC = properties3["S"];
		}
		if (properties3.ContainsKey("NotificationsEnabled"))
		{
			heartbeatContent.NotificationsEnabled = "Yes".Equals(properties3["NotificationsEnabled"]);
		}
		if (properties3.ContainsKey("SyncState"))
		{
			heartbeatContent.SyncState = "Yes".Equals(properties3["SyncState"]);
		}
		heartbeatContent.Notifications.AddRange(list);
		return heartbeatContent;
	}

	public static string GetSerializationValue(DeviceState state)
	{
		return state switch
		{
			DeviceState.Enabled => "Yes", 
			DeviceState.MarkedForDeletion => "MarkedForDeletion", 
			_ => "No", 
		};
	}

	public static DeviceState GetDeviceState(string selializationValue)
	{
		return selializationValue switch
		{
			"Yes" => DeviceState.Enabled, 
			"MarkedForDeletion" => DeviceState.MarkedForDeletion, 
			_ => DeviceState.Disabled, 
		};
	}
}
