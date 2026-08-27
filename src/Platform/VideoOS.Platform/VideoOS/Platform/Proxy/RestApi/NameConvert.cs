using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.Proxy.RestApi;

public class NameConvert : INameConvert
{
	public enum TagGroups
	{
		Core,
		Grouping,
		Events,
		VAP,
		Storage,
		DeviceConfig,
		Rule,
		Role,
		Security,
		Client,
		RecordingServer,
		WebRTC
	}

	private static readonly List<ResourceEntry> Resources = new List<ResourceEntry>
	{
		new ResourceEntry("AccessControlGroup", "AccessControlGroupFolder", "accessControlGroups"),
		new ResourceEntry("AccessControlSystem", "AccessControlSystemFolder", "accessControlSystems"),
		new ResourceEntry("AccessControlUnit", "AccessControlUnitFolder", "accessControlUnits"),
		new ResourceEntry("AccessControlRuleGroup", "", "accessControlRuleGroups"),
		new ResourceEntry("AlarmDefinition", "AlarmDefinitionFolder", "alarmDefinitions"),
		new ResourceEntry("AnalyticsEvent", "AnalyticsEventFolder", "analyticsEvents"),
		new ResourceEntry("ArchiveStorage", "ArchiveStorageFolder", "archiveStorages"),
		new ResourceEntry("ArchiveStorageInformation", "ArchiveStorageInformationFolder", "archiveStorageInformation"),
		new ResourceEntry("ArchiveSchedule", "ArchiveScheduleFolder", "archiveSchedules"),
		new ResourceEntry("AudioMessage", "AudioMessageFolder", "audioMessages"),
		new ResourceEntry("BasicUser", "BasicUserFolder", "basicUsers"),
		new ResourceEntry("BasicOwnerInformation", "BasicOwnerInformationFolder", "owner"),
		new ResourceEntry("Camera", "CameraFolder", "cameras"),
		new ResourceEntry("CameraGroup", "CameraGroupFolder", "cameraGroups"),
		new ResourceEntry("Claim", "ClaimFolder", "claims"),
		new ResourceEntry("RegisteredClaim", "RegisteredClaimFolder", "registeredClaims"),
		new ResourceEntry("ClientSettings", "ClientSettingsFolder", "clientSettings"),
		new ResourceEntry("ClientProfile", "ClientProfileFolder", "clientProfiles"),
		new ResourceEntry("ClientProfileAccessControl", "ClientProfile", "clientProfileAccessControl"),
		new ResourceEntry("ClientProfileAdvanced", "ClientProfile", "clientProfileAdvanced"),
		new ResourceEntry("ClientProfileAlarmManager", "ClientProfile", "clientProfileAlarmManager"),
		new ResourceEntry("ClientProfileExport", "ClientProfile", "clientProfileExport"),
		new ResourceEntry("ClientProfileGeneral", "ClientProfile", "clientProfileGeneral"),
		new ResourceEntry("ClientProfileGisMap", "ClientProfile", "clientProfileGisMap"),
		new ResourceEntry("ClientProfileLive", "ClientProfile", "clientProfileLive"),
		new ResourceEntry("ClientProfilePlayback", "ClientProfile", "clientProfilePlayback"),
		new ResourceEntry("ClientProfileSetup", "ClientProfile", "clientProfileSetup"),
		new ResourceEntry("ClientProfileTimeline", "ClientProfile", "clientProfileTimeline"),
		new ResourceEntry("ClientProfileViewLayouts", "ClientProfile", "clientProfileViewLayouts"),
		new ResourceEntry("Compressor", "CompressorFolder", "compressors"),
		new ResourceEntry("CustomProperties", "CustomPropertiesFolder", "customProperties"),
		new ResourceEntry("CustomSettingGlobal", "", "customSettingGlobal"),
		new ResourceEntry("CustomSettingUser", "", "customSettingUser"),
		new ResourceEntry("DeviceDriverSettings", "DeviceDriverSettingsFolder", "settings"),
		new ResourceEntry("FailoverGroup", "FailoverGroupFolder", "failoverGroups"),
		new ResourceEntry("FailoverRecorder", "FailoverRecorderFolder", "failoverRecorders"),
		new ResourceEntry("EventTypeGroup", "EventTypeGroupFolder", "eventTypeGroups"),
		new ResourceEntry("EventType", "EventTypeFolder", "eventTypes"),
		new ResourceEntry("EvidenceLockProfile", "EvidenceLockProfileFolder", "evidenceLockProfiles"),
		new ResourceEntry("External", "ExternalFolder", "externals"),
		new ResourceEntry("FindSaveSearches", "", "findSaveSearches"),
		new ResourceEntry("FisheyeLens", "FisheyeLensFolder", "fisheyeLens"),
		new ResourceEntry("GenericEvent", "GenericEventFolder", "genericEvents"),
		new ResourceEntry("GenericEventDataSource", "GenericEventDataSourceFolder", "genericEventDataSources"),
		new ResourceEntry("GisMapLocation", "GisMapLocationFolder", "gisMapLocations"),
		new ResourceEntry("Hardware", "HardwareFolder", "hardware"),
		new ResourceEntry("HardwareDriver", "HardwareDriverFolder", "hardwareDrivers"),
		new ResourceEntry("HardwareDeviceEvent", "HardwareDeviceEventFolder", "hardwareDeviceEvents"),
		new ResourceEntry("HardwareDriverSettings", "HardwareDriverSettingsFolder", "hardwareDriverSettings"),
		new ResourceEntry("HardwarePtzSettings", "HardwarePtzSettingsFolder", "hardwarePtzSettings"),
		new ResourceEntry("HardwarePtzDeviceSetting", "HardwarePtzDeviceSettingFolder", "hardwarePtzDeviceSettings"),
		new ResourceEntry("InfoItem", "InfoItemFolder", "infoItems"),
		new ResourceEntry("InputEvent", "InputEventFolder", "inputEvents"),
		new ResourceEntry("InputEventGroup", "InputEventGroupFolder", "inputEventGroups"),
		new ResourceEntry("Layout", "LayoutFolder", "layouts"),
		new ResourceEntry("LayoutGroup", "LayoutGroupFolder", "layoutGroups"),
		new ResourceEntry("LprMatchList", "LprMatchListFolder", "lprMatchLists"),
		new ResourceEntry("LicenseInformation", "LicenseInformationFolder", "licenseInformations"),
		new ResourceEntry("LicenseInstalledProduct", "LicenseInstalledProductFolder", "licenseInstalledProducts"),
		new ResourceEntry("LicenseOverviewAll", "LicenseOverviewAllFolder", "licenseOverviewAll"),
		new ResourceEntry("LicenseDetail", "LicenseDetailFolder", "licenseDetails"),
		new ResourceEntry("LicenseFeature", "", "licenseProperties"),
		new ResourceEntry("LoginProvider", "LoginProviderFolder", "loginProviders"),
		new ResourceEntry("MailNotificationProfile", "MailNotificationProfileFolder", "mailNotificationProfiles"),
		new ResourceEntry("Matrix", "MatrixFolder", "matrix"),
		new ResourceEntry("Metadata", "MetadataFolder", "metadata"),
		new ResourceEntry("MetadataGroup", "MetadataGroupFolder", "metadataGroups"),
		new ResourceEntry("Microphone", "MicrophoneFolder", "microphones"),
		new ResourceEntry("MicrophoneGroup", "MicrophoneGroupFolder", "microphoneGroups"),
		new ResourceEntry("MIPKind", "MIPKindFolder", "mipKinds"),
		new ResourceEntry("MIPItem", "MIPItemFolder", "mipItems"),
		new ResourceEntry("MotionDetection", "MotionDetectionFolder", "motionDetections"),
		new ResourceEntry("", "", "sites"),
		new ResourceEntry("Monitor", "MonitorFolder", "monitors"),
		new ResourceEntry("MonitorPreset", "MonitorPresetFolder", "monitorPresets"),
		new ResourceEntry("Output", "OutputFolder", "outputs"),
		new ResourceEntry("OutputGroup", "OutputGroupFolder", "outputGroups"),
		new ResourceEntry("PatrollingProfile", "PatrollingProfileFolder", "patrollingProfiles"),
		new ResourceEntry("PatrollingEntry", "PatrollingEntryFolder", "patrollingEntry"),
		new ResourceEntry("PtzPreset", "PtzPresetFolder", "ptzPresets"),
		new ResourceEntry("PTZ", "", "ptz"),
		new ResourceEntry("PTZSessionTimeout", "PTZSessionTimeoutFolder", "ptzSessionTimeouts"),
		new ResourceEntry("PrivacyProtection", "PrivacyProtectionFolder", "privacyProtections"),
		new ResourceEntry("RecordingServerFailover", "RecordingServerFailoverFolder", "recordingServerFailovers"),
		new ResourceEntry("RecordingServer", "RecordingServerFolder", "recordingServers"),
		new ResourceEntry("RecordingServerMulticast", "RecordingServerMulticastFolder", "recordingServerMulticasts"),
		new ResourceEntry("Role", "RoleFolder", "roles"),
		new ResourceEntry("Rule", "RuleFolder", "rules"),
		new ResourceEntry("SaveSearches", "SaveSearchesFolder", "saveSearches"),
		new ResourceEntry("Server", "ServerFolder", "servers"),
		new ResourceEntry("Speaker", "SpeakerFolder", "speakers"),
		new ResourceEntry("Storage", "StorageFolder", "storages"),
		new ResourceEntry("StorageInformation", "StorageInformationFolder", "storageInformation"),
		new ResourceEntry("Site", "SiteFolder", "childSites"),
		new ResourceEntry("SiteAddress", "SiteAddressFolder", "siteAddresses"),
		new ResourceEntry("StateGroup", "StateGroupFolder", "stateGroups"),
		new ResourceEntry("Stream", "StreamFolder", "streams"),
		new ResourceEntry("SpeakerGroup", "SpeakerGroupFolder", "speakerGroups"),
		new ResourceEntry("SystemAddress", "SystemAddressFolder", "systemAddresses"),
		new ResourceEntry("Task", "TaskFolder", "tasks"),
		new ResourceEntry("TimeProfile", "TimeProfileFolder", "timeProfiles"),
		new ResourceEntry("TimeProfileAppointmentRoot", "", "timeProfileAppointmentRoot"),
		new ResourceEntry("TimeProfileAppointmentRecur", "", "timeProfileAppointmentRecur"),
		new ResourceEntry("TimeProfileAppointmentVariance", "", "timeProfileAppointmentVariance"),
		new ResourceEntry("TimeProfileSunclock", "", "timeProfileSunclock"),
		new ResourceEntry("ToolOption", "ToolOptionFolder", "toolOptions"),
		new ResourceEntry("Unknown", "", "unknown"),
		new ResourceEntry("User", "UserFolder", "users"),
		new ResourceEntry("UserDefinedEvent", "UserDefinedEventFolder", "userDefinedEvents"),
		new ResourceEntry("VideoWall", "VideoWallFolder", "videoWalls"),
		new ResourceEntry("VideoWallPreset", "VideoWallPresetFolder", "videoWallPresets"),
		new ResourceEntry("View", "ViewFolder", "views"),
		new ResourceEntry("ViewItem", "ViewItemFolder", "viewItems"),
		new ResourceEntry("ViewGroup", "ViewGroupFolder", "viewGroups")
	};

