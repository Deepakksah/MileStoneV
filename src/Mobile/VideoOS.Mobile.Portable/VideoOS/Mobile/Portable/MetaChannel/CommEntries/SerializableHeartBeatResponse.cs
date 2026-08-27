using System;
using System.Linq;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public class SerializableHeartBeatResponse
{
	public static SerializableItem GetItem(HeartbeatResponse source)
	{
		SerializableItem serializableItem = new SerializableItem
		{
			Name = "HeartbeatResponse",
			IsFolder = true
		};
		serializableItem.SetItemType(SerializableItemTypes.Heartbeat);
		serializableItem.Properties.Add("HeartbeatResponseResult", source.HeartbeatResult.ToString());
		serializableItem.Properties.Add("Timestamp", source.AcknowledgedTimestamp.ToMilisecString());
		serializableItem.Properties.Add("HeartbeatOnly", ParamsHelper.BoolConvertionTo(source.HeartbeatOnly));
		serializableItem.Properties.Add("HeartbeatInterval", source.HeartbeatInterval.ToString());
		foreach (SerializableItem item in source.UnregisteredDevices.Select((Guid unregisteredDevice) => new SerializableItem
		{
			Id = unregisteredDevice,
			Name = "DeviceId",
			IsFolder = false
		}))
		{
			item.SetItemType(SerializableItemTypes.Device);
			serializableItem.Children.Add(item);
		}
		foreach (SerializableItem item2 in source.UnrecognizedDevices.Select((Guid unrecognizedDevice) => new SerializableItem
		{
			Id = unrecognizedDevice,
			Name = "DeviceId",
			IsFolder = false
		}))
		{
			item2.SetItemType(SerializableItemTypes.UnrecognizedDevice);
			serializableItem.Children.Add(item2);
		}
		return serializableItem;
	}

	public static HeartbeatResponse GetHeartbeatResponse(SerializableItem source)
	{
		if (source == null || SerializableItemTypes.Heartbeat != source.GetItemType())
		{
			return null;
		}
		HeartbeatResponse heartbeatResponse = new HeartbeatResponse();
		SerializableProperties<string, string, StringStringFactory> properties = source.Properties;
		if (properties.ContainsKey("Timestamp"))
		{
			heartbeatResponse.AcknowledgedTimestamp = TimeConverter.FromString(properties["Timestamp"]);
		}
		if (properties.ContainsKey("HeartbeatResponseResult"))
		{
			Enum.TryParse<HeartbeatResult>(properties["HeartbeatResponseResult"], out var result);
			heartbeatResponse.HeartbeatResult = result;
		}
		if (properties.ContainsKey("HeartbeatOnly"))
		{
			heartbeatResponse.HeartbeatOnly = ParamsHelper.BoolConvertion(properties["HeartbeatOnly"]);
		}
		if (properties.ContainsKey("HeartbeatInterval"))
		{
			heartbeatResponse.HeartbeatInterval = (int.TryParse(properties["HeartbeatInterval"], out var result2) ? result2 : 0);
		}
		foreach (SerializableItem item in source.Children.Where((SerializableItem guid) => SerializableItemTypes.Device == guid.GetItemType()))
		{
			heartbeatResponse.UnregisteredDevices.Add(item.Id);
		}
		foreach (SerializableItem item2 in source.Children.Where((SerializableItem guid) => SerializableItemTypes.UnrecognizedDevice == guid.GetItemType()))
		{
			heartbeatResponse.UnrecognizedDevices.Add(item2.Id);
		}
		return heartbeatResponse;
	}
}
