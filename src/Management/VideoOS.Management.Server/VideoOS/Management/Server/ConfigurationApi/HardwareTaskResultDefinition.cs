using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Common.Tasks;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class HardwareTaskResultDefinition
{
	public const string RecordingServerId = "RecordingServerId";

	public const string HardwareName = "HardwareName";

	public const string HardwareId = "HardwareId";

	public const string HardwareChanged = "HardwareChanged";

	public const string NewFirmwareVersion = "NewFirmwareVersion";

	public const string SessionDataId = "SessionDataId";

	private const string SettingsChanged = "SettingsChanged";

	private const string ItemType = "ItemType";

	public static Property[] CreateAddHardwareResultProperties(IGeminiRequestContext requestContext, Task task)
	{
		List<Property> list = new List<Property>();
		if (task.AsyncStatus.ExceptionStatus == ExceptionStatus.Succeeded)
		{
			AddHardwareTaskResult addHardwareTaskResult = AddHardwareTaskResult.FromXml(task.AsyncStatus.LastException);
			Hardware hardware = Hardware.QueryHardwareById(requestContext, addHardwareTaskResult.HardwareId.ToString());
			if (hardware == null)
			{
				throw new InternalServerException(string.Format(CultureInfo.InvariantCulture, "Unable to find newly added hardware, id is: {0}", addHardwareTaskResult.HardwareId));
			}
			ConfigurationItemPath path = ConfigurationItemPath.CreateItemPath("Hardware", hardware.Id);
			list.Add(TaskConfigurationApi.GetPath(path));
		}
		return list.ToArray();
	}

	public static Property[] CreateDeleteHardwareResultProperties(Task task)
	{
		List<Property> list = new List<Property>();
		if (task.AsyncStatus.ExceptionStatus == ExceptionStatus.Succeeded)
		{
			DeleteHardwareTaskResult deleteHardwareTaskResult = DeleteHardwareTaskResult.FromXml(task.AsyncStatus.LastException);
			ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreateItemPath("RecordingServer", deleteHardwareTaskResult.RecordingServerId.ToString());
			list.Add(PropertyRecordingServerId(configurationItemPath.Text));
			list.Add(PropertyHardwareName(deleteHardwareTaskResult.HardwareName));
		}
		return list.ToArray();
	}

	public static Property[] CreateUpdateHardwareResultProperties(Task task)
	{
		List<Property> list = new List<Property>();
		if (task.AsyncStatus.ExceptionStatus == ExceptionStatus.Succeeded)
		{
			UpdateHardwareTaskResult updateHardwareTaskResult = UpdateHardwareTaskResult.FromXml(task.AsyncStatus.LastException);
			ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreateItemPath("Hardware", updateHardwareTaskResult.HardwareId.ToString());
			list.Add(PropertyHardwareId(configurationItemPath.Text));
			list.Add(PropertyHardwareName(updateHardwareTaskResult.HardwareName));
			list.Add(PropertyHardwareEventsChanged(updateHardwareTaskResult.HardwareEventsChanged));
			list.Add(SessionDataIdProperty(updateHardwareTaskResult.SessionDataId, isSettable: false));
		}
		return list.ToArray();
	}

	public static Property[] CreateReplaceHardwareResultProperties(Task task)
	{
		List<Property> list = new List<Property>();
		if (task.AsyncStatus.ExceptionStatus == ExceptionStatus.Succeeded)
		{
			ReplaceHardwareTaskResult replaceHardwareTaskResult = ReplaceHardwareTaskResult.FromXml(task.AsyncStatus.LastException);
			ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreateItemPath("Hardware", replaceHardwareTaskResult.Hardware.Id.ToString());
			list.Add(PropertyHardwareId(configurationItemPath.Text));
			list.Add(PropertyHardwareName(replaceHardwareTaskResult.Hardware.Name));
		}
		return list.ToArray();
	}

	public static Property[] CreateFirmwareUpgradeResultProperties(Task task)
	{
		List<Property> list = new List<Property>();
		if (task.AsyncStatus.ExceptionStatus == ExceptionStatus.Succeeded)
		{
			FirmwareUpgradeTaskResult firmwareUpgradeTaskResult = FirmwareUpgradeTaskResult.FromXml(task.AsyncStatus.LastException);
			ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreateItemPath("Hardware", firmwareUpgradeTaskResult.HardwareId.ToString());
			list.Add(PropertyHardwareId(configurationItemPath.Text));
			list.Add(PropertyHardwareName(firmwareUpgradeTaskResult.HardwareName));
			list.Add(PropertyNewFirmwareVersion(firmwareUpgradeTaskResult.NewFirmwareVersion));
		}
		return list.ToArray();
	}

	private static Property PropertyRecordingServerId(string recorderId)
	{
		return new Property("RecordingServerId", recorderId)
		{
			ValueType = "String",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyRecordingServerId,
			ItemReference = true,
			ServerValidation = false,
			TranslationId = "PropertyRecordingServerId",
			ToolTipTranslationId = "PropertyRecordingServerIdToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 1
		};
	}

	private static Property PropertyHardwareName(string hardwareName)
	{
		return new Property("HardwareName", hardwareName)
		{
			ValueType = "String",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyHardwareName,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyHardwareName",
			ToolTipTranslationId = "PropertyHardwareNameToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 1
		};
	}

	private static Property PropertyHardwareId(string hardwarePath)
	{
		return new Property("HardwareId", hardwarePath)
		{
			ValueType = "Path",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyUpdateHardwareHardwareId,
			ItemReference = true,
			ServerValidation = false,
			TranslationId = "PropertyUpdateHardwareHardwareId",
			ToolTipTranslationId = "PropertyHardwareIdToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 1
		};
	}

	private static Property PropertyHardwareEventsChanged(bool hardwareChanged)
	{
		return new Property("HardwareChanged", hardwareChanged.ToString(CultureInfo.InvariantCulture))
		{
			ValueType = "Tick",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyUpdateHardwareEventsChanged,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyUpdateHardwareEventsChanged",
			ToolTipTranslationId = "PropertyHardwareEventsChangedToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 1
		};
	}

	public static Property SessionDataIdProperty(long sessionDataId, bool isSettable)
	{
		return new Property("SessionDataId", sessionDataId.ToString(CultureInfo.InvariantCulture))
		{
			ValueType = "Int",
			IsSettable = isSettable,
			UIImportance = UIImportance.Summary,
			DisplayName = ConfigurationApiResources.PropertyUpdateHardwareSessionDataId,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyUpdateHardwareSessionDataId",
			ToolTipTranslationId = "PropertyUpdateHardwareSessionDataIdToolTip"
		};
	}

	public static Property[] CreateMoveHardwareResultProperties(Task task)
	{
		List<Property> list = new List<Property>();
		if (task.AsyncStatus.ExceptionStatus == ExceptionStatus.Succeeded)
		{
			foreach (TaskWarning warning in MoveHardwareTaskResult.FromXml(task.AsyncStatus.LastException).Warnings)
			{
				string propertyName = "TaskWarning" + warning.Code.ToString(CultureInfo.InvariantCulture);
				list.Add(CreateMoveHardwareWarningProperty(propertyName, LookupWarning(warning)));
			}
		}
		return list.ToArray();
	}

	private static string LookupWarning(TaskWarning warning)
	{
		try
		{
			string format = ConfigurationApiResources.ResourceManager.GetString("PropertyHardwareWarning" + warning.Code.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);
			return string.Format(CultureInfo.InvariantCulture, format, warning.Arguments);
		}
		catch (MissingManifestResourceException)
		{
		}
		catch (MissingSatelliteAssemblyException)
		{
		}
		return string.Format(CultureInfo.InvariantCulture, warning.Message, warning.Arguments);
	}

	private static Property CreateMoveHardwareWarningProperty(string propertyName, string warningString)
	{
		return new Property(propertyName, warningString)
		{
			ValueType = "String",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyHardwareWarning,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyHardwareWarning",
			ToolTipTranslationId = "PropertyHardwareWarningToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 1
		};
	}

	public static ConfigurationItem[] CreateUpdateHardwareResultChildItems(Task task, string parentPath, ConfigurationItemPath path)
	{
		List<ConfigurationItem> list = new List<ConfigurationItem>();
		if (task.AsyncStatus.ExceptionStatus == ExceptionStatus.Succeeded)
		{
			UpdateHardwareTaskDetailResult[] taskDetails = UpdateHardwareTaskResult.FromXml(task.AsyncStatus.LastException).TaskDetails;
			foreach (UpdateHardwareTaskDetailResult updateHardwareTaskDetailResult in taskDetails)
			{
				List<Property> list2 = new List<Property>();
				string text = DeviceTypeRelation.TryGetItemType(updateHardwareTaskDetailResult.DeviceType);
				ConfigurationItemPath path2 = ConfigurationItemPath.CreateItemPath(text, updateHardwareTaskDetailResult.DeviceId.ToString());
				list2.Add(HardwarePropertyDefinition.DevicePathProperty(path2));
				list2.Add(HardwarePropertyDefinition.DeviceTypeProperty(text));
				list2.Add(HardwarePropertyDefinition.DeviceNameProperty(updateHardwareTaskDetailResult.DeviceName));
				list2.Add(HardwarePropertyDefinition.ChangeProperty(updateHardwareTaskDetailResult.Change));
				ConfigurationItem item = TaskConfigurationApi.ConstructTaskResult(list2, parentPath, path, new string[0]);
				list.Add(item);
			}
		}
		return list.ToArray();
	}

	public static ConfigurationItem[] CreateReplaceHardwareResultChildItems(Task task, ConfigurationItemPath path)
	{
		List<ConfigurationItem> list = new List<ConfigurationItem>();
		if (task.AsyncStatus.ExceptionStatus == ExceptionStatus.Succeeded)
		{
			ReplaceHardwareTaskResult replaceHardwareTaskResult = ReplaceHardwareTaskResult.FromXml(task.AsyncStatus.LastException);
			ConfigurationItem configurationItem = new ConfigurationItem();
			configurationItem.ItemCategory = ItemCategories.ChildItem;
			configurationItem.ItemType = "Hardware";
			configurationItem.Path = path.Text;
			configurationItem.ParentPath = path.Text;
			configurationItem.DisplayName = replaceHardwareTaskResult.Hardware.Name;
			configurationItem.UIHint = "DirectionHorizontal";
			configurationItem.Properties = new Property[3]
			{
				GetItemTypeProperty("Hardware"),
				GeneralPropertyDefinitions.GetIdPropertyGuid(replaceHardwareTaskResult.Hardware.Id),
				GetBooleanProperty("SettingsChanged", replaceHardwareTaskResult.Hardware.SettingsChanged)
			};
			configurationItem.ChildrenFilled = true;
			ConfigurationItem item = configurationItem;
			list.Add(item);
			ConfigurationItem item2 = CreateAddedOrDeletedDevicesInfoFolderItem(replaceHardwareTaskResult.AddedDevices, added: true);
			list.Add(item2);
			ConfigurationItem item3 = CreateAddedOrDeletedDevicesInfoFolderItem(replaceHardwareTaskResult.DeletedDevices, added: false);
			list.Add(item3);
			ConfigurationItem item4 = CreateMappedDevicesInfoFolderItem(replaceHardwareTaskResult.MappedDevices);
			list.Add(item4);
		}
		return list.ToArray();
		ConfigurationItem CreateAddedOrDeletedDevicesInfoFolderItem(IEnumerable<ReplaceHardwareTaskDevice> devices, bool added)
		{
			return new ConfigurationItem
			{
				ItemCategory = ItemCategories.ChildItem,
				ItemType = "InfoItemFolder",
				Path = path.Text,
				ParentPath = path.Text,
				DisplayName = (added ? "Added devices" : "Deleted devices"),
				UIHint = "DirectionHorizontal",
				Properties = Array.Empty<Property>(),
				Children = devices.Select((ReplaceHardwareTaskDevice d) => CreateDeviceInfoItem(d)).ToArray(),
				ChildrenFilled = true
			};
		}
		ConfigurationItem CreateDeviceInfoItem(ReplaceHardwareTaskDevice device)
		{
			DeviceTypeRelation deviceTypeRelation = DeviceTypeRelation.Relations.First((DeviceTypeRelation r) => r.DeviceType.ToString().Equals(device.DeviceType, StringComparison.Ordinal));
			return new ConfigurationItem
			{
				ItemCategory = ItemCategories.ChildItem,
				ItemType = "InfoItem",
				Path = path.Text,
				ParentPath = path.Text,
				DisplayName = device.Name,
				UIHint = "DirectionHorizontal",
				Properties = new Property[2]
				{
					GetItemTypeProperty(deviceTypeRelation.ItemType),
					GeneralPropertyDefinitions.GetIdPropertyGuid(device.Id)
				},
				ChildrenFilled = true
			};
		}
		ConfigurationItem CreateInfoItem(Guid id, string name, string displayName, string itemType)
		{
			return new ConfigurationItem
			{
				ItemCategory = ItemCategories.ChildItem,
				ItemType = "InfoItem",
				Path = path.Text,
				ParentPath = path.Text,
				DisplayName = name,
				UIHint = "DirectionHorizontal",
				Properties = new Property[2]
				{
					GetItemTypeProperty(itemType),
					GeneralPropertyDefinitions.GetIdPropertyGuid(id)
				},
				ChildrenFilled = true
			};
		}
		ConfigurationItem CreateMappedDeviceInfoItem(ReplaceHardwareTaskDevice mappedDevice)
		{
			DeviceTypeRelation deviceTypeRelation = DeviceTypeRelation.Relations.First((DeviceTypeRelation r) => r.DeviceType.ToString().Equals(mappedDevice.DeviceType, StringComparison.Ordinal));
			return new ConfigurationItem
			{
				ItemCategory = ItemCategories.ChildItem,
				ItemType = "InfoItem",
				Path = path.Text,
				ParentPath = path.Text,
				DisplayName = mappedDevice.Name,
				UIHint = "DirectionHorizontal",
				Properties = new Property[3]
				{
					GetItemTypeProperty(deviceTypeRelation.ItemType),
					GeneralPropertyDefinitions.GetIdPropertyGuid(mappedDevice.Id),
					GetBooleanProperty("SettingsChanged", mappedDevice.SettingsChanged)
				},
				Children = CreateChildren(),
				ChildrenFilled = true
			};
			ConfigurationItem[] CreateChildren()
			{
				List<ConfigurationItem> list2 = new List<ConfigurationItem>();
				ConfigurationItem configurationItem2 = CreateDeletedEventsInfoFolderItem();
				if (configurationItem2 != null)
				{
					list2.Add(configurationItem2);
				}
				ConfigurationItem configurationItem3 = CreateDeletedStreamsInfoFolderItem();
				if (configurationItem3 != null)
				{
					list2.Add(configurationItem3);
				}
				ConfigurationItem configurationItem4 = CreateDeletedPtzPresetsFolderItem();
				if (configurationItem4 != null)
				{
					list2.Add(configurationItem4);
				}
				ConfigurationItem configurationItem5 = CreateDeletedPatrollingProfilesInfoFolderItem();
				if (configurationItem5 != null)
				{
					list2.Add(configurationItem5);
				}
				return list2.ToArray();
			}
			ConfigurationItem CreateDeletedEventsInfoFolderItem()
			{
				if (mappedDevice.DeletedEvents == null)
				{
					return null;
				}
				return new ConfigurationItem
				{
					ItemCategory = ItemCategories.ChildItem,
					ItemType = "InfoItemFolder",
					Path = path.Text,
					ParentPath = path.Text,
					DisplayName = "Deleted events",
					UIHint = "DirectionHorizontal",
					Properties = Array.Empty<Property>(),
					Children = mappedDevice.DeletedEvents.Select((ReplaceHardwareTaskEvent e) => CreateInfoItem(e.Id, e.Name, e.Name, "HardwareDeviceEvent")).ToArray(),
					ChildrenFilled = true
				};
			}
			ConfigurationItem CreateDeletedPatrollingProfilesInfoFolderItem()
			{
				if (mappedDevice.DeletedPatrollingProfiles == null)
				{
					return null;
				}
				return new ConfigurationItem
				{
					ItemCategory = ItemCategories.ChildItem,
					ItemType = "InfoItemFolder",
					DisplayName = "Deleted patrolling profiles",
					Path = path.Text,
					ParentPath = path.Text,
					UIHint = "DirectionHorizontal",
					Properties = Array.Empty<Property>(),
					Children = CreateDeletedPatrollingProfilesInfoItems(),
					ChildrenFilled = true
				};
			}
			ConfigurationItem[] CreateDeletedPatrollingProfilesInfoItems()
			{
				List<ConfigurationItem> list2 = new List<ConfigurationItem>();
				foreach (ReplaceHardwareTaskPatrollingProfile deletedPatrollingProfile in mappedDevice.DeletedPatrollingProfiles)
				{
					ConfigurationItem item5 = new ConfigurationItem
					{
						ItemCategory = ItemCategories.ChildItem,
						ItemType = "InfoItem",
						Path = path.Text,
						DisplayName = deletedPatrollingProfile.Name,
						ParentPath = path.Text,
						UIHint = "DirectionHorizontal",
						Properties = new Property[1] { GetItemTypeProperty("PatrollingProfile") },
						ChildrenFilled = true
					};
					list2.Add(item5);
				}
				return list2.ToArray();
			}
			ConfigurationItem CreateDeletedPtzPresetsFolderItem()
			{
				if (mappedDevice.DeletedPtzPresets == null)
				{
					return null;
				}
				return new ConfigurationItem
				{
					ItemCategory = ItemCategories.ChildItem,
					ItemType = "InfoItemFolder",
					Path = path.Text,
					ParentPath = path.Text,
					DisplayName = "Deleted ptz presets",
					UIHint = "DirectionHorizontal",
					Properties = Array.Empty<Property>(),
					Children = mappedDevice.DeletedPtzPresets.Select((ReplaceHardwareTaskPtzPreset p) => CreateInfoItem(p.Id, p.Name, p.Name, "PtzPreset")).ToArray(),
					ChildrenFilled = true
				};
			}
			ConfigurationItem CreateDeletedStreamsInfoFolderItem()
			{
				if (mappedDevice.DeletedStreams == null)
				{
					return null;
				}
				return new ConfigurationItem
				{
					ItemCategory = ItemCategories.ChildItem,
					ItemType = "InfoItemFolder",
					Path = path.Text,
					ParentPath = path.Text,
					DisplayName = "Deleted streams",
					UIHint = "DirectionHorizontal",
					Properties = Array.Empty<Property>(),
					Children = mappedDevice.DeletedStreams.Select((ReplaceHardwareTaskStream s) => CreateInfoItem(s.ReferenceId, s.Name, s.Name, "Stream")).ToArray(),
					ChildrenFilled = true
				};
			}
		}
		ConfigurationItem CreateMappedDevicesInfoFolderItem(IEnumerable<ReplaceHardwareTaskDevice> mappedDevices)
		{
			return new ConfigurationItem
			{
				ItemCategory = ItemCategories.ChildItem,
				ItemType = "InfoItemFolder",
				Path = path.Text,
				ParentPath = path.Text,
				DisplayName = "Mapped devices",
				UIHint = "DirectionHorizontal",
				Properties = Array.Empty<Property>(),
				Children = mappedDevices.Select((ReplaceHardwareTaskDevice d) => CreateMappedDeviceInfoItem(d)).ToArray(),
				ChildrenFilled = true
			};
		}
		static Property GetItemTypeProperty(string itemType)
		{
			return new Property("ItemType", itemType);
		}
	}

	private static Property GetBooleanProperty(string name, bool value)
	{
		return new Property(name, PropertyTypesHelper.GetValue(value))
		{
			ValueType = "Tick",
			IsSettable = false,
			DisplayName = name,
			ItemReference = false,
			ServerValidation = false,
			ValueTypeInfos = PropertyTypesHelper.GetBoolValueTypeInfos()
		};
	}

	private static Property PropertyNewFirmwareVersion(string newFirmwareVersion)
	{
		return new Property("NewFirmwareVersion", newFirmwareVersion)
		{
			ValueType = "String",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyNewFirmwareVersion,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyNewFirmwareVersion",
			ToolTipTranslationId = "PropertyNewFirmwareVersionToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 1
		};
	}
}