	private static readonly Dictionary<string, string> ItemTypesWithNoRootFolderSupport = new Dictionary<string, string>
	{
		{ "Layout", "layoutGroups" },
		{ "HardwareDriver", "recordingServers" },
		{ "Storage", "recordingServers" },
		{ "ArchiveStorage", "recordingServers" },
		{ "VideoWallPreset", "videoWalls" },
		{ "FindSaveSearches", "saveSearches" },
		{ "User", "roles" },
		{ "Claim", "roles" },
		{ "RegisteredClaim", "loginProviders" },
		{ "PtzPreset", "cameras" },
		{ "ClientSettings", "cameras" },
		{ "settings", "cameras" },
		{ "Stream", "cameras" },
		{ "PtzPresets", "cameras" },
		{ "MotionDetection", "cameras" },
		{ "PrivacyProtection", "cameras" },
		{ "CustomProperties", "cameras" },
		{ "HardwareDeviceEvent", "cameras" },
		{ "CustomSettingGlobal", "" },
		{ "CustomSettingUser", "" },
		{ "LicenseFeature", "" }
	};

	private static readonly HashSet<string> ItemTypesWithOnlyFourthLevelPostSupport = new HashSet<string> { "PtzPreset" };

	private static readonly Dictionary<string, TagGroups> ResourcePluralTags = new Dictionary<string, TagGroups>
	{
		{
			"childSites",
			TagGroups.Core
		},
		{
			"registeredClaims",
			TagGroups.Core
		},
		{
			"licenseInformations",
			TagGroups.Core
		},
		{
			"licenseInstalledProducts",
			TagGroups.Core
		},
		{
			"licenseOverviewAll",
			TagGroups.Core
		},
		{
			"licenseDetails",
			TagGroups.Core
		},
		{
			"licenseProperties",
			TagGroups.Core
		},
		{
			"loginProviders",
			TagGroups.Core
		},
		{
			"owner",
			TagGroups.Core
		},
		{
			"matrix",
			TagGroups.Core
		},
		{
			"patrollingProfiles",
			TagGroups.Core
		},
		{
			"sites",
			TagGroups.Core
		},
		{
			"siteAddresses",
			TagGroups.Core
		},
		{
			"systemAddresses",
			TagGroups.Core
		},
		{
			"tasks",
			TagGroups.Core
		},
		{
			"toolOptions",
			TagGroups.Core
		},
		{
			"translations",
			TagGroups.Core
		},
		{
			"infoItems",
			TagGroups.Core
		},
		{
			"compressors",
			TagGroups.Core
		},
		{
			"mailNotificationProfiles",
			TagGroups.Core
		},
		{
			"servers",
			TagGroups.Core
		},
		{
			"clientSettings",
			TagGroups.Client
		},
		{
			"clientProfiles",
			TagGroups.Client
		},
		{
			"gisMapLocations",
			TagGroups.Client
		},
		{
			"layouts",
			TagGroups.Client
		},
		{
			"layoutGroups",
			TagGroups.Client
		},
		{
			"monitors",
			TagGroups.Client
		},
		{
			"monitorPresets",
			TagGroups.Client
		},
		{
			"saveSearches",
			TagGroups.Client
		},
		{
			"findSaveSearches",
			TagGroups.Client
		},
		{
			"fisheyeLens",
			TagGroups.Client
		},
		{
			"videoWalls",
			TagGroups.Client
		},
		{
			"videoWallPresets",
			TagGroups.Client
		},
		{
			"views",
			TagGroups.Client
		},
		{
			"viewGroups",
			TagGroups.Client
		},
		{
			"customSettingGlobal",
			TagGroups.Client
		},
		{
			"customSettingUser",
			TagGroups.Client
		},
		{
			"evidenceLockProfiles",
			TagGroups.Client
		},
		{
			"cameras",
			TagGroups.DeviceConfig
		},
		{
			"customProperties",
			TagGroups.DeviceConfig
		},
		{
			"hardware",
			TagGroups.DeviceConfig
		},
		{
			"hardwareDriverSettings",
			TagGroups.DeviceConfig
		},
		{
			"hardwareDeviceEvents",
			TagGroups.DeviceConfig
		},
		{
			"hardwarePtzSettings",
			TagGroups.DeviceConfig
		},
		{
			"hardwarePtzDeviceSettings",
			TagGroups.DeviceConfig
		},
		{
			"inputEvents",
			TagGroups.DeviceConfig
		},
		{
			"metadata",
			TagGroups.DeviceConfig
		},
		{
			"microphones",
			TagGroups.DeviceConfig
		},
		{
			"motionDetections",
			TagGroups.DeviceConfig
		},
		{
			"outputs",
			TagGroups.DeviceConfig
		},
		{
			"ptzPresets",
			TagGroups.DeviceConfig
		},
		{
			"privacyProtections",
			TagGroups.DeviceConfig
		},
		{
			"settings",
			TagGroups.DeviceConfig
		},
		{
			"speakers",
			TagGroups.DeviceConfig
		},
		{
			"streams",
			TagGroups.DeviceConfig
		},
		{
			"analyticsEvents",
			TagGroups.Events
		},
		{
			"externals",
			TagGroups.Events
		},
		{
			"genericEvents",
			TagGroups.Events
		},
		{
			"genericEventDataSources",
			TagGroups.Events
		},
		{
			"userDefinedEvents",
			TagGroups.Events
		},
		{
			"eventTypeGroups",
			TagGroups.Events
		},
		{
			"eventTypes",
			TagGroups.Events
		},
		{
			"stateGroups",
			TagGroups.Events
		},
		{
			"cameraGroups",
			TagGroups.Grouping
		},
		{
			"inputEventGroups",
			TagGroups.Grouping
		},
		{
			"metadataGroups",
			TagGroups.Grouping
		},
		{
			"microphoneGroups",
			TagGroups.Grouping
		},
		{
			"outputGroups",
			TagGroups.Grouping
		},
		{
			"speakerGroups",
			TagGroups.Grouping
		},
		{
			"basicUsers",
			TagGroups.Security
		},
		{
			"claims",
			TagGroups.Security
		},
		{
			"roles",
			TagGroups.Security
		},
		{
			"users",
			TagGroups.Security
		},
		{
			"rules",
			TagGroups.Rule
		},
		{
			"audioMessages",
			TagGroups.Rule
		},
		{
			"timeProfiles",
			TagGroups.Rule
		},
		{
			"failoverGroups",
			TagGroups.RecordingServer
		},
		{
			"failoverRecorders",
			TagGroups.RecordingServer
		},
		{
			"hardwareDrivers",
			TagGroups.RecordingServer
		},
		{
			"recordingServers",
			TagGroups.RecordingServer
		},
		{
			"recordingServerFailovers",
			TagGroups.RecordingServer
		},
		{
			"recordingServerMulticasts",
			TagGroups.RecordingServer
		},
		{
			"archiveSchedules",
			TagGroups.RecordingServer
		},
		{
			"archiveStorages",
			TagGroups.RecordingServer
		},
		{
			"archiveStorageInformation",
			TagGroups.RecordingServer
		},
		{
			"storages",
			TagGroups.RecordingServer
		},
		{
			"storageInformation",
			TagGroups.RecordingServer
		},
		{
			"accessControlGroups",
			TagGroups.VAP
		},
		{
			"accessControlSystems",
			TagGroups.VAP
		},
		{
			"accessControlUnits",
			TagGroups.VAP
		},
		{
			"alarmDefinitions",
			TagGroups.VAP
		},
		{
			"lprMatchLists",
			TagGroups.VAP
		},
		{
			"mipKinds",
			TagGroups.VAP
		},
		{
			"mipItems",
			TagGroups.VAP
		}
	};

