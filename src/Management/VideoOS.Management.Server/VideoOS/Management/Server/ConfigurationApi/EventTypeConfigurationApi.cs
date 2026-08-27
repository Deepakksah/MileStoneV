using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Common;
using VideoOS.Management.Server.BusinessObjects.EventTypes;
using VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPIEventTypeGroupFolder")]
[EntityName("ConfigAPIEventTypeGroup")]
[EntityName("ConfigAPIEventTypeFolder")]
[EntityName("ConfigAPIEventType")]
public class EventTypeConfigurationApi : ConfigurationApiBase, IConfigurationApiSupport
{
	private static readonly Guid AnalyticsPluginId = new Guid("9F6E7E9D-3FD2-49B2-9CA0-F7E7F8F8C3A7");

	private static readonly Guid DevicePredefined = new Guid("1AE79228-AD07-4E56-BE43-5EBF9B7C54A5");

	private static readonly Guid DeviceConfigurable = new Guid("44D45C4D-5B57-4F21-8629-CD2F9D3CE5DE");

	private static readonly Guid HardwareConfigurable = new Guid("EC4C53B4-0441-4811-B283-644E3B21F05B");

	private static readonly Guid HardwarePredefined = new Guid("E1391B62-2BD2-48DF-86E8-0EAAB6E1798A");

	private static readonly Guid ExternalUserDefined = new Guid("0D95643A-7C6A-4A68-8239-F78B109B47C2");

	private static readonly Guid ExternalGeneric = new Guid("FD95643A-7C6A-4A68-8239-F78B109B47CF");

	private static readonly Guid ExternalSystem = new Guid("10FFBC6A-DD7B-4F74-8A37-5A3B2872B5FC");

	private static readonly Guid RecordingServerEvents = new Guid("365E2FC3-D6C1-414A-A05B-E6BB3DC2D7CD");

	private static readonly Guid OtherEvents = new Guid("7086E26E-AB37-4CEF-AE91-5DE43DCF7CD9");

	private static readonly Guid MSFailoverEvents = new Guid("7286E26E-AB37-4CEF-AE91-5DE43DCF7CD9");

	private static readonly Guid AccessControlEvents = new Guid("7E4893E3-4590-4073-9AA3-3CA8E2129402");

	private static readonly Guid AccessControlCategories = new Guid("8D09E754-2BAD-46D2-9BF6-3B292A424B38");

	private static readonly Guid SystemMonitorDisk = new Guid("3F28F187-F1C4-414B-9CA1-C90F59EFB2F5");

	private static readonly Guid SystemMonitorCamera = new Guid("60C7D021-4096-429B-B281-CE9A4FF05040");

	private static readonly Guid SystemMonitorStorage = new Guid("11323282-85D7-43B2-97B0-841A171AEBDE");

	private static readonly Guid SystemMonitorServer = new Guid("98927BBC-E8F7-4041-8727-37EF2D6D1D41");

	private static readonly Guid SystemEvents = new Guid("518963C3-A795-4E07-9DCC-271612D0AC67");

	private static readonly Guid LprPluginId = new Guid("6f714690-e09f-4d6a-a0ca-9f913ca125e8");

	private static readonly Guid LprServerEventGroupId = new Guid("6530afe1-bd9d-447c-a796-380cbad2d11c");

