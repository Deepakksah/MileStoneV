using System;
using System.Collections.Generic;
using System.Linq;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public class SerializableAlarmDataSettings
{
	public static List<SerializableItem> GetItems(AlarmDataSettingsDetail alarmDataSettings)
	{
		List<SerializableItem> items = new List<SerializableItem>();
		ConvertLevelNameSettingsToItem(alarmDataSettings.States, ref items, "States");
		ConvertLevelNameSettingsToItem(alarmDataSettings.Priorities, ref items, "Priorities");
		ConvertLevelNameSettingsToItem(alarmDataSettings.Categories, ref items, "Categories");
		return items;
	}

	public static AlarmDataSettingsDetail GetAlarmDataSettings(IEnumerable<SerializableItem> items)
	{
		AlarmDataSettingsDetail alarmDataSettingsDetail = new AlarmDataSettingsDetail();
		if (items != null)
		{
			foreach (SerializableItem item in items)
			{
				switch (item.Name)
				{
				case "States":
					alarmDataSettingsDetail.States.AddRange(ConvertItemToLevelNameSettings(item));
					break;
				case "Priorities":
					alarmDataSettingsDetail.Priorities.AddRange(ConvertItemToLevelNameSettings(item));
					break;
				case "Categories":
					alarmDataSettingsDetail.Categories.AddRange(ConvertItemToLevelNameSettings(item));
					break;
				}
			}
		}
		return alarmDataSettingsDetail;
	}

	private static List<AlarmLevelNameDetail> ConvertItemToLevelNameSettings(SerializableItem item)
	{
		return item.Children.Select((SerializableItem itm) => new AlarmLevelNameDetail
		{
			Id = new Guid(itm.Properties["Id"]),
			Level = int.Parse(itm.Properties["Level"]),
			Name = itm.Properties["Name"]
		}).ToList();
	}

	private static void ConvertLevelNameSettingsToItem(List<AlarmLevelNameDetail> collection, ref List<SerializableItem> items, string name)
	{
		if (collection != null && collection.Count != 0)
		{
			SerializableItem serializableItem = new SerializableItem
			{
				Name = name,
				Children = collection.Select((AlarmLevelNameDetail levelName) => new SerializableItem
				{
					Properties = 
					{
						{
							"Id",
							levelName.Id.ToString()
						},
						{
							"Level",
							levelName.Level.ToString()
						},
						{ "Name", levelName.Name }
					}
				}).ToList()
			};
			serializableItem.SetItemType(SerializableItemTypes.AlarmDataSetting);
			items.Add(serializableItem);
		}
	}
}