	private static readonly Dictionary<string, string> ChildItemTypeNames = new Dictionary<string, string>
	{
		{ "DeviceDriverSettings", "generalSettings" },
		{ "PTZ", "ptz" },
		{ "PTZSettings", "ptzSettings" },
		{ "PTZSessionTimeout", "ptzSessionTimeout" }
	};

	private static readonly Dictionary<string, string> MethodToTasks = new Dictionary<string, string>
	{
		{ "GetDevicesUsingStorage", "GetAnyUsingStorage" },
		{ "GetOtherDevicesWithMediaOnRecorder", "GetOtherWithMediaOnRecorder" },
		{ "ChangeDeviceRecordingStorage", "ChangeRecordingStorage" }
	};

	private static readonly Dictionary<string, string> ItemTypeDisplayNameKey = new Dictionary<string, string> { { "User", "name" } };

	public static List<string> _multipleChildItemTypes = new List<string>
	{
		"DeviceDriverSettings", "Stream", "Claim", "SystemAddress", "HardwareDeviceEvent", "SiteAddress", "ViewItem", "PatrollingEntry", "TimeProfileAppointmentRoot", "TimeProfileAppointmentVariance",
		"TimeProfileAppointmentRecur", "LicenseInstalledProduct", "LicenseOverviewAll", "LicenseDetail", "LicenseFeature", "HardwareDeviceEvent", "Task", "PtzPreset", "HardwarePtzDeviceSetting", "User",
		"Camera", "Microphone", "Metadata", "Speaker", "Output", "InputEvent"
	};