	private static Dictionary<Guid, EventTypeGroupResources> eventTypeGroups = new Dictionary<Guid, EventTypeGroupResources>
	{
		{
			DevicePredefined,
			new EventTypeGroupResources("EventTypeDevicePredefined", GeneratorType.Device, "")
		},
		{
			DeviceConfigurable,
			new EventTypeGroupResources("EventTypeDeviceConfigurable", GeneratorType.DriverDevice, "")
		},
		{
			HardwareConfigurable,
			new EventTypeGroupResources("EventTypeHardwareConfigurable", GeneratorType.DriverHardware, "")
		},
		{
			HardwarePredefined,
			new EventTypeGroupResources("EventTypeHardwarePredefined", GeneratorType.Hardware, "")
		},
		{
			ExternalUserDefined,
			new EventTypeGroupResources("ItemTypeExternalUserDefinedEvent", GeneratorType.External, "UserDefined")
		},
		{
			ExternalGeneric,
			new EventTypeGroupResources("ItemTypeExternalGenericEventFolder", GeneratorType.External, "Generic")
		},
		{
			ExternalSystem,
			new EventTypeGroupResources("EventTypeExternalSystem", GeneratorType.External, "System")
		},
		{
			AnalyticsPluginId,
			new EventTypeGroupResources("ItemTypeAnalyticFolder", GeneratorType.MIPDevice, AnalyticsPluginId.ToString())
		},
		{
			RecordingServerEvents,
			new EventTypeGroupResources("EventTypeRecordingServerEvents", GeneratorType.Recorder, "")
		},
		{
			OtherEvents,
			new EventTypeGroupResources("EventTypeOtherEvents", GeneratorType.Server, "")
		},
		{
			MSFailoverEvents,
			new EventTypeGroupResources("EventTypeMSFailoverEvents", GeneratorType.Server, "MSFailover")
		},
		{
			AccessControlEvents,
			new EventTypeGroupResources("EventTypeAccessControlEvents", GeneratorType.MIP, "")
		},
		{
			AccessControlCategories,
			new EventTypeGroupResources("EventTypeAccessControlCategories", GeneratorType.MIP, "")
		},
		{
			SystemMonitorDisk,
			new EventTypeGroupResources("EventTypeSystemMonitorDisk", GeneratorType.SystemDashboard, "Disk")
		},
		{
			SystemMonitorCamera,
			new EventTypeGroupResources("EventTypeSystemMonitorCamera", GeneratorType.SystemDashboard, "Device")
		},
		{
			SystemMonitorStorage,
			new EventTypeGroupResources("EventTypeSystemMonitorStorage", GeneratorType.SystemDashboard, "Storage")
		},
		{
			SystemMonitorServer,
			new EventTypeGroupResources("EventTypeSystemMonitorServer", GeneratorType.SystemDashboard, "Server")
		},
		{
			SystemEvents,
			new EventTypeGroupResources("EventTypeSystem", GeneratorType.System, "")
		}
	};

