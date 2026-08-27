using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public class SerializableAlarm
{
	public static List<SerializableItem> GetItems(IEnumerable<AlarmLineDetail> alarms)
	{
		return alarms.Select(GetItem).ToList();
	}

	public static SerializableItem GetItem(AlarmLineDetail alarm)
	{
		SerializableItem serializableItem = new SerializableItem
		{
			Id = alarm.Id,
			Name = (alarm.Name ?? "")
		};
		serializableItem.SetItemType(SerializableItemTypes.Alarm);
		serializableItem.Properties.Add("State", alarm.State.ToString());
		serializableItem.Properties.Add("Type", alarm.Type);
		serializableItem.Properties.Add("Timestamp", alarm.Timestamp.ToMilisecString());
		serializableItem.Properties.Add("Source", alarm.SourceName);
		serializableItem.Properties.Add("Message", alarm.Message);
		serializableItem.Properties.Add("Priority", alarm.Priority.ToString());
		serializableItem.Properties.Add("CameraId", alarm.CameraId.ToString());
		serializableItem.Properties.Add("AssignedTo", alarm.AssignedTo);
		serializableItem.Properties.Add("Description", alarm.Description);
		serializableItem.Properties.Add("StateName", alarm.StateName);
		serializableItem.Properties.Add("PriorityName", alarm.PriorityName);
		serializableItem.Properties.Add("SourceId", alarm.SourceId.ToString());
		serializableItem.Properties.Add("MessageId", alarm.MessageId.ToString());
		serializableItem.Properties.Add("Category", alarm.Category.ToString());
		serializableItem.Properties.Add("CategoryName", alarm.CategoryName);
		if (alarm.RelatedCameras != null)
		{
			SerializableItem serializableItem2 = new SerializableItem
			{
				Name = "RelatedCameras"
			};
			serializableItem2.SetItemType(SerializableItemTypes.Folder);
			serializableItem2.Children.AddRange(alarm.RelatedCameras.Select((Tuple<Guid, string> x) => new SerializableItem(SerializableItemTypes.Camera)
			{
				Id = x.Item1,
				Name = x.Item2
			}).ToList());
			serializableItem.Children.Add(serializableItem2);
		}
		return serializableItem;
	}

	public static AlarmLineDetail GetAlarmLineDetail(SerializableItem item)
	{
		AlarmLineDetail alarmLineDetail = null;
		if (item != null && SerializableItemTypes.Alarm == item.GetItemType())
		{
			alarmLineDetail = new AlarmLineDetail
			{
				Id = item.Id,
				Name = item.Name
			};
			SerializableProperties<string, string, StringStringFactory> properties = item.Properties;
			if (properties.ContainsKey("State"))
			{
				alarmLineDetail.State = Convert.ToUInt16(properties["State"]);
			}
			if (properties.ContainsKey("StateName"))
			{
				alarmLineDetail.StateName = properties["StateName"];
			}
			if (properties.ContainsKey("Type"))
			{
				alarmLineDetail.Type = properties["Type"];
			}
			if (properties.ContainsKey("Timestamp"))
			{
				alarmLineDetail.Timestamp = TimeConverter.FromString(properties["Timestamp"]);
			}
			if (properties.ContainsKey("Source"))
			{
				alarmLineDetail.SourceName = properties["Source"];
			}
			if (properties.ContainsKey("Message"))
			{
				alarmLineDetail.Message = properties["Message"];
			}
			if (properties.ContainsKey("Priority"))
			{
				alarmLineDetail.Priority = Convert.ToUInt16(properties["Priority"]);
			}
			if (properties.ContainsKey("PriorityName"))
			{
				alarmLineDetail.PriorityName = properties["PriorityName"];
			}
			if (properties.ContainsKey("CameraId"))
			{
				alarmLineDetail.CameraId = new Guid(properties["CameraId"]);
			}
			if (properties.ContainsKey("AssignedTo"))
			{
				alarmLineDetail.AssignedTo = properties["AssignedTo"];
			}
			if (properties.ContainsKey("Description"))
			{
				alarmLineDetail.Description = properties["Description"];
			}
			if (properties.ContainsKey("SourceId"))
			{
				alarmLineDetail.SourceId = new Guid(properties["SourceId"]);
			}
			if (properties.ContainsKey("MessageId"))
			{
				alarmLineDetail.MessageId = new Guid(properties["MessageId"]);
			}
			if (properties.ContainsKey("Category"))
			{
				alarmLineDetail.Category = Convert.ToUInt16(properties["Category"]);
			}
			if (properties.ContainsKey("CategoryName"))
			{
				alarmLineDetail.CategoryName = properties["CategoryName"];
			}
			if (item.Children.Count > 0)
			{
				alarmLineDetail.RelatedCameras = item.Children.First((SerializableItem x) => !string.IsNullOrEmpty(x.Name) && x.Name.Equals("RelatedCameras"))?.Children.Select((SerializableItem itm) => new Tuple<Guid, string>(itm.Id, itm.Name)).ToList();
			}
		}
		return alarmLineDetail;
	}

	public static IEnumerable<AlarmLineDetail> GetAlarmLinesDetail(IEnumerable<SerializableItem> alarms)
	{
		return alarms?.Select(GetAlarmLineDetail).ToList();
	}
}