	public List<string> UnderstoodItemTypes => Resources.Select((ResourceEntry r) => r.ItemType).ToList();

	public string GetDisplayNameKey(string itemType)
	{
		if (ItemTypeDisplayNameKey.TryGetValue(itemType, out var value))
		{
			return value;
		}
		return "displayName";
	}

	public string SupportRootFolderSuggest(string itemType)
	{
		return ItemTypesWithNoRootFolderSupport[itemType];
	}

	public bool SupportRootFolder(string itemType)
	{
		return !ItemTypesWithNoRootFolderSupport.ContainsKey(itemType);
	}

	public bool OnlyFourthLevelPostSupported(string itemType)
	{
		return ItemTypesWithOnlyFourthLevelPostSupport.Contains(itemType);
	}

	public string ResourcePluralToItemType(string resourceName)
	{
		return ResourcePluralToItemType(resourceName, throwForInvalid: true);
	}

	public string ResourcePluralToItemType(string resourceName, bool throwForInvalid)
	{
		ResourceEntry resourceEntry = Resources.FirstOrDefault((ResourceEntry r) => r.ResourceNamePlural.Equals(resourceName, StringComparison.OrdinalIgnoreCase));
		if (resourceEntry != null)
		{
			return resourceEntry.ItemType;
		}
		string tempName = resourceName + "s";
		resourceEntry = Resources.FirstOrDefault((ResourceEntry r) => r.ResourceNamePlural.Equals(tempName, StringComparison.OrdinalIgnoreCase));
		if (resourceEntry != null)
		{
			return resourceEntry.ItemType;
		}
		if (resourceName.StartsWith("clientProfile", StringComparison.Ordinal))
		{
			return "C" + resourceName.Substring(1);
		}
		string text = ChildFieldNameToItemType(resourceName);
		if (text != resourceName)
		{
			return text;
		}
		if (throwForInvalid)
		{
			throw new ResourceNotFoundException(string.Format(CultureInfo.InvariantCulture, "Unknown resource: {0}", resourceName), null);
		}
		return null;
	}