	public ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		throw new NotSupportedException();
	}

	public ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		throw new NotSupportedException();
	}

	public ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, string methodId)
	{
		throw new NotSupportedException();
	}

	public ConfigurationItem[] GetChildItems(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		if (path.IsFolder)
		{
			ConfigurationItemPath parentPath = ConfigurationItemPath.CreateFolderPath(ConfigurationItemPath.CreateSystemPath(), "EventTypeGroupFolder");
			List<ConfigurationItem> list = new List<ConfigurationItem>();
			string itemType = path.ItemType;
			if (!(itemType == "EventTypeGroupFolder"))
			{
				if (!(itemType == "EventTypeFolder"))
				{
					throw new PathInvalidException();
				}
				EventTypeGroupResources value;
				if (!Guid.TryParse(path.Id, out var result))
				{
					list.AddRange(ConstructItems(requestContext, "EventType"));
				}
				else if (result == AnalyticsPluginId)
				{
					list.AddRange(ConstructItems(requestContext, "EventType", path, GeneratorType.MIPDevice, "", path.Id.ToString()));
				}
				else if (eventTypeGroups.TryGetValue(result, out value) && value.GeneratorType != GeneratorType.MIP)
				{
					list.AddRange(ConstructItems(requestContext, "EventType", path, value.GeneratorType, value.GeneratorSubType));
				}
				else if (result == AccessControlEvents)
				{
					list.AddRange(ConstructItems(requestContext, "EventType", path, GeneratorType.MIP, "", SourceGuid.AccessControl.ToString(), AlarmDefinitionConstants.AccessControlEventGroup));
				}
				else if (result == AccessControlCategories)
				{
					list.AddRange(ConstructItems(requestContext, "EventType", path, GeneratorType.MIP, "", SourceGuid.AccessControl.ToString(), AlarmDefinitionConstants.AccessControlCategoryEventGroup));
				}
				else
				{
					if (result == LprPluginId)
					{
						result = LprServerEventGroupId;
						path = ConfigurationItemPath.CreateFolderPath("EventTypeGroup", result.ToString(), "EventTypeFolder");
					}
					list.AddRange(ConstructItems(requestContext, "EventType", path, new string[2]
					{
						GeneratorType.MIP.ToString(),
						GeneratorType.MIPDevice.ToString()
					}, "", "", result));
				}
			}
			else
			{
				foreach (KeyValuePair<Guid, EventTypeGroupResources> eventTypeGroup in eventTypeGroups)
				{
					list.Add(BuildGroup("EventTypeGroup", eventTypeGroup.Key, eventTypeGroup.Value.ResourceId, path));
				}
				list.AddRange(ConstructMIPEventGroups(requestContext, parentPath));
			}
			return list.ToArray();
		}
		List<ConfigurationItem> list2 = new List<ConfigurationItem>();
		if (!Guid.TryParse(path.Id, out var result2))
		{
			throw new PathInvalidException();
		}
		if (path.ItemType == "EventType")
		{
			return list2.ToArray();
		}
		bool flag = false;
		if (eventTypeGroups.ContainsKey(result2))
		{
			if (result2 == ExternalUserDefined)
			{
				list2.Add(GroupConfigurationItemFactory.CreateFolderItem("UserDefinedEventFolder", ConfigurationItemPath.CreateSystemPath()));
			}
			else if (result2 == ExternalGeneric)
			{
				list2.Add(GroupConfigurationItemFactory.CreateFolderItem("GenericEventFolder", ConfigurationItemPath.CreateSystemPath()));
			}
			else if (result2 == AnalyticsPluginId)
			{
				list2.Add(GroupConfigurationItemFactory.CreateFolderItem("AnalyticsEventFolder", ConfigurationItemPath.CreateSystemPath()));
			}
			else
			{
				list2.Add(BuildFolder("EventTypeFolder", path));
			}
			flag = true;
		}
		if (!flag && GetMIPEventGroups(requestContext).ContainsKey(result2))
		{
			list2.Add(BuildFolder("EventTypeFolder", path));
		}
		return list2.ToArray();
	}

	public ConfigurationItem GetItem(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		ConfigurationItemPath parentPath = ConfigurationItemPath.CreateSystemPath();
		ConfigurationItemPath parentPath2 = ConfigurationItemPath.CreateFolderPath(parentPath, "EventTypeGroupFolder");
		if (path.IsFolder)
		{
			if (path.ItemType == "EventTypeGroupFolder")
			{
				return BuildFolder(path.ItemType, parentPath);
			}
			if (!Guid.TryParse(path.Id, out var result))
			{
				return BuildFolder(path.ItemType, parentPath);
			}
			ConfigurationItemPath parentPath3 = ConfigurationItemPath.CreateItemPath(path.ParentItemType, path.Id);
			if (GetMIPEventGroups(requestContext).ContainsKey(result))
			{
				return CreateMIPPluginFolder(GetMIPEventType(requestContext, result) ?? throw new PathNotFoundException(path.Text), parentPath3);
			}
			if (eventTypeGroups.TryGetValue(result, out var _))
			{
				return BuildFolder(path.ItemType, parentPath3);
			}
			throw new PathNotFoundException();
		}
		if (!Guid.TryParse(path.Id, out var result2))
		{
			throw new PathInvalidException();
		}
		if (path.ItemType == "EventTypeGroup")
		{
			if (eventTypeGroups.ContainsKey(result2))
			{
				return BuildGroup("EventTypeGroup", result2, eventTypeGroups[result2].ResourceId, parentPath2);
			}
			if (result2 == LprPluginId)
			{
				result2 = LprServerEventGroupId;
			}
			Dictionary<Guid, string> mIPEventGroups = GetMIPEventGroups(requestContext);
			if (mIPEventGroups.ContainsKey(result2))
			{
				return BuildGroup("EventTypeGroup", result2, mIPEventGroups[result2], parentPath2);
			}
		}
		else
		{
			EventTypeInfo eventType = EventType.GetEventType(requestContext, result2);
			if (eventType == null)
			{
				throw new PathNotFoundException();
			}
			ConfigurationItemPath configurationItemPath = null;
			EventDefinition definition = new EventDefinition(eventType.Definition);
			Guid guid = LookupParentId(eventType, definition);
			if (guid != Guid.Empty)
			{
				configurationItemPath = ConfigurationItemPath.CreateFolderPath("EventTypeGroup", guid.ToString(), "EventTypeFolder");
				return ConstructConfigurationItem("EventType", configurationItemPath, eventType);
			}
		}
		throw new PathInvalidException(path.Text);
	}

	private static Guid LookupParentId(EventTypeInfo eventTypeInfo, EventDefinition definition)
	{
		Guid result = Guid.Empty;
		IEnumerable<KeyValuePair<Guid, EventTypeGroupResources>> source = eventTypeGroups.Where((KeyValuePair<Guid, EventTypeGroupResources> g) => g.Value.GeneratorType.ToString() == eventTypeInfo.GeneratorType && g.Value.GeneratorType != GeneratorType.MIP && g.Value.GeneratorSubType == eventTypeInfo.GeneratorSubType);
		if (source.Any())
		{
			result = source.First().Key;
		}
		else if (eventTypeInfo.GeneratorType == GeneratorType.MIP.ToString())
		{
			Guid guid = new Guid(definition.GeneratorGroupId);
			result = ((guid == AlarmDefinitionConstants.AccessControlEventGroup) ? AccessControlEvents : ((!(guid == AlarmDefinitionConstants.AccessControlCategoryEventGroup)) ? guid : AccessControlCategories));
		}
		else if (eventTypeInfo.GeneratorType == GeneratorType.MIPDevice.ToString())
		{
			result = ((!(new Guid(definition.GeneratorID) == LprPluginId)) ? new Guid(definition.GeneratorID) : new Guid(definition.GeneratorGroupId));
		}
		else if (eventTypeInfo.GeneratorType == GeneratorType.RecorderPlugin.ToString())
		{
			result = DevicePredefined;
		}
		return result;
	}

	private static ConfigurationItem BuildGroup(string itemType, Guid id, string resourceId, ConfigurationItemPath parentPath)
	{
		return GroupConfigurationItemFactory.CreateGroupItem(itemType, id, resourceId, parentPath);
	}

	private static ConfigurationItem BuildFolder(string itemType, ConfigurationItemPath parentPath)
	{
		return GroupConfigurationItemFactory.CreateFolderItem(itemType, parentPath);
	}

	private static List<ConfigurationItem> ConstructItems(IGeminiRequestContext requestContext, string itemType, ConfigurationItemPath parentPath, GeneratorType generator, string generatorSubType, string idGenerator = null)
	{
		return ConstructItems(requestContext, itemType, parentPath, generator, generatorSubType, idGenerator, Guid.Empty);
	}

	private static List<ConfigurationItem> ConstructItems(IGeminiRequestContext requestContext, string itemType, ConfigurationItemPath parentPath, GeneratorType generator, string generatorSubType, string idGenerator, Guid eventGroup)
	{
		string[] generatorList = ((generator != GeneratorType.Device) ? new string[1] { generator.ToString() } : new string[2]
		{
			GeneratorType.Device.ToString(),
			GeneratorType.RecorderPlugin.ToString()
		});
		return ConstructItems(requestContext, itemType, parentPath, generatorList, generatorSubType, idGenerator, eventGroup);
	}

	private static List<ConfigurationItem> ConstructItems(IGeminiRequestContext requestContext, string itemType, ConfigurationItemPath parentPath, string[] generatorList, string generatorSubType, string idGenerator, Guid eventGroup)
	{
		List<ConfigurationItem> list = new List<ConfigurationItem>();
		foreach (EventTypeInfo eventType in EventType.GetEventTypes(requestContext, generatorList, idGenerator))
		{
			EventDefinition eventDefinition = new EventDefinition(eventType.Definition);
			if (eventType.GeneratorSubType == generatorSubType && (eventGroup == Guid.Empty || eventGroup == new Guid(eventDefinition.GeneratorGroupId)))
			{
				list.Add(ConstructConfigurationItem(itemType, parentPath, eventType));
			}
		}
		return list;
	}

	private static List<ConfigurationItem> ConstructItems(IGeminiRequestContext requestContext, string itemType)
	{
		List<ConfigurationItem> list = new List<ConfigurationItem>();
		List<string> list2 = new List<string>();
		foreach (object value in Enum.GetValues(typeof(GeneratorType)))
		{
			list2.Add(value.ToString());
		}
		foreach (EventTypeInfo eventType in EventType.GetEventTypes(requestContext, list2.ToArray(), null))
		{
			EventDefinition definition = new EventDefinition(eventType.Definition);
			Guid guid = LookupParentId(eventType, definition);
			if (guid != Guid.Empty)
			{
				ConfigurationItemPath parentPath = ConfigurationItemPath.CreateFolderPath("EventTypeGroup", guid.ToString(), "EventTypeFolder");
				list.Add(ConstructConfigurationItem(itemType, parentPath, eventType));
			}
		}
		return list;
	}

	private static List<ConfigurationItem> ConstructMIPEventGroups(IGeminiRequestContext requestContext, ConfigurationItemPath parentPath)
	{
		Dictionary<Guid, ConfigurationItem> dictionary = new Dictionary<Guid, ConfigurationItem>();
		foreach (EventTypeInfo eventType in EventType.GetEventTypes(requestContext, new string[2]
		{
			GeneratorType.MIP.ToString(),
			GeneratorType.MIPDevice.ToString()
		}, null))
		{
			Guid iDGenerator = eventType.IDGenerator;
			if (iDGenerator != SourceGuid.AccessControl && iDGenerator != AnalyticsPluginId && Guid.TryParse(new EventDefinition(eventType.Definition).GeneratorGroupId, out var result) && !dictionary.ContainsKey(result))
			{
				ConfigurationItem value = CreateMIPPluginGroup(eventType, result, parentPath);
				dictionary.Add(result, value);
			}
		}
		return dictionary.Values.ToList();
	}

	private static ConfigurationItem CreateMIPPluginGroup(EventTypeInfo eventType, Guid groupId, ConfigurationItemPath parentPath)
	{
		EventDefinition eventDefinition = new EventDefinition(eventType.Definition);
		ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreateItemPath("EventTypeGroup", groupId.ToString());
		return new ConfigurationItem
		{
			ItemCategory = ItemCategories.Item,
			ItemType = "EventTypeGroup",
			Path = configurationItemPath.Text,
			DisplayName = eventDefinition.GeneratorGroupName,
			ParentPath = parentPath.Text,
			UIHint = "DirectionHorizontal"
		};
	}

	private static ConfigurationItem CreateMIPPluginFolder(EventTypeInfo eventType, ConfigurationItemPath parentPath)
	{
		ConfigurationItemPath configurationItemPath = new ConfigurationItemPathBuilder(parentPath, "EventTypeFolder").CreatePath();
		EventDefinition eventDefinition = new EventDefinition(eventType.Definition);
		return new ConfigurationItem
		{
			ItemCategory = ItemCategories.Group,
			ItemType = "EventTypeFolder",
			Path = configurationItemPath.Text,
			DisplayName = eventDefinition.GeneratorName,
			ParentPath = parentPath.Text,
			UIHint = "DirectionHorizontal"
		};
	}

	private static ConfigurationItem ConstructConfigurationItem(string itemType, ConfigurationItemPath parentPath, EventTypeInfo eventTypeInfo)
	{
		ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreateItemPath(itemType, eventTypeInfo.IDEventType.ToString());
		EventDefinition eventDefinition = new EventDefinition(eventTypeInfo.Definition);
		ConfigurationItem configurationItem = new ConfigurationItem
		{
			UIHint = "DirectionHorizontal",
			DisplayName = eventDefinition.DisplayName,
			ItemType = configurationItemPath.ItemType,
			ItemCategory = ItemCategories.Item,
			ParentPath = parentPath.Text,
			Path = configurationItemPath.Text,
			ChildrenFilled = true
		};
		List<string> list = new List<string>();
		List<string> list2 = new List<string>();
		EventSource[] sources = eventDefinition.Sources;
		foreach (EventSource eventSource in sources)
		{
			string text = "";
			if (eventDefinition.GeneratorType == GeneratorType.MIP.ToString())
			{
				list.Add("MIPItem");
				list2.Add(string.Format(CultureInfo.InvariantCulture, "{0}", eventSource.Type));
			}
			else
			{
				List<string> list3 = ConvertSourceTypeToItemTypes(eventSource.Type);
				list.AddRange(list3);
				if (list3.Count > 1)
				{
					list2.Add(string.Empty);
				}
			}
			foreach (EventSourceFilter filter in eventSource.Filters)
			{
				text = ((filter.FilterType != EventSource.FilterTypes.MIP) ? (text + string.Format(CultureInfo.InvariantCulture, "{0},{1}", filter.FilterType, filter.Value)) : (text + string.Format(CultureInfo.InvariantCulture, "{0}", filter.Value)));
			}
			list2.Add(text);
		}
		List<Property> list4 = new List<Property>();
		list4.Add(GeneralPropertyDefinitions.GetIdPropertyGuid(eventTypeInfo.IDEventType));
		list4.Add(GeneralPropertyDefinitions.GetNameProperty(eventTypeInfo.Name, isSettable: false));
		list4.Add(GeneralPropertyDefinitions.GetLastModifiedProperty(eventTypeInfo.LastModified));
		list4.Add(EventTypeProperties.GetDescriptionProperty(eventDefinition.Description));
		list4.Add(EventTypeProperties.GetGeneratorTypeProperty(eventDefinition.GeneratorType));
		list4.Add(EventTypeProperties.GetGeneratorSubtypeProperty(eventDefinition.GeneratorSubtype));
		list4.Add(EventTypeProperties.GetGeneratorNameProperty(eventDefinition.GeneratorName));
		list4.Add(EventTypeProperties.GetGeneratorIDProperty(eventDefinition.GeneratorID));
		list4.Add(EventTypeProperties.GetGeneratorGroupNameProperty(eventDefinition.GeneratorGroupName));
		list4.Add(EventTypeProperties.GetGeneratorGroupIdProperty(eventDefinition.GeneratorGroupId));
		list4.Add(EventTypeProperties.GetOccursGloballyProperty(eventDefinition.OccursGlobally));
		list4.Add(EventTypeProperties.GetBuiltInProperty(eventDefinition.BuiltIn));
		list4.Add(EventTypeProperties.GetCounterEventIDProperty(eventDefinition.CounterEventID.ToString()));
		list4.Add(EventTypeProperties.GetSourceArrayProperty(list.ToArray()));
		list4.Add(EventTypeProperties.GetSourceFilterArrayProperty(list2.ToArray()));
		list4.Add(EventTypeProperties.GetStateGroupId(eventDefinition.StateGroupId));
		list4.Add(EventTypeProperties.GetState(eventDefinition.State));
		configurationItem.Properties = list4.ToArray();
		return configurationItem;
	}

	private static List<string> ConvertSourceTypeToItemTypes(string sourceIn)
	{
		List<string> list = EventTypeProxy.GetItemTypesFromEventSource(sourceIn).ToList();
		if (list.Count == 1 && list[0] == "External")
		{
			list[0] = "UserDefinedEvent";
		}
		return list;
	}

	private static Dictionary<Guid, string> GetMIPEventGroups(IGeminiRequestContext requestContext)
	{
		IEnumerable<EventTypeInfo> eventTypes = EventType.GetEventTypes(requestContext, new string[2]
		{
			GeneratorType.MIP.ToString(),
			GeneratorType.MIPDevice.ToString()
		}, null);
		Dictionary<Guid, string> dictionary = new Dictionary<Guid, string>();
		foreach (EventTypeInfo item in eventTypes)
		{
			Guid iDGenerator = item.IDGenerator;
			if (iDGenerator != SourceGuid.AccessControl && iDGenerator != AnalyticsPluginId)
			{
				EventDefinition eventDefinition = new EventDefinition(item.Definition);
				if (Guid.TryParse(eventDefinition.GeneratorGroupId, out var result) && !dictionary.ContainsKey(result))
				{
					dictionary[result] = eventDefinition.GeneratorGroupName;
				}
			}
		}
		return dictionary;
	}

	private static EventTypeInfo GetMIPEventType(IGeminiRequestContext requestContext, Guid pluginId)
	{
		IEnumerable<EventTypeInfo> eventTypes = EventType.GetEventTypes(requestContext, new string[2]
		{
			GeneratorType.MIP.ToString(),
			GeneratorType.MIPDevice.ToString()
		}, pluginId.ToString().ToUpperInvariant());
		if (eventTypes != null && eventTypes.FirstOrDefault() != null)
		{
			return eventTypes.FirstOrDefault();
		}
		return null;
	}
}
