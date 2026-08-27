using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class GroupConfigurationItemFactory
{
	private static Lazy<Dictionary<string, string>> _displayNames = new Lazy<Dictionary<string, string>>(LoadDisplayNames);

	private static Lazy<Dictionary<string, string[]>> _methods = new Lazy<Dictionary<string, string[]>>(LoadMethods);

	public static ConfigurationItem CreateFolderItem(string itemType, ConfigurationItemPath parentPath)
	{
		return CreateFolderItem(itemType, parentPath, null);
	}

	public static ConfigurationItem CreateFolderItem(string itemType, ConfigurationItemPath parentPath, Property[] properties)
	{
		ConfigurationItemPath configurationItemPath = new ConfigurationItemPathBuilder(parentPath, itemType).CreatePath();
		return new ConfigurationItem
		{
			ItemCategory = ItemCategories.Group,
			ItemType = itemType,
			Path = configurationItemPath.Text,
			DisplayName = GetDisplayName(itemType),
			ParentPath = parentPath.Text,
			UIHint = "DirectionHorizontal",
			MethodIds = ResolveMethodIds(itemType, parentPath),
			Properties = properties
		};
	}

	public static ConfigurationItem CreateGroupItem(string itemType, Guid id, string resourceId, ConfigurationItemPath parentPath)
	{
		ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreateItemPath(itemType, id.ToString());
		string text = ConfigurationApiResources.ResourceManager.GetString(resourceId);
		if (string.IsNullOrEmpty(text))
		{
			text = resourceId;
		}
		return new ConfigurationItem
		{
			ItemCategory = ItemCategories.Item,
			ItemType = itemType,
			Path = configurationItemPath.Text,
			DisplayName = text,
			ParentPath = parentPath.Text,
			UIHint = "DirectionHorizontal",
			MethodIds = new string[0],
			Properties = new Property[0],
			Children = null,
			ChildrenFilled = false
		};
	}

	private static string[] ResolveMethodIds(string itemType, ConfigurationItemPath parentPath)
	{
		DeviceTypeRelation deviceTypeRelation = DeviceTypeRelation.Relations.FirstOrDefault((DeviceTypeRelation r) => string.Equals(r.GroupFolderType, itemType, StringComparison.Ordinal));
		if (deviceTypeRelation != null && (string.Equals(parentPath.ItemType, "System", StringComparison.Ordinal) || string.Equals(parentPath.ItemType, deviceTypeRelation.GroupType, StringComparison.Ordinal)))
		{
			return new string[2] { "AddDeviceGroup", "RemoveDeviceGroup" };
		}
		deviceTypeRelation = DeviceTypeRelation.Relations.FirstOrDefault((DeviceTypeRelation r) => string.Equals(r.FolderType, itemType, StringComparison.Ordinal));
		if (deviceTypeRelation != null && string.Equals(parentPath.ItemType, deviceTypeRelation.GroupType, StringComparison.Ordinal))
		{
			return new string[2] { "AddDeviceGroupMember", "RemoveDeviceGroupMember" };
		}
		if (string.Equals(parentPath.ItemType, "BasicUser", StringComparison.Ordinal))
		{
			return Array.Empty<string>();
		}
		if (_methods.Value.TryGetValue(itemType, out var value))
		{
			return value;
		}
		return Array.Empty<string>();
	}

	private static string GetDisplayName(string itemType)
	{
		if (_displayNames.Value.TryGetValue(itemType, out var value))
		{
			return value;
		}
		throw new ArgumentException("Unknown item type in GroupConfigurationFactory", "itemType");
	}

	private static Dictionary<string, string> LoadDisplayNames()
	{
		return new Dictionary<string, string>
		{
			{
				"RecordingServerFolder",
				ConfigurationApiResources.ItemTypeRecordingServerFolder
			},
			{
				"ServerFolder",
				ConfigurationApiResources.ItemTypeServerFolder
			},
			{
				"SystemAddressFolder",
				ConfigurationApiResources.ItemTypeSystemAddressFolder
			},
			{
				"HardwareFolder",
				ConfigurationApiResources.ItemTypeHardwareFolder
			},
			{
				"HardwareDriverFolder",
				ConfigurationApiResources.ItemTypeHardwareDriverFolder
			},
			{
				"CameraFolder",
				ConfigurationApiResources.ItemTypeCameraFolder
			},
			{
				"HardwareDeviceEventFolder",
				ConfigurationApiResources.ItemTypeHardwareDeviceEventFolder
			},
			{
				"MicrophoneFolder",
				ConfigurationApiResources.ItemTypeMicrophoneFolder
			},
			{
				"SpeakerFolder",
				ConfigurationApiResources.ItemTypeSpeakerFolder
			},
			{
				"InputEventFolder",
				ConfigurationApiResources.ItemTypeInputEventFolder
			},
			{
				"OutputFolder",
				ConfigurationApiResources.ItemTypeOutputFolder
			},
			{
				"MetadataFolder",
				ConfigurationApiResources.ItemTypeMetadataFolder
			},
			{
				"CameraGroupFolder",
				ConfigurationApiResources.ItemTypeCameraGroupFolder
			},
			{
				"MicrophoneGroupFolder",
				ConfigurationApiResources.ItemTypeMicrophoneGroupFolder
			},
			{
				"SpeakerGroupFolder",
				ConfigurationApiResources.ItemTypeSpeakerGroupFolder
			},
			{
				"InputEventGroupFolder",
				ConfigurationApiResources.ItemTypeInputEventGroupFolder
			},
			{
				"OutputGroupFolder",
				ConfigurationApiResources.ItemTypeOutputGroupFolder
			},
			{
				"MetadataGroupFolder",
				ConfigurationApiResources.ItemTypeMetadataGroupFolder
			},
			{
				"HardwareDriverSettingsFolder",
				ConfigurationApiResources.ItemTypeHardwareDriverSettingsFolder
			},
			{
				"DeviceDriverSettingsFolder",
				ConfigurationApiResources.ItemTypeDeviceDriverSettingsFolder
			},
			{
				"LoginProviderFolder",
				ConfigurationApiResources.ItemTypeLoginProviderFolder
			},
			{
				"RegisteredClaimFolder",
				ConfigurationApiResources.ItemTypeRegisteredClaimFolder
			},
			{
				"BasicUserFolder",
				ConfigurationApiResources.ItemTypeBasicUserFolder
			},
			{
				"PtzPresetFolder",
				ConfigurationApiResources.ItemTypePtzPresetFolder
			},
			{
				"StreamFolder",
				ConfigurationApiResources.ItemTypeStreamFolder
			},
			{
				"RoleFolder",
				ConfigurationApiResources.ItemTypeRoleFolder
			},
			{
				"UserFolder",
				ConfigurationApiResources.ItemTypeRoleMemberFolder
			},
			{
				"ClaimFolder",
				ConfigurationApiResources.ItemTypeClaimFolder
			},
			{
				"StorageFolder",
				ConfigurationApiResources.ItemTypeStorageFolder
			},
			{
				"ArchiveStorageFolder",
				ConfigurationApiResources.ItemTypeArchiveStorageFolder
			},
			{
				"MotionDetectionFolder",
				ConfigurationApiResources.ItemTypeMotionDetectionFolder
			},
			{
				"PrivacyProtectionFolder",
				ConfigurationApiResources.ItemTypePrivacyProtectionFolder
			},
			{
				"LayoutFolder",
				ConfigurationApiResources.ItemTypeViewLayoutFolder
			},
			{
				"LayoutGroupFolder",
				ConfigurationApiResources.ItemTypeViewLayoutGroupFolder
			},
			{
				"VideoWallFolder",
				ConfigurationApiResources.ItemTypeVideoWallFolder
			},
			{
				"MonitorFolder",
				ConfigurationApiResources.ItemTypeMonitorFolder
			},
			{
				"VideoWallPresetFolder",
				ConfigurationApiResources.ItemTypeVideoWallPresetFolder
			},
			{
				"MonitorPresetFolder",
				ConfigurationApiResources.ItemTypeMonitorPresetFolder
			},
			{
				"CustomPropertiesFolder",
				ConfigurationApiResources.ItemTypeCustomPropertiesFolder
			},
			{
				"UserDefinedEventFolder",
				ConfigurationApiResources.ItemTypeUserDefinedEventFolder
			},
			{
				"AnalyticsEventFolder",
				ConfigurationApiResources.ItemTypeAnalyticsEventFolder
			},
			{
				"GenericEventFolder",
				ConfigurationApiResources.ItemTypeGenericEventFolder
			},
			{
				"GenericEventDataSourceFolder",
				ConfigurationApiResources.ItemTypeGenericEventDataSourceFolder
			},
			{
				"GisMapLocationFolder",
				ConfigurationApiResources.ItemTypeGisMapLocationFolder
			},
			{
				"TimeProfileFolder",
				ConfigurationApiResources.ItemTypeTimeProfileFolder
			},
			{
				"ClientSettingsFolder",
				ConfigurationApiResources.ItemTypeClientSettingsFolder
			},
			{
				"AlarmDefinitionFolder",
				ConfigurationApiResources.ItemTypeAlarmDefinitionFolder
			},
			{
				"MIPKindFolder",
				ConfigurationApiResources.ItemTypeMIPKindFolder
			},
			{
				"LprMatchListFolder",
				ConfigurationApiResources.ItemTypeLPRMatchListFolder
			},
			{
				"SaveSearchesFolder",
				ConfigurationApiResources.ItemTypeSearchTemplateFolder
			},
			{
				"RuleFolder",
				ConfigurationApiResources.ItemTypeRuleFolder
			},
			{
				"AccessControlSystemFolder",
				ConfigurationApiResources.ItemTypeAccessControlSystemFolder
			},
			{
				"AccessControlUnitFolder",
				ConfigurationApiResources.ItemTypeAccessControlUnitFolder
			},
			{
				"SiteFolder",
				ConfigurationApiResources.ItemTypeSiteFolder
			},
			{
				"SiteAddressFolder",
				ConfigurationApiResources.ItemTypeSystemAddressFolder
			},
			{
				"LicenseInformationFolder",
				ConfigurationApiResources.ItemTypeLicenseInformationFolder
			},
			{
				"LicenseInstalledProductFolder",
				ConfigurationApiResources.ItemTypeLicenseInstalledProductFolder
			},
			{
				"LicenseOverviewAllFolder",
				ConfigurationApiResources.ItemTypeLicenseOverviewAllFolder
			},
			{
				"LicenseDetailFolder",
				ConfigurationApiResources.ItemTypeLicenseDetailFolder
			},
			{
				"BasicOwnerInformationFolder",
				ConfigurationApiResources.ItemTypeBasicOwnerInformationFolder
			},
			{
				"ToolOptionFolder",
				ConfigurationApiResources.ItemTypeToolOptionFolder
			},
			{
				"ViewGroupFolder",
				ConfigurationApiResources.ItemTypeViewGroupFolder
			},
			{
				"ViewFolder",
				ConfigurationApiResources.ItemTypeViewFolder
			},
			{
				"RecordingServerMulticastFolder",
				ConfigurationApiResources.ItemTypeRecordingServerMulticastFolder
			},
			{
				"HardwarePtzSettingsFolder",
				ConfigurationApiResources.ItemTypeHardwarePtzSettingsFolder
			},
			{
				"PatrollingProfileFolder",
				ConfigurationApiResources.ItemTypePatrollingProfileFolder
			},
			{
				"FailoverGroupFolder",
				ConfigurationApiResources.ItemTypeFailoverGroupFolder
			},
			{
				"FailoverRecorderFolder",
				ConfigurationApiResources.ItemTypeFailoverRecorderFolder
			},
			{
				"ClientProfileFolder",
				ConfigurationApiResources.ItemTypeClientProfileFolder
			},
			{
				"RecordingServerFailoverFolder",
				ConfigurationApiResources.ItemTypeRecordingServerFailoverFolder
			},
			{
				"EventTypeGroupFolder",
				ConfigurationApiResources.ItemTypeEventTypeGroupFolder
			},
			{
				"EventTypeFolder",
				ConfigurationApiResources.ItemTypeEventTypeFolder
			},
			{
				"StateGroupFolder",
				ConfigurationApiResources.StateGroupFolder
			},
			{
				"MatrixFolder",
				ConfigurationApiResources.MatrixFolder
			},
			{
				"FisheyeLensFolder",
				ConfigurationApiResources.FisheyeFolder
			},
			{
				"EvidenceLockProfileFolder",
				ConfigurationApiResources.EvidenceLockProfileFolder
			},
			{
				"CompressorFolder",
				ConfigurationApiResources.CompressorFolder
			},
			{
				"MailNotificationProfileFolder",
				ConfigurationApiResources.MailNotificationProfileFolder
			},
			{
				"AccessControlGroupFolder",
				ConfigurationApiResources.AccessControlGroupFolder
			}
		};
	}

	private static Dictionary<string, string[]> LoadMethods()
	{
		Dictionary<string, string[]> dictionary = new Dictionary<string, string[]>(StringComparer.Ordinal);
		dictionary.Add("RecordingServerFolder", new string[1] { "RemoveRecordingServer" });
		dictionary.Add("LoginProviderFolder", new string[2] { "AddLoginProvider", "RemoveLoginProvider" });
		dictionary.Add("RegisteredClaimFolder", new string[2] { "AddRegisteredClaim", "RemoveRegisteredClaim" });
		dictionary.Add("BasicUserFolder", new string[2] { "AddBasicUser", "RemoveBasicUser" });
		dictionary.Add("RoleFolder", new string[5] { "AddRole", "RemoveRole", "CopyRole", "QueryUsersWithAccess", "QueryIsPermittedTimeBasedAction" });
		dictionary.Add("UserFolder", new string[2] { "AddRoleMember", "RemoveRoleMember" });
		dictionary.Add("ClaimFolder", new string[2] { "AddRoleClaim", "RemoveRoleClaim" });
		dictionary.Add("PtzPresetFolder", new string[4] { "AddPtzPreset", "RemovePtzPreset", "DefaultPtzPreset", "GetDevicePresets" });
		dictionary.Add("StorageFolder", new string[2] { "AddStorage", "RemoveStorage" });
		dictionary.Add("ArchiveStorageFolder", new string[2] { "AddArchiveStorage", "RemoveArchiveStorage" });
		dictionary.Add("MotionDetectionFolder", Array.Empty<string>());
		dictionary.Add("PrivacyProtectionFolder", Array.Empty<string>());
		dictionary.Add("HardwareFolder", new string[1] { "DeleteHardware" });
		dictionary.Add("LayoutGroupFolder", Array.Empty<string>());
		dictionary.Add("LayoutFolder", new string[2] { "AddLayout", "RemoveLayout" });
		dictionary.Add("VideoWallFolder", new string[2] { "AddVideoWall", "RemoveVideoWall" });
		dictionary.Add("MonitorFolder", new string[2] { "AddMonitor", "RemoveMonitor" });
		dictionary.Add("VideoWallPresetFolder", new string[2] { "AddVideoWallPreset", "RemoveVideoWallPreset" });
		dictionary.Add("UserDefinedEventFolder", new string[2] { "AddUserDefinedEvent", "RemoveUserDefinedEvent" });
		dictionary.Add("AnalyticsEventFolder", new string[2] { "AddAnalyticsEvent", "RemoveAnalyticsEvent" });
		dictionary.Add("GenericEventFolder", new string[2] { "AddGenericEvent", "RemoveGenericEvent" });
		dictionary.Add("GenericEventDataSourceFolder", new string[2] { "AddGenericEventDataSource", "RemoveGenericEventDataSource" });
		dictionary.Add("GisMapLocationFolder", new string[2] { "AddGisMapLocation", "RemoveGisMapLocation" });
		dictionary.Add("TimeProfileFolder", new string[2] { "AddTimeProfile", "RemoveTimeProfile" });
		dictionary.Add("ClientSettingsFolder", Array.Empty<string>());
		dictionary.Add("AlarmDefinitionFolder", new string[2] { "AddAlarmDefinition", "RemoveAlarmDefinition" });
		dictionary.Add("MIPKindFolder", new string[5] { "AddMIPKind", "RemoveMIPKind", "GetMIPOptionProperty", "SetMIPOptionProperty", "MIPItemDataVersionSum" });
		dictionary.Add("LprMatchListFolder", new string[2] { "MethodIdAddLprMatchList", "MethodIdRemoveLprMatchList" });
		dictionary.Add("SaveSearchesFolder", new string[5] { "AddSaveSearch", "UpdateSaveSearch", "RemoveSaveSearch", "GetSaveSearch", "FindSaveSearches" });
		dictionary.Add("RuleFolder", new string[2] { "AddRule", "RemoveRule" });
		dictionary.Add("AccessControlSystemFolder", Array.Empty<string>());
		dictionary.Add("AccessControlUnitFolder", Array.Empty<string>());
		dictionary.Add("SiteFolder", Array.Empty<string>());
		dictionary.Add("SiteAddressFolder", Array.Empty<string>());
		dictionary.Add("HardwareDeviceEventFolder", Array.Empty<string>());
		dictionary.Add("ToolOptionFolder", Array.Empty<string>());
		dictionary.Add("ViewGroupFolder", new string[2] { "AddViewGroup", "RemoveViewGroup" });
		dictionary.Add("ViewFolder", new string[2] { "AddView", "RemoveView" });
		dictionary.Add("PatrollingProfileFolder", new string[2] { "AddPatrollingProfile", "RemovePatrollingProfile" });
		dictionary.Add("FailoverRecorderFolder", Array.Empty<string>());
		dictionary.Add("FailoverGroupFolder", new string[3] { "AddFailoverGroup", "RemoveFailoverGroup", "MoveFailoverGroup" });
		dictionary.Add("ClientProfileFolder", new string[2] { "AddClientProfile", "RemoveClientProfile" });
		dictionary.Add("StateGroupFolder", new string[2] { "AddStateGroup", "RemoveStateGroup" });
		dictionary.Add("MatrixFolder", new string[2] { "AddMatrix", "RemoveMatrix" });
		dictionary.Add("EvidenceLockProfileFolder", new string[2] { "AddEvidenceLockProfile", "RemoveEvidenceLockProfile" });
		dictionary.Add("MailNotificationProfileFolder", new string[2] { "AddMailNotificationProfile", "RemoveMailNotificationProfile" });
		dictionary.Add("AccessControlGroupFolder", new string[2] { "AddAccessControlGroup", "RemoveAccessControlGroup" });
		return dictionary;
	}
}