	public string ResourcePluralToSingular(string resourceName)
	{
		string text = ResourcePluralToItemType(resourceName, throwForInvalid: false);
		if (text == null)
		{
			return resourceName.TrimEnd('s');
		}
		return FirstLower(text);
	}

	public virtual ResourceEntry GetResourcePlural(string resourceName)
	{
		ResourceEntry resourceEntry = Resources.FirstOrDefault((ResourceEntry r) => r.ResourceNamePlural.Equals(resourceName, StringComparison.OrdinalIgnoreCase));
		if (resourceEntry != null)
		{
			return resourceEntry;
		}
		throw new ResourceNotFoundException(string.Format(CultureInfo.InvariantCulture, "Unknown resource: {0}", resourceName), null);
	}

	public string ResourcePluralToFolderItemType(string resourceName)
	{
		ResourceEntry resourceEntry = Resources.FirstOrDefault((ResourceEntry r) => r.ResourceNamePlural.Equals(resourceName, StringComparison.OrdinalIgnoreCase));
		if (resourceEntry != null)
		{
			return resourceEntry.ItemTypeFolder;
		}
		string tempName = resourceName + "s";
		resourceEntry = Resources.FirstOrDefault((ResourceEntry r) => r.ResourceNamePlural.Equals(tempName, StringComparison.OrdinalIgnoreCase));
		if (resourceEntry != null)
		{
			return resourceEntry.ItemTypeFolder;
		}
		throw new ResourceNotFoundException(string.Format(CultureInfo.InvariantCulture, "Unknown resource: {0}", resourceName), null);
	}

