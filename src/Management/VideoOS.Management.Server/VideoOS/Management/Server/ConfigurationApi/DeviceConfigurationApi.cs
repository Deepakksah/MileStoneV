using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPICamera")]
[EntityName("ConfigAPICameraFolder")]
[EntityName("ConfigAPIMicrophone")]
[EntityName("ConfigAPIMicrophoneFolder")]
[EntityName("ConfigAPIInputEvent")]
[EntityName("ConfigAPIInputEventFolder")]
[EntityName("ConfigAPIOutput")]
[EntityName("ConfigAPIOutputFolder")]
[EntityName("ConfigAPISpeaker")]
[EntityName("ConfigAPISpeakerFolder")]
[EntityName("ConfigAPIMetadata")]
[EntityName("ConfigAPIMetadataFolder")]
public class DeviceConfigurationApi : SecurityConfigurationApi, IConfigurationApiSupport
{
	public const string RecordingStorageProperty = "RecordingStorage";

	public override bool SupportGetChildItemsWithItemTypes => true;

	protected override Dictionary<string, List<string>> ItemTypeActionsToExclude => new Dictionary<string, List<string>>
	{
		{
			"Camera",
			new List<string> { "ADMIN_SECURITY" }
		},
		{
			"Microphone",
			new List<string> { "ADMIN_SECURITY" }
		},
		{
			"Speaker",
			new List<string> { "ADMIN_SECURITY" }
		},
		{
			"Metadata",
			new List<string> { "ADMIN_SECURITY" }
		},
		{
			"InputEvent",
			new List<string> { "ADMIN_SECURITY" }
		},
		{
			"Output",
			new List<string> { "ADMIN_SECURITY" }
		}
	};

