using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.EventServer.Communication.Configuration;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class AlarmDefinitionProperties
{
	public const string PriorityKey = "Priority";

	public const string SourceListKey = "SourceList";

	public const string RelatedCameraListKey = "RelatedCameraList";

	public const string OwnerKey = "Owner";

	public const string ManagementTimeoutTimeKey = "ManagementTimeoutTime";

	public const string ManagementTimeoutEventListKey = "ManagementTimeoutEventList";

	public const string TimeProfileKey = "TimeProfile";

	public const string EventTypeKey = "EventType";

	public const string EnableRuleKey = "EnableRule";

	public const string DisableEventListKey = "DisableEventList";

	public const string CategoryKey = "Category";

	public const string AutoCloseKey = "AutoClose";

	public const string EnableEventListKey = "EnableEventList";

	public const string TriggerEventListKey = "TriggerEventlist";

	public const string RelatedMapKey = "RelatedMap";

	public const string EventTypeGroupKey = "EventTypeGroup";

	public const string MapTypeKey = "MapType";

	public const string AssignableToAdminsKey = "AssignableToAdmins";

	public static Property EventTypeGroupProperty(Guid val, bool isSettable, List<EventTypeGroup> eventTypeGroups)
	{
		return new Property("EventTypeGroup", val.ToString())
		{
			ValueType = "Enum",
			IsSettable = isSettable,
			DisplayName = ConfigurationApiResources.PropertyAlarmDefinitionEventTypeGroup,
			ItemReference = false,
			ServerValidation = true,
			TranslationId = "PropertyAlarmDefinitionEventTypeGroup",
			ToolTipTranslationId = "PropertyAlarmDefinitionEventTypeGroupTip",
			UIImportance = UIImportance.Summary,
			SortKey = 100,
			ValueTypeInfos = (isSettable ? eventTypeGroups.Select((EventTypeGroup group) => new ValueTypeInfo
			{
				Name = group.Name,
				Value = group.ID.ToString()
			}).ToArray() : Array.Empty<ValueTypeInfo>())
		};
	}

	public static Property EventTypeProperty(string val, bool isSettable, List<EventType> eventTypes)
	{
		List<ValueTypeInfo> list = null;
		if (isSettable)
		{
			list = new List<ValueTypeInfo>();
			foreach (EventType eventType in eventTypes)
			{
				list.Add(new ValueTypeInfo
				{
					Name = eventType.Message,
					Value = eventType.ID.ToString()
				});
			}
		}
		return new Property("EventType", val)
		{
			ValueType = "Enum",
			IsSettable = isSettable,
			DisplayName = ConfigurationApiResources.PropertyAlarmDefinitionEventType,
			ItemReference = false,
			ServerValidation = true,
			TranslationId = "PropertyAlarmDefinitionEventType",
			ToolTipTranslationId = "PropertyAlarmDefinitionEventTypeTip",
			UIImportance = UIImportance.Summary,
			SortKey = 100,
			ValueTypeInfos = list.ToArray()
		};
	}

	public static Property SourceListProperty(string val, bool isSettable, List<string> itemTypes, bool allowAll)
	{
		List<ValueTypeInfo> list = new List<ValueTypeInfo>();
		foreach (string itemType in itemTypes)
		{
			list.Add(new ValueTypeInfo
			{
				Name = "PathItemType",
				Value = itemType
			});
		}
		if (allowAll)
		{
			list.Add(new ValueTypeInfo
			{
				Name = "PathAllowAllItemType",
				Value = ""
			});
		}
		return new Property("SourceList", val)
		{
			ValueType = "PathList",
			IsSettable = isSettable,
			DisplayName = ConfigurationApiResources.PropertyAlarmDefinitionSourceList,
			ItemReference = true,
			ServerValidation = false,
			TranslationId = "PropertyAlarmDefinitionSourceList",
			ToolTipTranslationId = "PropertyAlarmDefinitionSourceListTip",
			UIImportance = UIImportance.Summary,
			SortKey = 100,
			ValueTypeInfos = list.ToArray()
		};
	}

	public static Property PriorityProperty(string val, bool isSettable, AlarmPriority[] alarmPriorities)
	{
		List<ValueTypeInfo> list = null;
		if (isSettable)
		{
			list = new List<ValueTypeInfo>();
			foreach (AlarmPriority alarmPriority in alarmPriorities)
			{
				list.Add(new ValueTypeInfo
				{
					Name = alarmPriority.Name,
					Value = alarmPriority.Id.ToString()
				});
			}
		}
		return new Property("Priority", val)
		{
			ValueType = "Enum",
			IsSettable = isSettable,
			DisplayName = ConfigurationApiResources.PropertyAlarmDefinitionPriority,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyAlarmDefinitionPriority",
			ToolTipTranslationId = "PropertyAlarmDefinitionPriorityTip",
			UIImportance = UIImportance.Summary,
			SortKey = 100,
			ValueTypeInfos = list.ToArray()
		};
	}

	public static Property AlarmCategoryProperty(Guid val, bool isSettable, AlarmCategory[] alarmCategories)
	{
		List<ValueTypeInfo> list = null;
		if (isSettable)
		{
			list = new List<ValueTypeInfo>();
			list.Add(new ValueTypeInfo
			{
				Name = "",
				Value = Guid.Empty.ToString()
			});
			foreach (AlarmCategory alarmCategory in alarmCategories)
			{
				list.Add(new ValueTypeInfo
				{
					Name = alarmCategory.Name,
					Value = alarmCategory.Id.ToString()
				});
			}
		}
		return new Property("Category", val.ToString())
		{
			ValueType = "Enum",
			IsSettable = isSettable,
			DisplayName = ConfigurationApiResources.PropertyAlarmDefinitionCategory,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyAlarmDefinitionCategory",
			ToolTipTranslationId = "PropertyAlarmDefinitionCategoryTip",
			UIImportance = UIImportance.Summary,
			SortKey = 100,
			ValueTypeInfos = list.ToArray()
		};
	}

	public static Property OwnerProperty(string val, bool isSettable)
	{
		return new Property("Owner", val)
		{
			ValueType = "String",
			IsSettable = isSettable,
			DisplayName = ConfigurationApiResources.PropertyAlarmDefinitionOwner,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyAlarmDefinitionOwner",
			ToolTipTranslationId = "PropertyAlarmDefinitionOwnerTip",
			UIImportance = UIImportance.Summary,
			SortKey = 100
		};
	}

	public static Property RelatedMapProperty(string val, bool isSettable, Dictionary<string, MapUpdateData> mapData)
	{
		List<ValueTypeInfo> list = null;
		if (isSettable)
		{
			list = new List<ValueTypeInfo>();
			list.Add(new ValueTypeInfo
			{
				Name = "",
				Value = Guid.Empty.ToString()
			});
			foreach (MapUpdateData value in mapData.Values)
			{
				list.Add(new ValueTypeInfo
				{
					Name = value.Name,
					Value = value.Id
				});
			}
		}
		return new Property("RelatedMap", val)
		{
			ValueType = "Enum",
			IsSettable = isSettable,
			DisplayName = ConfigurationApiResources.PropertyAlarmDefinitionRelatedMap,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyAlarmDefinitionRelatedMap",
			ToolTipTranslationId = "PropertyAlarmDefinitionRelatedMapTip",
			UIImportance = UIImportance.Summary,
			SortKey = 100,
			ValueTypeInfos = list.ToArray()
		};
	}

	public static Property MapTypeProperty(int val, bool isSettable)
	{
		List<ValueTypeInfo> list = null;
		if (isSettable)
		{
			list = new List<ValueTypeInfo>();
			list.Add(new ValueTypeInfo
			{
				Name = "None",
				Value = "0"
			});
			list.Add(new ValueTypeInfo
			{
				Name = "Map",
				Value = "1"
			});
			list.Add(new ValueTypeInfo
			{
				Name = "Smart Map",
				Value = "2"
			});
		}
		return new Property("MapType", val.ToString(CultureInfo.InvariantCulture))
		{
			ValueType = "Enum",
			IsSettable = isSettable,
			DisplayName = ConfigurationApiResources.PropertyAlarmDefinitionMapType,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyAlarmDefinitionMapType",
			UIImportance = UIImportance.Summary,
			SortKey = 100,
			ValueTypeInfos = list.ToArray()
		};
	}

	public static Property EnableRuleProperty(EnableRule val, bool isSettable)
	{
		List<ValueTypeInfo> list = null;
		if (isSettable)
		{
			list = new List<ValueTypeInfo>
			{
				new ValueTypeInfo
				{
					Name = "Always",
					Value = "0"
				},
				new ValueTypeInfo
				{
					Name = "TimeProfile",
					Value = "1"
				},
				new ValueTypeInfo
				{
					Name = "EventTriggered",
					Value = "2"
				}
			};
		}
		int num = (int)val;
		return new Property("EnableRule", num.ToString(CultureInfo.InvariantCulture))
		{
			ValueType = "Enum",
			IsSettable = isSettable,
			DisplayName = ConfigurationApiResources.PropertyAlarmDefinitionEnableRule,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyAlarmDefinitionEnableRule",
			ToolTipTranslationId = "PropertyAlarmDefinitionEnableRuleTip",
			UIImportance = UIImportance.Summary,
			SortKey = 100,
			ValueTypeInfos = list.ToArray()
		};
	}

	public static Property EnableEventListProperty(string val, bool isSettable)
	{
		Property property = new Property("EnableEventList", val);
		property.ValueType = "PathList";
		property.IsSettable = isSettable;
		property.DisplayName = ConfigurationApiResources.PropertyAlarmDefinitionEnableEventList;
		property.ItemReference = true;
		property.ServerValidation = false;
		property.TranslationId = "PropertyAlarmDefinitionEnableEventList";
		property.ToolTipTranslationId = "PropertyAlarmDefinitionEnableEventListTip";
		property.UIImportance = UIImportance.Summary;
		property.SortKey = 100;
		property.ValueTypeInfos = new ValueTypeInfo[2]
		{
			new ValueTypeInfo
			{
				Name = "PathItemType",
				Value = "UserDefinedEvent"
			},
			new ValueTypeInfo
			{
				Name = "PathItemType",
				Value = "InputEvent"
			}
		};
		return property;
	}

	public static Property RelatedCameraListProperty(string val, bool isSettable)
	{
		Property property = new Property("RelatedCameraList", val);
		property.ValueType = "PathList";
		property.IsSettable = isSettable;
		property.DisplayName = ConfigurationApiResources.PropertyAlarmDefinitionRelatedCameraList;
		property.ItemReference = true;
		property.ServerValidation = false;
		property.TranslationId = "PropertyAlarmDefinitionRelatedCameraList";
		property.ToolTipTranslationId = "PropertyAlarmDefinitionRelatedCameraListTip";
		property.UIImportance = UIImportance.Summary;
		property.SortKey = 100;
		property.ValueTypeInfos = new ValueTypeInfo[1]
		{
			new ValueTypeInfo
			{
				Name = "PathItemType",
				Value = "Camera"
			}
		};
		return property;
	}

	public static Property DisableEventListProperty(string val, bool isSettable)
	{
		Property property = new Property("DisableEventList", val);
		property.ValueType = "PathList";
		property.IsSettable = isSettable;
		property.DisplayName = ConfigurationApiResources.PropertyAlarmDefinitionDisableEventList;
		property.ItemReference = true;
		property.ServerValidation = false;
		property.TranslationId = "PropertyAlarmDefinitionDisableEventList";
		property.ToolTipTranslationId = "PropertyAlarmDefinitionDisableEventListTip";
		property.UIImportance = UIImportance.Summary;
		property.SortKey = 100;
		property.ValueTypeInfos = new ValueTypeInfo[2]
		{
			new ValueTypeInfo
			{
				Name = "PathItemType",
				Value = "UserDefinedEvent"
			},
			new ValueTypeInfo
			{
				Name = "PathItemType",
				Value = "InputEvent"
			}
		};
		return property;
	}

	public static Property TimeLimitProperty(TimeSpan timeSpan, bool isSettable)
	{
		return new Property("ManagementTimeoutTime", timeSpan.ToString())
		{
			ValueType = "String",
			IsSettable = isSettable,
			DisplayName = ConfigurationApiResources.PropertyAlarmDefinitionManagementTimeoutTime,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyAlarmDefinitionManagementTimeoutTime",
			ToolTipTranslationId = "PropertyAlarmDefinitionManagementTimeoutTimeTip",
			UIImportance = UIImportance.Summary,
			SortKey = 121
		};
	}

	public static Property ManagementTimeOutEventListProperty(string val, bool isSettable)
	{
		Property property = new Property("ManagementTimeoutEventList", val);
		property.ValueType = "PathList";
		property.IsSettable = isSettable;
		property.DisplayName = ConfigurationApiResources.PropertyAlarmDefinitionManagementTimeOutEventList;
		property.ItemReference = true;
		property.ServerValidation = false;
		property.TranslationId = "PropertyAlarmDefinitionManagementTimeOutEventList";
		property.ToolTipTranslationId = "PropertyAlarmDefinitionManagementTimeOutEventListTip";
		property.UIImportance = UIImportance.Summary;
		property.SortKey = 100;
		property.ValueTypeInfos = new ValueTypeInfo[2]
		{
			new ValueTypeInfo
			{
				Name = "PathItemType",
				Value = "UserDefinedEvent"
			},
			new ValueTypeInfo
			{
				Name = "PathItemType",
				Value = "Output"
			}
		};
		return property;
	}

	public static Property TriggerEventListProperty(string val, bool isSettable)
	{
		Property property = new Property("TriggerEventlist", val);
		property.ValueType = "PathList";
		property.IsSettable = isSettable;
		property.DisplayName = ConfigurationApiResources.PropertyAlarmDefinitionTriggerEventList;
		property.ItemReference = true;
		property.ServerValidation = false;
		property.TranslationId = "PropertyAlarmDefinitionTriggerEventList";
		property.ToolTipTranslationId = "PropertyAlarmDefinitionTriggerEventListTip";
		property.UIImportance = UIImportance.Summary;
		property.SortKey = 100;
		property.ValueTypeInfos = new ValueTypeInfo[2]
		{
			new ValueTypeInfo
			{
				Name = "PathItemType",
				Value = "UserDefinedEvent"
			},
			new ValueTypeInfo
			{
				Name = "PathItemType",
				Value = "Output"
			}
		};
		return property;
	}

	public static Property TimeProfileProperty(string val, bool isSettable)
	{
		if (!string.IsNullOrEmpty(val))
		{
			val = ConfigurationItemPath.CreateItemPath("TimeProfile", val).Text;
		}
		Property property = new Property("TimeProfile", val);
		property.ValueType = "Path";
		property.IsSettable = isSettable;
		property.DisplayName = ConfigurationApiResources.PropertyAlarmDefinitionTimeProfile;
		property.ItemReference = true;
		property.ServerValidation = false;
		property.TranslationId = "PropertyAlarmDefinitionTimeProfile";
		property.ToolTipTranslationId = "PropertyAlarmDefinitionTimeProfileTip";
		property.UIImportance = UIImportance.Summary;
		property.SortKey = 100;
		property.ValueTypeInfos = new ValueTypeInfo[1]
		{
			new ValueTypeInfo
			{
				Name = "PathItemType",
				Value = "TimeProfile"
			}
		};
		return property;
	}

	public static Property AutoCloseProperty(bool val, bool isSettable, string messageName)
	{
		return new Property("AutoClose", val.ToString(CultureInfo.InvariantCulture))
		{
			ValueType = "Tick",
			IsSettable = isSettable,
			DisplayName = string.Format(ConfigurationApiResources.PropertyAlarmDefinitionAutoClose, messageName),
			ItemReference = true,
			ServerValidation = false,
			TranslationId = "PropertyAlarmDefinitionAutoClose",
			ToolTipTranslationId = "PropertyAlarmDefinitionAutoCloseTip",
			UIImportance = UIImportance.Summary,
			SortKey = 100
		};
	}

	public static Property AssignableToAdminsProperty(bool val, bool isSettable)
	{
		return new Property("AssignableToAdmins", val.ToString(CultureInfo.InvariantCulture))
		{
			ValueType = "Tick",
			IsSettable = isSettable,
			DisplayName = ConfigurationApiResources.PropertyAlarmDefinitionAssignableToAdmins,
			ItemReference = true,
			ServerValidation = false,
			TranslationId = "PropertyAlarmDefinitionAssignableToAdmins",
			ToolTipTranslationId = "PropertyAlarmDefinitionAssignableToAdminsTip",
			UIImportance = UIImportance.Summary,
			SortKey = 100
		};
	}
}