	public string FolderItemTypeToResourcePlural(string folderItemType)
	{
		if (string.IsNullOrEmpty(folderItemType))
		{
			return "sites";
		}
		string itemType = (folderItemType.EndsWith("Folder", StringComparison.Ordinal) ? folderItemType.Substring(0, folderItemType.Length - 6) : folderItemType);
		return ItemTypeToResourcePlural(itemType);
	}

	public string ItemTypeToResourcePlural(string itemType)
	{
		if (string.IsNullOrEmpty(itemType) || itemType == "System")
		{
			return "sites";
		}
		if (itemType == "InfoItemFolder")
		{
			return "itemInfoFolder";
		}
		ResourceEntry resourceEntry = Resources.FirstOrDefault((ResourceEntry r) => r.ItemType == itemType);
		if (resourceEntry != null)
		{
			return resourceEntry.ResourceNamePlural;
		}
		throw new ResourceNotFoundException("Unknown ItemType: " + itemType, null);
	}

	public string ItemTypeToResourceSingular(string itemType)
	{
		if (string.IsNullOrEmpty(itemType) || itemType == "System")
		{
			return "site";
		}
		if (itemType == "InfoItemFolder")
		{
			return "itemInfoFolder";
		}
		return FirstLower(itemType);
	}

	public static string FirstLower(string str)
	{
		if (str.Substring(0, 3).Equals("mip", StringComparison.OrdinalIgnoreCase))
		{
			return str.Substring(0, 3).ToLowerInvariant() + str.Substring(3);
		}
		return str.Substring(0, 1).ToLowerInvariant() + str.Substring(1);
	}