	public ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		ConfigurationApiBase.ThrowIfAnyParameterNull(requestContext, item, path, parentPath);
		ThrowIfPathInvalid(path);
		ThrowIfPathInvalid(parentPath);
		if (path.IsFolder)
		{
			throw new NotSupportedException();
		}
		string name;
		string description;
		bool? enabled;
		bool? recordingEnabled;
		bool? prebufferEnabled;
		bool? prebufferInMemory;
		int? prebufferSeconds;
		double? recordingFramerate;
		bool? edgeStorageEnabled;
		bool? edgeStoragePlaybackEnabled;
		bool? recordKeyframesOnly;
		bool? manualTimeoutEnabled;
		int? manualTimeoutMinutes;
		bool? recordOnRelatedDevices;
		string gisPoint;
		string shortName;
		int? icon;
		double? coverageDirection;
		double? coverageDepth;
		double? coverageFieldOfView;
		string failoverSetting;
		ValidateResult validateResult;
		try
		{
			validateResult = ValidateItemInternal(requestContext, item, path, out name, out description, out enabled, out recordingEnabled, out prebufferEnabled, out prebufferInMemory, out prebufferSeconds, out recordingFramerate, out edgeStorageEnabled, out edgeStoragePlaybackEnabled, out recordKeyframesOnly, out manualTimeoutEnabled, out manualTimeoutMinutes, out recordOnRelatedDevices, out gisPoint, out shortName, out icon, out coverageDirection, out coverageDepth, out coverageFieldOfView, out failoverSetting);
		}
		catch (UnknownDeviceException)
		{
			throw new PathNotFoundException(path.Text);
		}
		if (!validateResult.ValidatedOk)
		{
			return validateResult;
		}
		if (DeviceTypeRelation.Relations.FirstOrDefault((DeviceTypeRelation r) => string.Equals(r.ItemType, path.ItemType, StringComparison.Ordinal)) == null)
		{
			throw new PathInvalidException("Unknown item type requested. " + path.Text);
		}
		try
		{
			ConvertFailoverSetting(failoverSetting, out var failoverLiveOnly, out var failoverDisabled);
			Device device = Device.UpdateDevice(requestContext, path.Id, name, description, enabled, prebufferEnabled, prebufferInMemory, prebufferSeconds, recordingEnabled, recordingFramerate, edgeStorageEnabled, edgeStoragePlaybackEnabled, recordKeyframesOnly, manualTimeoutEnabled, manualTimeoutMinutes, recordOnRelatedDevices, gisPoint, shortName, icon, coverageDirection, coverageDepth, coverageFieldOfView, null, null, null, null, null, null, failoverLiveOnly, failoverDisabled);
			ConfigurationItem resultItem = ConvertToConfigurationItem(parentPath, device);
			validateResult = new ValidateResult();
			validateResult.ValidatedOk = true;
			validateResult.ResultItem = resultItem;
			return validateResult;
		}
		catch (DeviceUpdateException ex2)
		{
			throw new PropertyInvalidException(ex2.Message, ex2);
		}
	}

	public ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		ConfigurationApiBase.ThrowIfAnyParameterNull(requestContext, item, path, parentPath);
		ThrowIfPathInvalid(path);
		ThrowIfPathInvalid(parentPath);
		if (!path.IsFolder)
		{
			if (!Device.DeviceExists(requestContext, path.Id))
			{
				throw new PathNotFoundException(path.Text);
			}
			try
			{
				string name;
				string description;
				bool? enabled;
				bool? recordingEnabled;
				bool? prebufferEnabled;
				bool? prebufferInMemory;
				int? prebufferSeconds;
				double? recordingFramerate;
				bool? edgeStorageEnabled;
				bool? edgeStoragePlaybackEnabled;
				bool? recordKeyframesOnly;
				bool? manualTimeoutEnabled;
				int? manualTimeoutMinutes;
				bool? recordOnRelatedDevices;
				string gisPoint;
				string shortName;
				int? icon;
				double? coverageDirection;
				double? coverageDepth;
				double? coverageFieldOfView;
				string failoverSetting;
				return ValidateItemInternal(requestContext, item, path, out name, out description, out enabled, out recordingEnabled, out prebufferEnabled, out prebufferInMemory, out prebufferSeconds, out recordingFramerate, out edgeStorageEnabled, out edgeStoragePlaybackEnabled, out recordKeyframesOnly, out manualTimeoutEnabled, out manualTimeoutMinutes, out recordOnRelatedDevices, out gisPoint, out shortName, out icon, out coverageDirection, out coverageDepth, out coverageFieldOfView, out failoverSetting);
			}
			catch (UnknownDeviceException)
			{
				throw new PathNotFoundException(path.Text);
			}
		}
		throw new NotSupportedException();
	}

	private static ValidateResult ValidateItemInternal(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, out string name, out string description, out bool? enabled, out bool? recordingEnabled, out bool? prebufferEnabled, out bool? prebufferInMemory, out int? prebufferSeconds, out double? recordingFramerate, out bool? edgeStorageEnabled, out bool? edgeStoragePlaybackEnabled, out bool? recordKeyframesOnly, out bool? manualTimeoutEnabled, out int? manualTimeoutMinutes, out bool? recordOnRelatedDevices, out string gisPoint, out string shortName, out int? icon, out double? coverageDirection, out double? coverageDepth, out double? coverageFieldOfView, out string failoverSetting)
	{
		name = null;
		description = null;
		enabled = null;
		recordingEnabled = null;
		prebufferEnabled = null;
		prebufferInMemory = null;
		prebufferSeconds = null;
		recordingFramerate = null;
		edgeStorageEnabled = null;
		edgeStoragePlaybackEnabled = null;
		recordKeyframesOnly = null;
		manualTimeoutEnabled = null;
		manualTimeoutMinutes = null;
		recordOnRelatedDevices = null;
		gisPoint = null;
		shortName = null;
		icon = null;
		coverageDirection = null;
		coverageDepth = null;
		coverageFieldOfView = null;
		failoverSetting = null;
		try
		{
			if (!path.IsFolder)
			{
				if (item.Properties != null)
				{
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Name", out name);
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Description", out description);
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("RecordingEnabled", out recordingEnabled);
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("PrebufferEnabled", out prebufferEnabled);
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("PrebufferInMemory", out prebufferInMemory);
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("PrebufferSeconds", out prebufferSeconds);
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("EdgeStorageEnabled", out edgeStorageEnabled);
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("EdgeStoragePlaybackEnabled", out edgeStoragePlaybackEnabled);
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("RecordingFramerate", out recordingFramerate);
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("RecordKeyframesOnly", out recordKeyframesOnly);
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("RecordOnRelatedDevices", out recordOnRelatedDevices);
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("ManualRecordingTimeoutEnabled", out manualTimeoutEnabled);
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("ManualRecordingTimeoutMinutes", out manualTimeoutMinutes);
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("GisPoint", out gisPoint);
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("ShortName", out shortName);
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Icon", out icon);
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("CoverageDirection", out coverageDirection);
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("CoverageDepth", out coverageDepth);
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("CoverageFieldOfView", out coverageFieldOfView);
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("FailoverSetting", out failoverSetting);
					ConvertFailoverSetting(failoverSetting, out var _, out var _);
				}
				if (item.EnableProperty != null)
				{
					enabled = item.EnableProperty.Enabled;
				}
				Device.UpdateDeviceValidation(requestContext, path.Id, name, description, enabled, recordingEnabled, prebufferEnabled, prebufferInMemory, prebufferSeconds, recordingFramerate, edgeStorageEnabled, edgeStoragePlaybackEnabled, recordKeyframesOnly, manualTimeoutEnabled, manualTimeoutMinutes, recordOnRelatedDevices, gisPoint, shortName, icon, coverageDirection, coverageDepth, coverageFieldOfView, null);
				ValidateResult validateResult = new ValidateResult();
				validateResult.ValidatedOk = true;
				validateResult.ResultItem = item;
				return validateResult;
			}
			throw new NotSupportedException();
		}
		catch (ArgumentException exception)
		{
			ValidateResult validateResult = ErrorHelper.CreateValidateResult(exception, item.Properties);
			if (validateResult != null)
			{
				return validateResult;
			}
			throw;
		}
	}

	public ConfigurationItem[] GetChildItems(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		return GetChildItems(requestContext, path, null, new ItemFilter[0], int.MaxValue, 0, null, new ItemFilter[0]);
	}

	public override ConfigurationItem[] GetChildItems(IGeminiRequestContext requestContext, ConfigurationItemPath path, string[] itemTypes, ItemFilter[] itemFilters, int maxResult, int startIndex, string[] childItemTypes, ItemFilter[] childItemFilters)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		if (itemFilters == null)
		{
			throw new ArgumentNullException("itemFilters");
		}
		ThrowIfPathInvalid(path);
		if (path.IsFolder)
		{
			DeviceTypeRelation relation = DeviceTypeRelation.Relations.FirstOrDefault((DeviceTypeRelation r) => string.Equals(r.FolderType, path.ItemType, StringComparison.Ordinal));
			if (relation == null)
			{
				throw new PathInvalidException("Unexpected item type. " + path.Text);
			}
			ItemFilter itemFilter = itemFilters.FirstOrDefault((ItemFilter f) => f.ItemType == relation.ItemType);
			DeviceTypeSearch deviceTypeSearch = ConvertItemTypeToDeviceTypeSearch(path.ItemType);
			string text = string.Empty;
			Device[] array2;
			if (string.Equals("Hardware", path.ParentItemType, StringComparison.Ordinal))
			{
				text = path.Id;
				string[] array = Device.QueryDeviceIdsGeneric(requestContext, null, text, deviceTypeSearch);
				array2 = ((array.Length == 0) ? new Device[0] : Device.QueryDevicesByIds(requestContext, array));
			}
			else
			{
				if (!DeviceTypeRelation.Relations.Any((DeviceTypeRelation r) => string.Equals(r.GroupType, path.ParentItemType, StringComparison.Ordinal)))
				{
					throw new NotSupportedException();
				}
				DeviceType deviceType = relation.DeviceType;
				string id = path.Id;
				string filter = string.Empty;
				bool checkDevicesEnabled = false;
				if (itemFilter != null)
				{
					if (itemFilter.EnableFilter == EnableFilter.Enabled)
					{
						checkDevicesEnabled = true;
					}
					filter = QueryFormatter.ConstructSqlSelect(GetPossibleQueryProperties(deviceType), keysToTranslate: GetQueryKeyTranslations(), itemType: relation.ItemType, itemFilter: itemFilter);
				}
				array2 = Device.QueryDevicesByFilter(requestContext, id, deviceTypeSearch, filter, maxResult, startIndex, checkDevicesEnabled);
			}
			if (!string.IsNullOrEmpty(text) && array2.Length == 0 && Hardware.QueryHardwareById(requestContext, text) == null)
			{
				throw new PathNotFoundException();
			}
			List<ConfigurationItem> list = new List<ConfigurationItem>();
			Device[] array3 = array2;
			foreach (Device device in array3)
			{
				ConfigurationItem configurationItem = ConvertToConfigurationItem(ConfigurationItemPath.CreateFolderPath("Hardware", device.HardwareId, path.ItemType), device);
				if (itemTypes == null || itemTypes.Length == 0 || (itemTypes.Contains(configurationItem.ItemType) && ConfigurationApiBase.Match(configurationItem, itemFilter)))
				{
					list.Add(configurationItem);
				}
				if (list.Count >= maxResult)
				{
					break;
				}
			}
			ConfigurationItem[] array4 = list.ToArray();
			if (childItemTypes != null && childItemTypes.Any())
			{
				FillChildren(requestContext, array4, childItemTypes, childItemFilters);
			}
			return array4;
		}
		List<ConfigurationItem> list2 = new List<ConfigurationItem>();
		if (itemTypes == null || itemTypes.Contains("DeviceDriverSettings"))
		{
			list2.Add(ConvertToDeviceSettingsFolderConfigurationItem(path));
		}
		if (string.Equals("Camera", path.ItemType, StringComparison.Ordinal))
		{
			Device device2 = Device.QueryDeviceById(requestContext, path.Id);
			if (device2 == null)
			{
				throw new PathNotFoundException(path.Text);
			}
			if (device2.DeviceType != DeviceType.Camera)
			{
				throw new PathNotFoundException(path.Text);
			}
			if ((itemTypes == null || itemTypes.Contains("PtzPreset")) && HasPtzSupport(device2))
			{
				list2.Add(GroupConfigurationItemFactory.CreateFolderItem("PtzPresetFolder", path));
				list2.Add(GroupConfigurationItemFactory.CreateFolderItem("PatrollingProfileFolder", path));
			}
			if ((itemTypes == null || itemTypes.Contains("FisheyeLens")) && FisheyeConfigurationApi.HasFisheyeSupport(device2))
			{
				list2.Add(GroupConfigurationItemFactory.CreateFolderItem("FisheyeLensFolder", path));
			}
			if (itemTypes == null || itemTypes.Contains("Stream"))
			{
				list2.Add(GroupConfigurationItemFactory.CreateFolderItem("StreamFolder", path));
			}
			if (itemTypes == null || itemTypes.Contains("MotionDetection"))
			{
				list2.Add(GroupConfigurationItemFactory.CreateFolderItem("MotionDetectionFolder", path));
			}
			if (itemTypes == null || itemTypes.Contains("PrivacyProtection"))
			{
				list2.Add(GroupConfigurationItemFactory.CreateFolderItem("PrivacyProtectionFolder", path));
			}
		}
		if (itemTypes == null || itemTypes.Contains("CustomProperties"))
		{
			list2.Add(GroupConfigurationItemFactory.CreateFolderItem("CustomPropertiesFolder", path));
		}
		if ((itemTypes == null || itemTypes.Contains("ClientSettings")) && new List<string> { "Camera", "Microphone", "Speaker", "Metadata" }.Contains(path.ItemType))
		{
			list2.Add(GroupConfigurationItemFactory.CreateFolderItem("ClientSettingsFolder", path));
		}
		if (itemTypes == null || itemTypes.Contains("HardwareDeviceEvent"))
		{
			list2.Add(GroupConfigurationItemFactory.CreateFolderItem("HardwareDeviceEventFolder", path));
		}
		ConfigurationItem[] array5 = list2.ToArray();
		if (childItemTypes != null && childItemTypes.Any())
		{
			FillChildren(requestContext, array5, childItemTypes, childItemFilters);
		}
		return array5;
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
		ThrowIfPathInvalid(path);
		if (path.IsFolder)
		{
			if (string.Equals(path.ParentItemType, "Hardware", StringComparison.Ordinal))
			{
				string id = path.Id;
				if (!Hardware.HardwareExists(requestContext, id))
				{
					throw new PathNotFoundException(path.Text);
				}
				ConfigurationItemPath parentPath = ConfigurationItemPath.CreateItemPath("Hardware", path.Id);
				return GroupConfigurationItemFactory.CreateFolderItem(path.ItemType, parentPath);
			}
			DeviceTypeRelation deviceTypeRelation = DeviceTypeRelation.Relations.FirstOrDefault((DeviceTypeRelation r) => string.Equals(r.GroupType, path.ParentItemType, StringComparison.Ordinal));
			if (deviceTypeRelation != null)
			{
				try
				{
					string id2 = path.Id;
					DeviceGroup deviceGroup = DeviceGroup.QueryDeviceGroupById(requestContext, id2, deviceTypeRelation.DeviceType);
					if (!DeviceTypeRelation.Relations.Any((DeviceTypeRelation r) => r.DeviceType == deviceGroup.DeviceType && string.Equals(r.GroupType, path.ParentItemType, StringComparison.Ordinal)))
					{
						throw new PathNotFoundException(path.Text);
					}
				}
				catch (DeviceGroupNotFoundException)
				{
					throw new PathNotFoundException(path.Text);
				}
				ConfigurationItemPath parentPath2 = ConfigurationItemPath.CreateItemPath(path.ParentItemType, path.Id);
				return GroupConfigurationItemFactory.CreateFolderItem(path.ItemType, parentPath2);
			}
			throw new NotSupportedException();
		}
		DeviceTypeRelation deviceTypeRelation2 = DeviceTypeRelation.Relations.FirstOrDefault((DeviceTypeRelation r) => string.Equals(r.ItemType, path.ItemType, StringComparison.Ordinal));
		if (deviceTypeRelation2 == null)
		{
			throw new PathInvalidException("Unknown item type requested. " + path.Text);
		}
		Device device = Device.QueryDeviceById(requestContext, path.Id);
		if (device == null)
		{
			throw new PathNotFoundException("Device not found. " + path.Text);
		}
		if (device.DeviceType != deviceTypeRelation2.DeviceType)
		{
			throw new PathNotFoundException(path.Text);
		}
		string hardwareId = device.HardwareId;
		string folderType = deviceTypeRelation2.FolderType;
		return ConvertToConfigurationItem(ConfigurationItemPath.CreateFolderPath("Hardware", hardwareId, folderType), device);
	}

	public ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, string methodId)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		if (methodId == null)
		{
			throw new ArgumentNullException("methodId");
		}
		if (!item.MethodIds.Contains(methodId))
		{
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Invalid MethodId {0} on ItemType: {1}", methodId, item.ItemType), "methodId");
		}
		bool flag = string.Equals(item.ItemType, "InvokeInfo", StringComparison.Ordinal);
		switch (methodId)
		{
		case "AddDeviceGroupMember":
			if (!flag)
			{
				return DeviceGroupConfigurationApi.CreateWizardItemAddMember(item, methodId);
			}
			return DeviceGroupConfigurationApi.AddDeviceGroupMember(requestContext, item, path);
		case "RemoveDeviceGroupMember":
			if (!flag)
			{
				return DeviceGroupConfigurationApi.CreateWizardItemRemoveMember(item, GetChildItems(requestContext, path), methodId);
			}
			return DeviceGroupConfigurationApi.RemoveDeviceGroupMember(requestContext, item, path);
		case "GetOtherRecordingServersWithMedia":
			if (!flag)
			{
				return CreateInvokeInfoItemGetOtherRecordingServersWithMedia(item, methodId);
			}
			return GetOtherRecordingServersWithMedia(requestContext, item, path);
		case "GetOtherRecordingStoragesWithMedia":
			if (!flag)
			{
				return CreateInvokeInfoItemGetOtherRecordingStoragesWithMedia(item, methodId);
			}
			return GetOtherRecordingStoragesWithMedia(requestContext, item, path);
		case "ChangeSecurityPermissions":
			return InvokeSecurityMethod(requestContext, item, path, methodId);
		case "RetrieveEdgeStorage":
			if (!flag)
			{
				return CreateInvokeInfoItemRetrieveEdgeStorage(item, methodId);
			}
			return RetrieveEdgeStorage(requestContext, item, path);
		case "TruncateStorage":
			if (!flag)
			{
				return CreateInvokeInfoItemTruncateStorage(item, methodId);
			}
			return TruncateStorage(requestContext, item, path);
		case "TruncateStorageOnOtherRecordingServer":
			if (!flag)
			{
				return CreateInvokeInfoItemTruncateStorageOnOtherRecordingServer(item, GetRecordingServersWithMedia(requestContext, path), methodId);
			}
			return TruncateStorageOnOtherRecordingServer(requestContext, item, path);
		case "ChangeDeviceRecordingStorage":
			if (!flag)
			{
				return CreateInvokeInfoItemChangeDeviceRecordingStorage(item, GetRecordingStoragesOnRecordingServer(requestContext, path), methodId);
			}
			return ChangeDeviceRecordingStorage(requestContext, item, path);
		default:
			throw new ArgumentException("Unknown methodId:" + methodId, "methodId");
		}
	}

	private static ConfigurationItem CreateInvokeInfoItemChangeDeviceRecordingStorage(ConfigurationItem item, ConfigurationItem[] storages, string methodId)
	{
		ConfigurationItem configurationItem = MethodTypesHelper.CreateInvokeMemberSelectionInfo(methodId, ConfigurationApiResources.WizardChangeDeviceRecordingStorage, "PropertySelectStorageToMoveTo", ConfigurationApiResources.PropertySelectStorageToMoveTo, item, storages);
		configurationItem.Properties = new List<Property>(configurationItem.Properties) { RecordingDevicePropertyDefinition.MoveDeviceData(moveData: true) }.ToArray();
		return configurationItem;
	}

	private static ConfigurationItem ChangeDeviceRecordingStorage(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		if (!IsRecordingDevicePath(path))
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		ConfigurationItemPath selectedItemPath = MethodTypesHelper.GetInvokeMemberItemSelectionPath(item);
		if (!GetRecordingStoragesOnRecordingServer(requestContext, path).Any((ConfigurationItem s) => s.Properties.FirstOrDefault((Property p) => p.Key.Equals("Id")).Value.Equals(selectedItemPath.Id, StringComparison.OrdinalIgnoreCase)))
		{
			throw new ArgumentException("Invalid storage selection", selectedItemPath.Text);
		}
		string id = path.Id;
		return TaskHelperFunctions.CreateTaskInvokeResultConfigurationItem(Task.ChangeDeviceRecordingStorage(requestContext, id, selectedItemPath.Id), item);
	}

	private static ConfigurationItem GetOtherRecordingServersWithMedia(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		if (!IsRecordingDevicePath(path))
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		string id = path.Id;
		string[] recordingServerIds = Device.QueryOtherRecordingServersWithMedia(requestContext, id);
		RecordingServer[] array = RecordingServer.QueryRecordingServersByIds(requestContext, recordingServerIds);
		List<ConfigurationItem> list = new List<ConfigurationItem>();
		RecordingServer[] array2 = array;
		foreach (RecordingServer recordingServer in array2)
		{
			list.Add(RecordingServerConfigurationApi.ConvertToConfigurationItem(recordingServer));
		}
		ConfigurationItem configurationItem = MethodTypesHelper.CreateInvokeResult(item, null, "Result", "Success", true);
		configurationItem.Children = list.ToArray();
		configurationItem.ChildrenFilled = true;
		return configurationItem;
	}

	private static ConfigurationItem GetOtherRecordingStoragesWithMedia(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		if (!IsRecordingDevicePath(path))
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		string id = path.Id;
		string[] ids = RecordingStorage.QueryRecordingStorageIdsByDeviceId(requestContext, id);
		RecordingStorage[] array = RecordingStorage.QueryRecordingStoragesByIds(requestContext, ids);
		List<ConfigurationItem> list = new List<ConfigurationItem>();
		RecordingStorage[] array2 = array;
		foreach (RecordingStorage recordingStorage in array2)
		{
			ConfigurationItemPath parentPath = ConfigurationItemPath.CreateItemPath("RecordingServer", recordingStorage.RecordingServerId);
			list.Add(RecordingStorageConfigurationApi.ConvertToStorageConfigurationItem(parentPath, recordingStorage));
		}
		ConfigurationItem configurationItem = MethodTypesHelper.CreateInvokeResult(item, null, "Result", "Success", true);
		configurationItem.Children = list.ToArray();
		configurationItem.ChildrenFilled = true;
		return configurationItem;
	}

	private static ConfigurationItem TruncateStorage(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		if (!IsRecordingDevicePath(path))
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		string id = path.Id;
		Task task = null;
		try
		{
			task = Task.TruncateDeviceStorage(requestContext, id, null);
		}
		catch (FeatureNotLicensedException)
		{
			throw new LicenseException("Truncate device storage");
		}
		return TaskHelperFunctions.CreateTaskInvokeResultConfigurationItem(task, item);
	}

	private static ConfigurationItem TruncateStorageOnOtherRecordingServer(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		ConfigurationItemPath invokeMemberItemSelectionPath = MethodTypesHelper.GetInvokeMemberItemSelectionPath(item);
		string id = path.Id;
		Task task = null;
		try
		{
			task = Task.TruncateDeviceStorage(requestContext, id, invokeMemberItemSelectionPath.Id);
		}
		catch (FeatureNotLicensedException)
		{
			throw new LicenseException("Truncate device storage");
		}
		return TaskHelperFunctions.CreateTaskInvokeResultConfigurationItem(task, item);
	}

	private static ConfigurationItem RetrieveEdgeStorage(IGeminiRequestContext requestContext, ConfigurationItem invokeInfoItem, ConfigurationItemPath path)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (invokeInfoItem == null)
		{
			throw new ArgumentNullException("invokeInfoItem");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		if (!IsRecordingDevicePath(path))
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		if (!((IEnumerable<Property>)invokeInfoItem.Properties).TryGetPropertyValue("startTime", out DateTime? value))
		{
			throw new PropertyInvalidException("Start time is missing");
		}
		if (!((IEnumerable<Property>)invokeInfoItem.Properties).TryGetPropertyValue("endTime", out DateTime? value2))
		{
			throw new PropertyInvalidException("End time is missing");
		}
		Guid result = Guid.Empty;
		if (((IEnumerable<Property>)invokeInfoItem.Properties).TryGetPropertyValue("Identifier", out string value3) && !Guid.TryParse(value3, out result))
		{
			throw new PropertyInvalidException("Identifier property must be valid GUID");
		}
		if (result == Guid.Empty)
		{
			result = Guid.NewGuid();
			List<Property> list = new List<Property>(invokeInfoItem.Properties.Where((Property p) => p.Key != "Identifier"));
			list.Add(RecordingDeviceMethodPropertyDefinition.IdentifierProperty(result));
			invokeInfoItem.Properties = list.ToArray();
		}
		List<Guid> list2 = new List<Guid>();
		Property property = invokeInfoItem.Properties.FirstOrDefault((Property p) => p.Key == "RelatedTasks");
		if (property != null && property.ValueArray != null)
		{
			string[] valueArray = property.ValueArray;
			for (int num = 0; num < valueArray.Length; num++)
			{
				if (!Guid.TryParse(valueArray[num], out var result2))
				{
					throw new PropertyInvalidException("Related tasks must be valid GUIDs", "RelatedTasks");
				}
				list2.Add(result2);
			}
		}
		string metadata = EdgeRetrievalMetadata.Create(requestContext.UserDomainAccountName, result, list2);
		Task task = null;
		try
		{
			string id = path.Id;
			task = Task.RequestEdgeStorageRetrieval(requestContext, id, value.Value, value2.Value, metadata);
		}
		catch (FeatureNotLicensedException)
		{
			throw new LicenseException("Retrieving edge storage");
		}
		invokeInfoItem = TaskHelperFunctions.CreateTaskInvokeResultConfigurationItem(task, invokeInfoItem);
		return invokeInfoItem;
	}

	private static ConfigurationItem CreateInvokeInfoItemGetOtherRecordingServersWithMedia(ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionHorizontal";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.MethodGetOtherRecordingServersWithMedia;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.Properties = new List<Property>().ToArray();
		configurationItem.MethodIds = new string[1] { methodId };
		return configurationItem;
	}

	private static ConfigurationItem CreateInvokeInfoItemGetOtherRecordingStoragesWithMedia(ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionHorizontal";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.MethodGetOtherRecordingStoragesWithMedia;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.Properties = new List<Property>().ToArray();
		configurationItem.MethodIds = new string[1] { methodId };
		return configurationItem;
	}

	private static ConfigurationItem CreateInvokeInfoItemTruncateStorage(ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionHorizontal";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.MethodTruncateStorage;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.Properties = new List<Property>().ToArray();
		configurationItem.MethodIds = new string[1] { methodId };
		return configurationItem;
	}

	private static ConfigurationItem CreateInvokeInfoItemTruncateStorageOnOtherRecordingServer(ConfigurationItem item, ConfigurationItem[] recordingServers, string methodId)
	{
		return MethodTypesHelper.CreateInvokeMemberSelectionInfo(methodId, ConfigurationApiResources.WizardTruncateStorageOnOtherRecordingServer, "PropertySelectRecorderToTruncateStorageOn", ConfigurationApiResources.PropertySelectRecorderToTruncateStorageOn, item, recordingServers);
	}

	private static ConfigurationItem CreateInvokeInfoItemRetrieveEdgeStorage(ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionHorizontal";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.MethodRetrieveEdgeStorage;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		List<Property> list = new List<Property>
		{
			RecordingDeviceMethodPropertyDefinition.StartTimeProperty(DateTime.UtcNow - TimeSpan.FromMinutes(10.0)),
			RecordingDeviceMethodPropertyDefinition.EndTimeProperty(DateTime.UtcNow),
			RecordingDeviceMethodPropertyDefinition.IdentifierProperty(Guid.Empty),
			RecordingDeviceMethodPropertyDefinition.RelatedTasksProperty(new List<Guid>())
		};
		configurationItem.Properties = list.ToArray();
		configurationItem.MethodIds = new string[1] { methodId };
		return configurationItem;
	}

	private static ConfigurationItem[] GetRecordingStoragesOnRecordingServer(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		string id = path.Id;
		Device device = null;
		RecordingStorage[] array = null;
		device = Device.QueryDeviceById(requestContext, id);
		if (device == null)
		{
			throw new PathNotFoundException(path.Id);
		}
		try
		{
			array = RecordingStorage.QueryRecordingStorages(requestContext, device.RecordingServerId);
		}
		catch (DeviceNotFoundException)
		{
			throw new PathNotFoundException(path.Id);
		}
		ConfigurationItemPath parentPath = ConfigurationItemPath.CreateItemPath("RecordingServer", device.RecordingServerId);
		List<ConfigurationItem> list = new List<ConfigurationItem>();
		RecordingStorage[] array2 = array;
		foreach (RecordingStorage recordingStorage in array2)
		{
			list.Add(RecordingStorageConfigurationApi.ConvertToStorageConfigurationItem(parentPath, recordingStorage));
		}
		return list.ToArray();
	}

	private static ConfigurationItem[] GetRecordingServersWithMedia(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		string id = path.Id;
		string[] array = null;
		RecordingServer[] array2 = null;
		try
		{
			array = Device.QueryOtherRecordingServersWithMedia(requestContext, id);
			array2 = RecordingServer.QueryRecordingServersByIds(requestContext, array);
		}
		catch (DeviceNotFoundException)
		{
			throw new PathNotFoundException(path.Id);
		}
		List<ConfigurationItem> list = new List<ConfigurationItem>();
		RecordingServer[] array3 = array2;
		foreach (RecordingServer recordingServer in array3)
		{
			list.Add(RecordingServerConfigurationApi.ConvertToConfigurationItem(recordingServer));
		}
		return list.ToArray();
	}

	private static void ThrowIfPathInvalid(ConfigurationItemPath path)
	{
		if (path.IsFolder)
		{
			if (!string.Equals(path.ParentItemType, "Hardware", StringComparison.Ordinal) && !DeviceTypeRelation.Relations.Any((DeviceTypeRelation r) => string.Equals(r.GroupType, path.ParentItemType, StringComparison.Ordinal)))
			{
				throw new PathInvalidException("Invalid parent. " + path.Text);
			}
			if (DeviceTypeRelation.Relations.Any((DeviceTypeRelation r) => string.Equals(r.GroupType, path.ParentItemType, StringComparison.Ordinal) && !string.Equals(r.FolderType, path.ItemType, StringComparison.Ordinal)))
			{
				throw new PathInvalidException("Parent type and item type mismatch. " + path.Text);
			}
			if (!path.Id.IsGuid())
			{
				throw new PathInvalidException("Id format invalid. " + path.Text);
			}
		}
		else
		{
			if (!DeviceTypeRelation.Relations.Any((DeviceTypeRelation r) => string.Equals(r.ItemType, path.ItemType, StringComparison.Ordinal)))
			{
				throw new PathInvalidException("Unsupported item type. " + path.Text);
			}
			if (!path.Id.IsGuid())
			{
				throw new PathInvalidException("Id format invalid. " + path.Text);
			}
		}
	}

	private static bool IsRecordingDevicePath(ConfigurationItemPath path)
	{
		if (string.Equals(path.ItemType, "Camera", StringComparison.Ordinal) || string.Equals(path.ItemType, "Speaker", StringComparison.Ordinal) || string.Equals(path.ItemType, "Microphone", StringComparison.Ordinal) || string.Equals(path.ItemType, "Metadata", StringComparison.Ordinal))
		{
			return true;
		}
		return false;
	}

	public static bool HasPtzSupport(Device cameraDevice)
	{
		return cameraDevice.SettingUtilities.IsPtzEnabled;
	}

	public static ConfigurationItem ConvertToConfigurationItem(ConfigurationItemPath parentPath, Device device)
	{
		DeviceTypeRelation deviceTypeRelation = DeviceTypeRelation.Relations.FirstOrDefault((DeviceTypeRelation r) => r.DeviceType == device.DeviceType);
		if (deviceTypeRelation == null)
		{
			throw new InternalServerException();
		}
		ConfigurationItem obj = new ConfigurationItem
		{
			ItemType = deviceTypeRelation.ItemType,
			UIHint = "DirectionHorizontal",
			ItemCategory = ItemCategories.Item,
			DisplayName = device.Name,
			ParentPath = (parentPath.IsFolder ? parentPath.Text : ConfigurationItemPath.CreateFolderPath("Hardware", parentPath.Id, deviceTypeRelation.FolderType).Text)
		};
		ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreateItemPath(obj.ItemType, device.Id);
		obj.Path = configurationItemPath.Text;
		obj.EnableProperty = GeneralPropertyDefinitions.GetEnabledProperty(device.Enabled, uiFollowsEnabled: true);
		int num = 1;
		List<Property> list = new List<Property>
		{
			GeneralPropertyDefinitions.GetIdPropertyGuid(device.Id),
			GeneralPropertyDefinitions.GetNameProperty(device.Name),
			GeneralPropertyDefinitions.GetChannelProperty(device.DeviceIndex, num++),
			GeneralPropertyDefinitions.GetDescriptionProperty(device.Description)
		};
		Property createdDateProperty = GeneralPropertyDefinitions.GetCreatedDateProperty(device.CreatedDate);
		createdDateProperty.DisplayName = ConfigurationApiResources.PropertyReservedForFutureUse;
		createdDateProperty.TranslationId = "PropertyReservedForFutureUse";
		createdDateProperty.ToolTipTranslationId = "PropertyReservedForFutureUse";
		list.Add(createdDateProperty);
		list.Add(GeneralPropertyDefinitions.GetLastModifiedProperty(device.LastModified));
		list.Add(GeneralPropertyDefinitions.GetGisPointProperty(device.GisPoint));
		list.Add(GeneralPropertyDefinitions.GetShortNameProperty(device.ShortName));
		list.Add(GeneralPropertyDefinitions.GetIconProperty(device.Icon));
		list.Add(GeneralPropertyDefinitions.GetCoverageDirectionProperty(device.CoverageDirection));
		list.Add(GeneralPropertyDefinitions.GetCoverageDepthProperty(device.CoverageDepth));
		list.Add(GeneralPropertyDefinitions.GetCoverageFieldOfViewProperty(device.CoverageFieldOfView));
		List<string> list2 = new List<string>();
		if (device.DeviceType == DeviceType.Camera)
		{
			list.Add(CameraPropertyDefinition.RecordingFramerateProperty(device.RecordingFramerate));
			list.Add(CameraPropertyDefinition.RecordKeyFramesOnlyProperty(device.RecordKeyframesOnly));
			list.Add(CameraPropertyDefinition.RecordOnRelatedDevicesProperty(device.RecordOnRelatedDevices));
			list.Add(CameraPropertyDefinition.PtzEnabledProperty(device.PtzEnabled));
		}
		if (DeviceTypeUtilities.IsRecordingDevice(device.DeviceType))
		{
			list.Add(RecordingDevicePropertyDefinition.RecordingEnabledProperty(device.RecordingEnabled));
			list.Add(RecordingDevicePropertyDefinition.PrebufferEnabledProperty(device.PrebufferEnabled));
			list.Add(RecordingDevicePropertyDefinition.PrebufferInMemoryProperty(device.PrebufferInMemory));
			list.Add(RecordingDevicePropertyDefinition.PrebufferSecondsProperty(device.PrebufferSeconds));
			list.Add(RecordingDevicePropertyDefinition.EdgeStorageEnabledProperty(device.EdgeStorageEnabled));
			list.Add(RecordingDevicePropertyDefinition.EdgeStoragePlaybackEnabledProperty(device.EdgeStoragePlayback));
			list.Add(RecordingDevicePropertyDefinition.ManualRecordingTimeoutEnabledProperty(device.ManualTimeoutEnabled));
			list.Add(RecordingDevicePropertyDefinition.ManualRecordingTimeoutMinutesProperty(device.ManualTimeoutMinutes));
			ConfigurationItemPath path = new ConfigurationItemPathBuilder("Storage", device.RecordingStorageId).CreatePath();
			list.Add(RecordingDevicePropertyDefinition.RecordingStoragePathProperty(path));
			list.Add(RecordingDevicePropertyDefinition.FailoverSetting(device.FailoverLiveOnly, device.DisableOnFailover));
			list2.Add("GetOtherRecordingServersWithMedia");
			list2.Add("GetOtherRecordingStoragesWithMedia");
			list2.Add("RetrieveEdgeStorage");
			list2.Add("TruncateStorage");
			list2.Add("TruncateStorageOnOtherRecordingServer");
			list2.Add("ChangeDeviceRecordingStorage");
		}
		obj.Properties = list.ToArray();
		obj.MethodIds = list2.ToArray();
		obj.MethodIds = SecurityConfigurationApiBase.AddSecurityMethods(obj.MethodIds);
		List<ConfigurationItem> list3 = new List<ConfigurationItem>();
		obj.Children = list3.ToArray();
		obj.ChildrenFilled = false;
		return obj;
	}

	private static ConfigurationItem ConvertToDeviceSettingsFolderConfigurationItem(ConfigurationItemPath parentPath)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "DeviceDriverSettingsFolder";
		configurationItem.UIHint = "DirectionHorizontal";
		configurationItem.ItemCategory = ItemCategories.Group;
		configurationItem.DisplayName = ConfigurationApiResources.ItemTypeDeviceDriverSettingsFolder;
		configurationItem.ParentPath = parentPath.Text;
		ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreateFolderPath(parentPath.ItemType, parentPath.Id, configurationItem.ItemType);
		configurationItem.Path = configurationItemPath.Text;
		configurationItem.EnableProperty = null;
		configurationItem.Properties = new Property[0];
		configurationItem.ChildrenFilled = false;
		return configurationItem;
	}

	public static DeviceTypeSearch ConvertItemTypeToDeviceTypeSearch(string itemType)
	{
		switch (itemType)
		{
		case "CameraFolder":
		case "Camera":
			return DeviceTypeSearch.Camera;
		case "MicrophoneFolder":
		case "Microphone":
			return DeviceTypeSearch.Microphone;
		case "SpeakerFolder":
		case "Speaker":
			return DeviceTypeSearch.Speaker;
		case "InputEventFolder":
		case "InputEvent":
			return DeviceTypeSearch.Input;
		case "OutputFolder":
		case "Output":
			return DeviceTypeSearch.Output;
		case "MetadataFolder":
		case "Metadata":
			return DeviceTypeSearch.Metadata;
		default:
			return DeviceTypeSearch.All;
		}
	}

	public override bool SetSecurityOnRoleOnly()
	{
		return true;
	}

	private static List<Property> GetPossibleQueryProperties(DeviceType deviceType)
	{
		List<Property> list = new List<Property>();
		list.Add(new Property("Enabled", "True")
		{
			ValueType = "Tick"
		});
		list.Add(GeneralPropertyDefinitions.GetIdPropertyGuid(Guid.Empty.ToString()));
		list.Add(GeneralPropertyDefinitions.GetNameProperty(""));
		list.Add(GeneralPropertyDefinitions.GetChannelProperty(0, 0));
		list.Add(GeneralPropertyDefinitions.GetDescriptionProperty(""));
		list.Add(GeneralPropertyDefinitions.GetLastModifiedProperty(DateTime.Now));
		list.Add(GeneralPropertyDefinitions.GetShortNameProperty(""));
		list.Add(GeneralPropertyDefinitions.GetCoverageDirectionProperty(0.0));
		list.Add(GeneralPropertyDefinitions.GetCoverageDepthProperty(0.0));
		list.Add(GeneralPropertyDefinitions.GetCoverageFieldOfViewProperty(0.0));
		if (deviceType == DeviceType.Camera)
		{
			list.Add(CameraPropertyDefinition.RecordingFramerateProperty(0.0));
			list.Add(CameraPropertyDefinition.RecordKeyFramesOnlyProperty(recordKeyFramesOnly: true));
			list.Add(CameraPropertyDefinition.RecordOnRelatedDevicesProperty(recordOnRelatedDevices: true));
		}
		if (DeviceTypeUtilities.IsRecordingDevice(deviceType))
		{
			list.Add(RecordingDevicePropertyDefinition.RecordingEnabledProperty(recordingEnabled: true));
			list.Add(RecordingDevicePropertyDefinition.PrebufferEnabledProperty(prebufferEnabled: true));
			list.Add(RecordingDevicePropertyDefinition.PrebufferInMemoryProperty(prebufferInMemory: true));
			list.Add(RecordingDevicePropertyDefinition.PrebufferSecondsProperty(1));
			list.Add(RecordingDevicePropertyDefinition.EdgeStorageEnabledProperty(edgeStorageEnabled: true));
			list.Add(RecordingDevicePropertyDefinition.EdgeStoragePlaybackEnabledProperty(edgeStoragePlaybackEnabled: true));
			list.Add(RecordingDevicePropertyDefinition.ManualRecordingTimeoutEnabledProperty(manualRecordingTimeoutEnabled: true));
			list.Add(RecordingDevicePropertyDefinition.ManualRecordingTimeoutMinutesProperty(1));
			ConfigurationItemPath path = new ConfigurationItemPathBuilder("Storage", Guid.Empty.ToString()).CreatePath();
			list.Add(RecordingDevicePropertyDefinition.RecordingStoragePathProperty(path));
		}
		return list;
	}

	private static Dictionary<string, string> GetQueryKeyTranslations()
	{
		return new Dictionary<string, string>
		{
			{ "Id", "d.IDDevice" },
			{ "Channel", "d.DeviceIndex" },
			{ "RecordingFramerate", "rs.recording_framerate" },
			{ "RecordKeyframesOnly", "rs.record_keyframes" },
			{ "RecordOnRelatedDevices", "rs.record_on_related_devices" },
			{ "RecordingEnabled", "rs.recording_enabled" },
			{ "PrebufferEnabled", "rs.prebuffer_enabled" },
			{ "PrebufferInMemory", "rs.prebuffer_in_memory" },
			{ "PrebufferSeconds", "rs.prebuffer_seconds" },
			{ "EdgeStorageEnabled", "rs.edgestorage_enabled" },
			{ "EdgeStoragePlaybackEnabled", "rs.edgestorage_playback" },
			{ "ManualRecordingTimeoutEnabled", "rs.manual_timeout_enabled" },
			{ "ManualRecordingTimeoutMinutes", "rs.manual_timeout" }
		};
	}

	private static void ConvertFailoverSetting(string failoverSetting, out bool? failoverLiveOnly, out bool? failoverDisabled)
	{
		failoverLiveOnly = null;
		failoverDisabled = null;
		switch (failoverSetting)
		{
		case "LiveOnly":
			failoverLiveOnly = true;
			failoverDisabled = false;
			break;
		case "Disabled":
			failoverLiveOnly = false;
			failoverDisabled = true;
			break;
		case "FullSupport":
			failoverLiveOnly = false;
			failoverDisabled = false;
			break;
		default:
			throw new PropertyInvalidException("Invalid value", "FailoverSetting");
		}
	}
}