	public string PropertyKeyToField(string itemType, string key, string methodId)
	{
		if (methodId != null && methodId == "AddDeviceGroup")
		{
			if (key == "GroupName")
			{
				key = "Name";
			}
			if (key == "GroupDescription")
			{
				key = "Description";
			}
		}
		return PropertyKeyToField(itemType, key, methodId == "AddMIPItem");
	}

	public string FieldToPropertyKeyNameConvert(string itemType, string[] methodIds, string key)
	{
		if (itemType == "InvokeInfo" && methodIds != null && methodIds.Length != 0 && methodIds[0] == "AddDeviceGroup")
		{
			if (key == "name")
			{
				return "GroupName";
			}
			if (key == "description")
			{
				return "GroupDescription";
			}
		}
		return key;
	}

	public string PropertyKeyToField(string itemType, string key, bool noCasing)
	{
		if (itemType == null)
		{
			throw new ArgumentNullException("itemType");
		}
		if (!RestApiConvert.CaseProperty(itemType, key, noCasing))
		{
			return key;
		}
		if (string.IsNullOrEmpty(key))
		{
			throw new ArgumentNullException("key");
		}
		switch (itemType)
		{
		default:
			if (!(itemType == "HardwarePtzDeviceSetting"))
			{
				break;
			}
			goto case "DeviceDriverSettings";
		case "DeviceDriverSettings":
		case "PTZ":
		case "Stream":
		case "HardwareDriverSettings":
			if (key.Contains('.') || key.Contains(':') || key.Contains('/'))
			{
				string[] array = key.Split('/');
				if (array.Length > 2)
				{
					key = array[1];
				}
			}
			break;
		}
		if (key.StartsWith("PTZ", StringComparison.Ordinal))
		{
			return "ptz" + key.Substring(3);
		}
		if (key.StartsWith("HTTP", StringComparison.Ordinal))
		{
			return "http" + key.Substring(4);
		}
		if (key.StartsWith("AAC", StringComparison.Ordinal))
		{
			return "aac" + key.Substring(3);
		}
		if (IsAllUpperCase(key))
		{
			return key;
		}
		return (key[0].ToString() ?? "").ToLower(CultureInfo.InvariantCulture) + key.Substring(1);
	}

	private static bool IsAllUpperCase(string key)
	{
		return Regex.IsMatch(key, "^[^a-z]*$");
	}

	public string PropertyFieldToKey(string itemType, string field, bool noCasingOfMIPItems)
	{
		switch (field)
		{
		case "pan":
		case "tilt":
		case "zoom":
			return field;
		default:
			if (!RestApiConvert.CaseProperty(itemType, field, noCasingOfMIPItems))
			{
				return field;
			}
			if (field.StartsWith("ptz", StringComparison.Ordinal))
			{
				return "PTZ" + field.Substring(3);
			}
			if (field.StartsWith("http", StringComparison.Ordinal))
			{
				return "HTTP" + field.Substring(4);
			}
			if (field.StartsWith("aac", StringComparison.Ordinal))
			{
				return "AAC" + field.Substring(3);
			}
			return (field[0].ToString() ?? "").ToUpper(CultureInfo.InvariantCulture) + field.Substring(1);
		}
	}

	public string GetAddMethodId(string[] methodIds, string itemType)
	{
		if (methodIds != null)
		{
			if (itemType == "SaveSearches")
			{
				itemType = "SaveSearch";
			}
			string text = ((!(itemType == "BasicOwnerInformation")) ? ("Add" + itemType) : "AddBasicOwnerInfo");
			string text2 = "MethodIdAdd" + itemType;
			string text3 = "AddDeviceGroup";
			string text4 = "AddDeviceGroupMember";
			foreach (string text5 in methodIds)
			{
				if (text5 == text || text5 == text2 || text5 == text3 || text5 == text4)
				{
					return text5;
				}
			}
		}
		return null;
	}

	public string GetRemoveMethodId(string[] methodIds, string itemType)
	{
		if (itemType == "SaveSearches")
		{
			itemType = "SaveSearch";
		}
		string text = ((!(itemType == "BasicOwnerInformation")) ? ("Remove" + itemType) : "RemoveBasicOwnerInfo");
		string text2 = "Delete" + itemType;
		string text3 = "RemoveDeviceGroup";
		string text4 = "RemoveDeviceGroupMember";
		if (methodIds != null)
		{
			foreach (string text5 in methodIds)
			{
				if (text5 == text || text5 == text2 || text5 == text3 || text5 == text4)
				{
					return text5;
				}
				if (text5.StartsWith("MethodId", StringComparison.OrdinalIgnoreCase) && (text5.Substring("MethodId".Length) == text || text5.Substring("MethodId".Length) == text2))
				{
					return text5;
				}
			}
		}
		return null;
	}

	public string ConfigApiPathToResource(string path)
	{
		return ConfigApiPathToResource(path, full: false);
	}

	public string ConfigApiPathToResource(string configApiPath, bool full)
	{
		ConfigurationItemPath configurationItemPath = new ConfigurationItemPath(configApiPath);
		string id = configurationItemPath.Id;
		string text = configurationItemPath.ItemType;
		if (text.EndsWith("Folder", StringComparison.OrdinalIgnoreCase))
		{
			text = text.Substring(0, text.Length - 6);
		}
		if (text == "System")
		{
			text = "";
		}
		string text2 = ItemTypeToResourcePlural(text);
		if (!full)
		{
			return text2;
		}
		if (configurationItemPath.IsFolder)
		{
			if (string.IsNullOrEmpty(configurationItemPath.ParentItemType) || configurationItemPath.ParentItemType == "System")
			{
				return "/" + text2;
			}
			return ItemTypeToResourcePlural(configurationItemPath.ParentItemType) + "/" + id + "/" + text2;
		}
		return "/" + text2 + "/" + id;
	}

	public string ChildItemTypeToFieldName(string childItemType)
	{
		if (!ChildItemTypeNames.TryGetValue(childItemType, out var value))
		{
			return childItemType;
		}
		return value;
	}

	public string ChildFieldNameToItemType(string childFieldName)
	{
		foreach (KeyValuePair<string, string> childItemTypeName in ChildItemTypeNames)
		{
			if (childItemTypeName.Value == childFieldName)
			{
				return childItemTypeName.Key;
			}
		}
		foreach (ResourceEntry resource in Resources)
		{
			if (resource.ResourceNamePlural == childFieldName || resource.ResourceNamePlural == childFieldName + "s")
			{
				return resource.ItemType;
			}
		}
		return childFieldName;
	}

	public string MethodIdToTask(string methodId)
	{
		if (!MethodToTasks.TryGetValue(methodId, out var value))
		{
			return methodId;
		}
		return value;
	}

	public string TaskToMethodId(string task)
	{
		foreach (KeyValuePair<string, string> methodToTask in MethodToTasks)
		{
			if (methodToTask.Value == task)
			{
				return methodToTask.Key;
			}
		}
		return task;
	}

	public static string GetTagFromResource(string resourcePath)
	{
		string key = resourcePath.Split('/')[0];
		if (ResourcePluralTags.ContainsKey(key))
		{
			return ResourcePluralTags[key].ToString();
		}
		return null;
	}

	public bool HasMultipleChildItems(string itemType)
	{
		return _multipleChildItemTypes.Contains(itemType);
	}
}
