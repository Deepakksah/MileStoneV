using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Definition;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPIHardware")]
[EntityName("ConfigAPIHardwareFolder")]
public class HardwareConfigurationApi : ConfigurationApiBase, IConfigurationApiSupport
{
	public override bool SupportGetChildItemsWithItemTypes => true;

	public ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		ConfigurationApiBase.ThrowIfAnyParameterNull(requestContext, item, path, parentPath);
		ThrowIfPathInvalid(path);
		ThrowIfPathInvalid(parentPath);
		if (string.Equals(path.ItemType, "Hardware", StringComparison.Ordinal))
		{
			ValidateResult validateResult = ValidateItemInternal(requestContext, item, path, out var name, out var description, out var address, out var userName, out var enabled);
			if (!validateResult.ValidatedOk)
			{
				return validateResult;
			}
			try
			{
				Hardware hardware = Hardware.UpdateHardware(requestContext, path.Id, name, description, address, userName, enabled);
				ConfigurationItem resultItem = ConvertToConfigurationItem(requestContext, item.ParentPath, hardware);
				validateResult = new ValidateResult();
				validateResult.ValidatedOk = true;
				validateResult.ResultItem = resultItem;
				return validateResult;
			}
			catch (HardwareUpdateException)
			{
				throw new PathNotFoundException();
			}
		}
		throw new NotSupportedException();
	}

	public ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		ConfigurationApiBase.ThrowIfAnyParameterNull(requestContext, item, path, parentPath);
		ThrowIfPathInvalid(path);
		ThrowIfPathInvalid(parentPath);
		if (string.Equals(path.ItemType, "Hardware", StringComparison.Ordinal))
		{
			if (!Hardware.HardwareExists(requestContext, path.Id))
			{
				throw new PathNotFoundException(path.Text);
			}
			string name;
			string description;
			string address;
			string userName;
			bool? enabled;
			return ValidateItemInternal(requestContext, item, path, out name, out description, out address, out userName, out enabled);
		}
		throw new NotSupportedException();
	}

	private static ValidateResult ValidateItemInternal(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, out string name, out string description, out string address, out string userName, out bool? enabled)
	{
		name = null;
		description = null;
		address = null;
		userName = null;
		enabled = null;
		try
		{
			if (string.Equals(path.ItemType, "Hardware", StringComparison.Ordinal))
			{
				if (item.Properties != null)
				{
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Name", out name);
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Description", out description);
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Address", out address);
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("UserName", out userName);
				}
				if (item.EnableProperty != null)
				{
					enabled = item.EnableProperty.Enabled;
				}
				Hardware.UpdateHardwareValidation(requestContext, path.Id, name, description, address, userName, enabled);
				ValidateResult validateResult = new ValidateResult();
				validateResult.ValidatedOk = true;
				validateResult.ResultItem = item;
				return validateResult;
			}
			throw new NotSupportedException();
		}
		catch (ArgumentNullException exception)
		{
			ValidateResult validateResult = ErrorHelper.CreateValidateResult(exception, item.Properties);
			if (validateResult != null)
			{
				return validateResult;
			}
			throw;
		}
		catch (ArgumentException exception2)
		{
			ValidateResult validateResult = ErrorHelper.CreateValidateResult(exception2, item.Properties);
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
		string itemType = path.ItemType;
		if (string.Equals(itemType, "HardwareFolder", StringComparison.OrdinalIgnoreCase))
		{
			string id = path.Id;
			if (!RecordingServer.RecordingServerExists(requestContext, id))
			{
				throw new PathNotFoundException();
			}
			ItemFilter itemFilter = itemFilters.FirstOrDefault((ItemFilter f) => f.ItemType == "Hardware");
			string[] ids = Hardware.QueryHardwareIdsByRecorder(requestContext, id);
			Hardware[] array = Hardware.QueryHardwareByIds(requestContext, ids);
			List<ConfigurationItem> list = new List<ConfigurationItem>();
			Hardware[] array2 = array;
			foreach (Hardware hardware in array2)
			{
				ConfigurationItem item = ConvertToConfigurationItem(requestContext, path.Text, hardware);
				if (itemFilter == null || ConfigurationApiBase.Match(item, itemFilter))
				{
					list.Add(item);
				}
			}
			ConfigurationItem[] array3 = list.ToArray();
			if (childItemTypes != null && childItemTypes.Any())
			{
				FillChildren(requestContext, array3, childItemTypes, childItemFilters);
			}
			return array3;
		}
		if (string.Equals(itemType, "Hardware", StringComparison.OrdinalIgnoreCase))
		{
			string id2 = path.Id;
			if (itemFilters == null && Hardware.QueryHardwareById(requestContext, id2) == null)
			{
				throw new PathNotFoundException();
			}
			List<ConfigurationItem> list2 = new List<ConfigurationItem>();
			if (itemTypes == null || itemTypes.Contains("Camera"))
			{
				list2.Add(GroupConfigurationItemFactory.CreateFolderItem("CameraFolder", path));
			}
			if (itemTypes == null || itemTypes.Contains("Microphone"))
			{
				list2.Add(GroupConfigurationItemFactory.CreateFolderItem("MicrophoneFolder", path));
			}
			if (itemTypes == null || itemTypes.Contains("Output"))
			{
				list2.Add(GroupConfigurationItemFactory.CreateFolderItem("OutputFolder", path));
			}
			if (itemTypes == null || itemTypes.Contains("Speaker"))
			{
				list2.Add(GroupConfigurationItemFactory.CreateFolderItem("SpeakerFolder", path));
			}
			if (itemTypes == null || itemTypes.Contains("InputEvent"))
			{
				list2.Add(GroupConfigurationItemFactory.CreateFolderItem("InputEventFolder", path));
			}
			if (itemTypes == null || itemTypes.Contains("Metadata"))
			{
				list2.Add(GroupConfigurationItemFactory.CreateFolderItem("MetadataFolder", path));
			}
			if (itemTypes == null || itemTypes.Contains("HardwareDriverSettings"))
			{
				list2.Add(ConvertToHardwareSettingsFolderConfigurationItem(path));
			}
			if (itemTypes == null || itemTypes.Contains("HardwarePtzSettings"))
			{
				DriverSettingsBusiness.QueryHardwareSettings(requestContext, id2, out var mainDefinition, out var _);
				if (mainDefinition.HardwareDefinition.IsPtzEnabledVideoServer())
				{
					list2.Add(GroupConfigurationItemFactory.CreateFolderItem("HardwarePtzSettingsFolder", path));
				}
			}
			if (itemTypes == null || itemTypes.Contains("CustomProperties"))
			{
				list2.Add(GroupConfigurationItemFactory.CreateFolderItem("CustomPropertiesFolder", path));
			}
			if (itemTypes == null || itemTypes.Contains("HardwareDeviceEvent"))
			{
				list2.Add(GroupConfigurationItemFactory.CreateFolderItem("HardwareDeviceEventFolder", path));
			}
			ConfigurationItem[] array4 = list2.ToArray();
			if (childItemTypes != null && childItemTypes.Any())
			{
				FillChildren(requestContext, array4, childItemTypes, childItemFilters);
			}
			return array4;
		}
		throw new NotSupportedException();
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
		if (string.Equals(path.ItemType, "Hardware", StringComparison.Ordinal))
		{
			Hardware hardware = Hardware.QueryHardwareById(requestContext, path.Id);
			if (hardware == null)
			{
				throw new PathNotFoundException();
			}
			ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreateFolderPath("RecordingServer", hardware.RecordingServerId, "HardwareFolder");
			return ConvertToConfigurationItem(requestContext, configurationItemPath.Text, hardware);
		}
		if (string.Equals(path.ItemType, "HardwareFolder", StringComparison.Ordinal))
		{
			if (RecordingServer.QueryRecordingServer(requestContext, path.Id) == null)
			{
				throw new PathNotFoundException(path.Text);
			}
			ConfigurationItemPath parentPath = new ConfigurationItemPathBuilder
			{
				ItemType = "RecordingServer",
				Id = path.Id
			}.CreatePath();
			return GroupConfigurationItemFactory.CreateFolderItem("HardwareFolder", parentPath);
		}
		throw new NotSupportedException();
	}

	public override ConfigurationItem[] QueryItems(IGeminiRequestContext requestContext, ConfigurationItemPath folderPath, ItemFilter itemFilter, int maxResults, int startIndex, List<ItemFilter> childItemFilter)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (folderPath == null)
		{
			throw new ArgumentNullException("folderPath");
		}
		if (itemFilter == null)
		{
			throw new ArgumentNullException("itemFilter");
		}
		string[] recordingServerIds = RecordingServer.QueryRecordingServerIds(requestContext);
		RecordingServer[] array = RecordingServer.QueryRecordingServersByIds(requestContext, recordingServerIds);
		List<ConfigurationItem> list = new List<ConfigurationItem>();
		int num = 0;
		RecordingServer[] array2 = array;
		foreach (RecordingServer recordingServer in array2)
		{
			ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreateFolderPath("RecordingServer", recordingServer.Id, "HardwareFolder");
			Hardware[] array3 = Hardware.QueryHardwareByRecordingServer(requestContext, recordingServer.Id);
			foreach (Hardware hardware in array3)
			{
				ConfigurationItem item = ConvertToConfigurationItem(requestContext, configurationItemPath.Text, hardware);
				if (ConfigurationApiBase.Match(item, itemFilter))
				{
					if (num >= startIndex)
					{
						list.Add(item);
					}
					num++;
					if (num >= startIndex + maxResults)
					{
						break;
					}
				}
			}
			if (num >= startIndex + maxResults)
			{
				break;
			}
		}
		List<string> list2 = new List<string>();
		if (childItemFilter != null)
		{
			foreach (ItemFilter item2 in childItemFilter)
			{
				list2.Add(item2.ItemType);
			}
		}
		ConfigurationItem[] array4 = list.ToArray();
		if (list2.Any())
		{
			FillChildren(requestContext, array4, list2.ToArray(), childItemFilter.ToArray());
		}
		return array4;
	}

	public override int QueryItemCount(IGeminiRequestContext requestContext, ConfigurationItemPath folderPath, ItemFilter itemFilter)
	{
		return QueryItems(requestContext, folderPath, itemFilter, 999999, 0, null).Length;
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
		case "ReadPasswordHardware":
			if (!flag)
			{
				return CreateInvokeInfoReadPassword(item, methodId);
			}
			return ReadPassword(requestContext, item, path);
		case "ChangePasswordHardware":
			if (!flag)
			{
				return CreateInvokeInfoChangePassword(item, methodId);
			}
			return ChangePassword(requestContext, item, path);
		case "UpdateFirmwareHardware":
			if (!flag)
			{
				return CreateInvokeInfoUpdateFirmware(item, methodId);
			}
			return UpdateFirmware(requestContext, item, path);
		case "DeleteHardware":
			if (!flag)
			{
				return CreateInvokeInfoItemDeleteHardware(item, GetChildItems(requestContext, path), methodId);
			}
			return DeleteHardware(requestContext, item, path);
		case "MoveHardware":
			if (!flag)
			{
				return CreateInvokeInfoMoveHardwareRecorderSelection(requestContext, item, methodId);
			}
			return MoveHardware(requestContext, item, path);
		case "UpdateHardware":
			if (!flag)
			{
				return CreateInvokeInfoUpdateHardwareParameters(item, methodId);
			}
			return UpdateHardware(requestContext, item, path);
		case "ReplaceHardware":
			if (!flag)
			{
				return CreateInvokeInfoReplaceHardwareParameters(requestContext, item, path, parentPath, methodId);
			}
			return ReplaceHardware(requestContext, item, path);
		default:
			throw new ArgumentException("Unknown methodId:" + methodId, "methodId");
		}
	}

	private static ConfigurationItem CreateInvokeInfoItemDeleteHardware(ConfigurationItem item, ConfigurationItem[] hardware, string methodId)
	{
		if (item.ItemType != "HardwareFolder")
		{
			throw new PathInvalidException("Invalid path. Path: " + item.ItemType);
		}
		return MethodTypesHelper.CreateInvokeMemberSelectionInfo(methodId, ConfigurationApiResources.WizardDeleteHardware, "PropertyDeleteHardware", ConfigurationApiResources.PropertyDeleteHardware, item, hardware);
	}

	private static ConfigurationItem DeleteHardware(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		if (!path.IsFolder)
		{
			throw new PathInvalidException("Invalid path. Path: " + path.Text);
		}
		if (!string.Equals(path.ParentItemType, "RecordingServer", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		ConfigurationItemPath invokeMemberItemSelectionPath = MethodTypesHelper.GetInvokeMemberItemSelectionPath(item);
		try
		{
			return TaskHelperFunctions.CreateTaskInvokeResultConfigurationItem(Task.DeleteHardware(requestContext, invokeMemberItemSelectionPath.Id), item);
		}
		catch (HardwareNotFoundException)
		{
			throw new PathNotFoundException(invokeMemberItemSelectionPath.Text);
		}
	}

	private static ConfigurationItem CreateInvokeInfoReadPassword(ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardReadPasswordHardware;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { methodId };
		configurationItem.Properties = new Property[0];
		return configurationItem;
	}

	private static ConfigurationItem ReadPassword(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ValidatePath(path);
		VerifyHardwareExists(requestContext, path);
		try
		{
			string password = Hardware.ReadHardwarePassword(requestContext, path.Id);
			item.Properties = new Property[1] { HardwarePropertyDefinition.PasswordProperty(password) };
			return MethodTypesHelper.CreateInvokeResult(item, null, "Result", "Success", true);
		}
		catch (HardwareNotFoundException)
		{
			throw new PathNotFoundException(path.Text);
		}
	}

	private static ConfigurationItem CreateInvokeInfoChangePassword(ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardChangePasswordHardware;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { methodId };
		configurationItem.Properties = new Property[2]
		{
			HardwarePropertyDefinition.PasswordProperty(""),
			HardwarePropertyDefinition.UpdatePasswordOnDeviceProperty()
		};
		return configurationItem;
	}

	private static ConfigurationItem ChangePassword(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ValidatePath(path);
		VerifyHardwareExists(requestContext, path);
		if (item.Properties == null || !((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Password", out string value))
		{
			throw new PropertyInvalidException("No new password selected.", "Password");
		}
		if (((IEnumerable<Property>)item.Properties).TryGetPropertyValue("UpdatePasswordOnDevice", out bool? value2) && (value2 ?? false))
		{
			try
			{
				return TaskHelperFunctions.CreateTaskInvokeResultConfigurationItem(Task.ChangeHardwarePassword(requestContext, path.Id, string.Empty, value), item);
			}
			catch (HardwareNotFoundException)
			{
				throw new PathNotFoundException(path.Text);
			}
			catch (UnauthorizedAccessException)
			{
				throw new UnauthorizedAccessException(path.Text);
			}
			catch (FeatureNotLicensedException)
			{
				throw new LicenseException("Change hardware password");
			}
		}
		try
		{
			Hardware.ChangeHardwarePassword(requestContext, path.Id, value);
			return MethodTypesHelper.CreateInvokeResult(item, null, "Result", "Success", true);
		}
		catch (HardwareUpdateException)
		{
			throw new PathNotFoundException(path.Text);
		}
	}

	private static ConfigurationItem CreateInvokeInfoUpdateFirmware(ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.MethodUpdateFirmwareHardware;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { methodId };
		configurationItem.Properties = new Property[1] { FileUploadPropertyDefinition.FileStorageIdProperty("") };
		return configurationItem;
	}

	private static ConfigurationItem UpdateFirmware(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ValidatePath(path);
		VerifyHardwareExists(requestContext, path);
		if (!GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "FirmwareUpdate"))
		{
			throw new LicenseException("Firmware update");
		}
		if (item.Properties == null || !((IEnumerable<Property>)item.Properties).TryGetPropertyValue("StorageId", out string value))
		{
			throw new PropertyInvalidException("No file id provided.", "StorageId");
		}
		try
		{
			item = TaskHelperFunctions.CreateTaskInvokeResultConfigurationItem(Task.FirmwareUpgrade(requestContext, path.Id, value), item);
			return item;
		}
		catch (HardwareNotFoundException)
		{
			throw new PathNotFoundException(path.Text);
		}
		catch (UnauthorizedAccessException)
		{
			throw new PathNotFoundException(path.Text);
		}
		catch (FeatureNotLicensedException)
		{
			throw new LicenseException("Firmware update");
		}
	}

	private static ConfigurationItem CreateInvokeInfoMoveHardwareRecorderSelection(IGeminiRequestContext requestContext, ConfigurationItem item, string methodId)
	{
		RecordingServerConfigurationApi recordingServerConfigurationApi = new RecordingServerConfigurationApi();
		ConfigurationItemPath parentPath = ConfigurationItemPath.CreateSystemPath();
		ConfigurationItem[] childItems = recordingServerConfigurationApi.GetChildItems(requestContext, ConfigurationItemPath.CreateFolderPath(parentPath, "RecordingServerFolder"));
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardMoveHardware;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { methodId };
		configurationItem.Properties = new Property[1] { PropertyTypesHelper.GetMembersPathAsEnumsProperty("DestinationRecordingServer", ConfigurationApiResources.PropertyHardwareDestinationRecorder, "PropertyHardwareDestinationRecorder", "PropertyHardwareDestinationRecorderToolTip", childItems) };
		return configurationItem;
	}

	private static ConfigurationItem CreateInvokeInfoMoveHardwareStorageSelection(IGeminiRequestContext requestContext, ConfigurationItemPath recorderPath, ConfigurationItem item)
	{
		ConfigurationItem[] childItems = new RecordingStorageConfigurationApi().GetChildItems(requestContext, ConfigurationItemPath.CreateFolderPath(recorderPath, "StorageFolder"));
		List<Property> list = new List<Property>();
		list.AddRange(item.Properties.Select(delegate(Property p)
		{
			if (p.Key.Equals("DestinationRecordingServer"))
			{
				p.IsSettable = false;
			}
			return p;
		}));
		list.Add(PropertyTypesHelper.GetMembersPathAsEnumsProperty("DestinationStorage", ConfigurationApiResources.PropertyHardwareDestinationStorage, "PropertyHardwareDestinationStorage", "PropertyHardwareDestinationStorageToolTip", childItems));
		list.Add(HardwarePropertyDefinition.IgnoreSourceRecordingServerProperty(ignoreSourceRecordingServer: false));
		item.Properties = list.ToArray();
		return item;
	}

	private static ConfigurationItem MoveHardware(IGeminiRequestContext requestContext, ConfigurationItem invokeInfoItem, ConfigurationItemPath path)
	{
		ValidatePath(path);
		if (ConfigurationItemPath.CreatePath(invokeInfoItem.ParentPath).ParentItemType != "RecordingServer")
		{
			throw new PathInvalidException("Invalid id. Path: " + path.Text);
		}
		if (invokeInfoItem.Properties == null)
		{
			throw new PropertyInvalidException("No properties found.");
		}
		if (!((IEnumerable<Property>)invokeInfoItem.Properties).TryGetPropertyValue("DestinationRecordingServer", out string value))
		{
			throw new PropertyInvalidException("No destination recorder.");
		}
		ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreatePath(value);
		ThrowIfPathInvalid(configurationItemPath);
		if (ConfigurationItemPath.CreatePath(invokeInfoItem.ParentPath).Id == configurationItemPath.Id)
		{
			throw new PropertyInvalidException("Invalid id. Id: " + configurationItemPath.Id);
		}
		if (!((IEnumerable<Property>)invokeInfoItem.Properties).TryGetPropertyValue("DestinationStorage", out string value2))
		{
			return CreateInvokeInfoMoveHardwareStorageSelection(requestContext, configurationItemPath, invokeInfoItem);
		}
		ConfigurationItemPath configurationItemPath2 = ConfigurationItemPath.CreatePath(value2);
		ThrowIfPathInvalid(configurationItemPath2);
		RecordingStorage recordingStorage = RecordingStorage.QueryRecordingStorage(requestContext, configurationItemPath2.Id);
		if (recordingStorage == null || !string.Equals(recordingStorage.RecordingServerId, configurationItemPath.Id, StringComparison.OrdinalIgnoreCase))
		{
			throw new PropertyInvalidException("Storage does not reside on destination recorder.");
		}
		if (!((IEnumerable<Property>)invokeInfoItem.Properties).TryGetPropertyValue("ignoreSourceRecordingServer", out bool? value3))
		{
			throw new PropertyInvalidException("No ignore source recorder property.");
		}
		try
		{
			return TaskHelperFunctions.CreateTaskInvokeResultConfigurationItem(Task.MoveHardware(requestContext, path.Id, configurationItemPath.Id, configurationItemPath2.Id, value3.Value), invokeInfoItem);
		}
		catch (HardwareNotFoundException)
		{
			throw new PathNotFoundException(path.Text);
		}
		catch (RecordingServerNotFoundException)
		{
			throw new PathNotFoundException(configurationItemPath.Text);
		}
		catch (RecordingStorageNotFoundException)
		{
			throw new PathNotFoundException(configurationItemPath2.Text);
		}
		catch (UnauthorizedAccessException)
		{
			throw new PathNotFoundException(path.Text);
		}
		catch (FeatureNotLicensedException)
		{
			throw new LicenseException("Move hardware");
		}
	}

	private static ConfigurationItem CreateInvokeInfoUpdateHardwareParameters(ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.MethodUpdateHardwareDisplayName;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { methodId };
		configurationItem.Properties = new Property[1] { HardwareTaskResultDefinition.SessionDataIdProperty(0L, isSettable: true) };
		return configurationItem;
	}

	private static ConfigurationItem UpdateHardware(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ValidatePath(path);
		Hardware hardware = VerifyHardwareExists(requestContext, path);
		if (hardware.DriverNumber < 30000)
		{
			throw new ArgumentException(VmoResources.VMO67001);
		}
		try
		{
			long result = 0L;
			if (((IEnumerable<Property>)item.Properties).TryGetPropertyValue("SessionDataId", out string value) && !long.TryParse(value, out result))
			{
				result = 0L;
			}
			return TaskHelperFunctions.CreateTaskInvokeResultConfigurationItem(Task.UpdateHardware(requestContext, hardware.Id, result), item);
		}
		catch (HardwareNotFoundException)
		{
			throw new PathNotFoundException(path.Text);
		}
		catch (UnauthorizedAccessException)
		{
			throw new PathNotFoundException(path.Text);
		}
		catch (FeatureNotLicensedException)
		{
			throw new LicenseException("Update hardware");
		}
	}

	private static ConfigurationItem CreateInvokeInfoReplaceHardwareParameters(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, string methodId)
	{
		(string, int, string) tuple = ResolveHardwareSettings();
		(bool, int) tuple2 = ResolveHttpsSettings();
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.MethodReplaceHardwareDisplayName;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { methodId };
		configurationItem.Properties = new Property[9]
		{
			HardwarePropertyDefinition.ReplaceHardwareAddressProperty(tuple.Item1),
			HardwarePropertyDefinition.ReplaceHardwarePortProperty(tuple.Item2),
			HardwarePropertyDefinition.ReplaceHardwareUseHttpsProperty(tuple2.Item1),
			HardwarePropertyDefinition.ReplaceHardwareHttpsPortProperty(tuple2.Item2),
			HardwarePropertyDefinition.ReplaceHardwareUserNameProperty(tuple.Item3),
			HardwarePropertyDefinition.ReplaceHardwarePasswordProperty(),
			CreateDriverNumberProperty(),
			HardwarePropertyDefinition.ReplaceHardwareCustomDriverDataProperty(),
			HardwarePropertyDefinition.ReplaceHardwareAllowDeletingDisabledDevicesProperty(value: false)
		};
		return configurationItem;
		Property CreateDriverNumberProperty()
		{
			ConfigurationItem[] array = new HardwareDriverConfigurationApi().GetChildItems(path: ConfigurationItemPath.CreateFolderPath(ConfigurationItemPath.CreateItemPath("RecordingServer", parentPath.Id), "HardwareDriverFolder"), requestContext: requestContext);
			Property membersPropertyAsEnumsProperty = PropertyTypesHelper.GetMembersPropertyAsEnumsProperty("Driver", ConfigurationApiResources.PropertyReplaceHardwareDriver, "PropertyReplaceHardwareDriver", "PropertyReplaceHardwareDriverToolTip", array, "Number");
			if (item?.Properties != null && ((IEnumerable<Property>)item.Properties).TryGetPropertyValue("HardwareDriverPath", out string hardwareDriverPath))
			{
				ConfigurationItem configurationItem2 = array.FirstOrDefault((ConfigurationItem d) => d.Path.Equals(hardwareDriverPath, StringComparison.OrdinalIgnoreCase));
				if (configurationItem2?.Properties != null && ((IEnumerable<Property>)configurationItem2.Properties).TryGetPropertyValue("Number", out string value))
				{
					membersPropertyAsEnumsProperty.Value = value.ToString(CultureInfo.InvariantCulture);
				}
			}
			return membersPropertyAsEnumsProperty;
		}
		(string Address, int Port, string UserName) ResolveHardwareSettings()
		{
			(string, int, string) result = ("0.0.0.0", 0, "");
			if (item.Properties != null)
			{
				if (((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Address", out string value) && Uri.TryCreate(value, UriKind.Absolute, out var result2))
				{
					result.Item1 = result2.Host;
					result.Item2 = result2.Port;
				}
				if (((IEnumerable<Property>)item.Properties).TryGetPropertyValue("UserName", out string value2))
				{
					result.Item3 = value2;
				}
			}
			return result;
		}
		(bool Enabled, int Port) ResolveHttpsSettings()
		{
			ConfigurationItem configurationItem2 = new HardwareSettingsConfigurationApi().GetChildItems(path: ConfigurationItemPath.CreateFolderPath("Hardware", path.Id, "HardwareDriverSettingsFolder"), requestContext: requestContext)?.FirstOrDefault()?.Children?.FirstOrDefault((ConfigurationItem s) => s.ItemType.Equals("HardwareDriverSettings", StringComparison.Ordinal));
			(bool, int) result = (false, 443);
			if (configurationItem2?.Properties != null)
			{
				Property property = configurationItem2.Properties.FirstOrDefault((Property p) => new Guid(p.Key.Split('/')[2]) == TypeReferenceIds.HttpsEnabled);
				if (property != null && property.Value != null)
				{
					result.Item1 = string.Equals(property.Value, "Yes", StringComparison.OrdinalIgnoreCase);
				}
				Property property2 = configurationItem2.Properties.FirstOrDefault((Property p) => new Guid(p.Key.Split('/')[2]) == TypeReferenceIds.HttpsPort);
				if (property2 != null && property2.Value != null)
				{
					result.Item2 = (int)Convert.ChangeType(property2.Value, typeof(int), CultureInfo.InvariantCulture);
				}
			}
			return result;
		}
	}

	private static ConfigurationItem ReplaceHardware(IGeminiRequestContext requestContext, ConfigurationItem invokeInfoItem, ConfigurationItemPath path)
	{
		ValidatePath(path);
		if (path.ItemType != "Hardware")
		{
			throw new PathInvalidException("Invalid item type. Path: " + path.Text);
		}
		if (invokeInfoItem.Properties == null)
		{
			throw new PropertyInvalidException("No properties found.", "Properties");
		}
		string address = ReadStringPropertyValue("Address");
		int port = ReadPropertyValue<int>("Port");
		bool useHttps = ReadPropertyValue<bool>("UseHttps");
		int httpsPort = ReadPropertyValue<int>("HttpsPort");
		string userName = ReadStringPropertyValue("UserName");
		string password = ReadStringPropertyValue("Password");
		string s = ReadStringPropertyValue("Driver");
		string customDriverData = ReadStringPropertyValue("CustomDriverData");
		bool allowDeletingDisabledDevices = ReadPropertyValue<bool>("AllowDeletingDisabledDevices");
		if (!int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result))
		{
			result = 0;
		}
		try
		{
			return TaskHelperFunctions.CreateTaskInvokeResultConfigurationItem(Task.ReplaceHardware(requestContext, path.Id, address, port, useHttps, httpsPort, userName, password, result, customDriverData, allowDeletingDisabledDevices), invokeInfoItem);
		}
		catch (HardwareNotFoundException)
		{
			throw new PathNotFoundException(path.Text);
		}
		catch (UnauthorizedAccessException)
		{
			throw new PathNotFoundException(path.Text);
		}
		catch (FeatureNotLicensedException)
		{
			throw new LicenseException("Replace hardware");
		}
		T ReadPropertyValue<T>(string propertyName) where T : struct
		{
			if (!((IEnumerable<Property>)invokeInfoItem.Properties).TryGetPropertyValue(propertyName, out T? value) || !value.HasValue)
			{
				throw new PropertyInvalidException("The property is missing or invalid", propertyName);
			}
			return value.Value;
		}
		string ReadStringPropertyValue(string propertyName)
		{
			if (!((IEnumerable<Property>)invokeInfoItem.Properties).TryGetPropertyValue(propertyName, out string value))
			{
				throw new PropertyInvalidException("The property is missing or invalid", propertyName);
			}
			return value;
		}
	}

	private static Hardware VerifyHardwareExists(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		return Hardware.QueryHardwareById(requestContext, path.Id) ?? throw new PathNotFoundException(path.Text);
	}

	private static void ValidatePath(ConfigurationItemPath path)
	{
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		ThrowIfPathInvalid(path);
		if (!string.Equals(path.ItemType, "Hardware", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid path. Path: " + path.Text);
		}
		if (path.IsFolder)
		{
			throw new PathInvalidException("Invalid path. Path: " + path.Text);
		}
	}

	private static void ThrowIfPathInvalid(ConfigurationItemPath path)
	{
		if (path.IsFolder)
		{
			if (!string.Equals(path.ParentItemType, "RecordingServer", StringComparison.Ordinal))
			{
				throw new PathInvalidException("Invalid parent. Path: " + path.Text);
			}
			if (!path.Id.IsGuid())
			{
				throw new PathInvalidException("Invalid id. Path: " + path.Text);
			}
		}
		else if (!path.Id.IsGuid())
		{
			throw new PathInvalidException("Invalid id. Path: " + path.Text);
		}
	}

	private static ConfigurationItem ConvertToConfigurationItem(IGeminiRequestContext requestContext, string parentPath, Hardware hardware)
	{
		ConfigurationItem obj = new ConfigurationItem
		{
			ItemType = "Hardware",
			UIHint = "DirectionHorizontal",
			ItemCategory = ItemCategories.Item,
			DisplayName = hardware.Name,
			ParentPath = parentPath
		};
		ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreateItemPath("Hardware", hardware.Id);
		obj.Path = configurationItemPath.Text;
		obj.EnableProperty = GeneralPropertyDefinitions.GetEnabledProperty(hardware.Enabled, uiFollowsEnabled: true);
		List<Property> list = new List<Property>
		{
			GeneralPropertyDefinitions.GetIdPropertyGuid(hardware.Id),
			GeneralPropertyDefinitions.GetNameProperty(hardware.Name),
			GeneralPropertyDefinitions.GetDescriptionProperty(hardware.Description)
		};
		if (!hardware.HostedHardwareId.HasValue)
		{
			list.Add(HardwarePropertyDefinition.AddressProperty(hardware.Address));
		}
		list.Add(HardwarePropertyDefinition.UserNameProperty(hardware.LoginId));
		string productId = FetchModelString(hardware);
		list.Add(HardwarePropertyDefinition.ModelProperty(productId));
		list.Add(HardwarePropertyDefinition.PasswordLastModifiedProperty(hardware.CredentialsLastModified));
		list.Add(GeneralPropertyDefinitions.GetLastModifiedProperty(hardware.LastModified));
		ConfigurationItemPath path = new ConfigurationItemPathBuilder("HardwareDriver", hardware.HardwareDriverId).CreatePath();
		list.Add(HardwarePropertyDefinition.HardwareDriverProperty(path));
		obj.Properties = list.ToArray();
		List<string> list2 = new List<string> { "ReadPasswordHardware", "ChangePasswordHardware", "MoveHardware", "ReplaceHardware" };
		if (GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "FirmwareUpdate"))
		{
			list2.Add("UpdateFirmwareHardware");
		}
		if (DriverNumbering.IsIcpDriver(hardware.DriverNumber) || DriverNumbering.IsMipDriver(hardware.DriverNumber))
		{
			list2.Add("UpdateHardware");
		}
		obj.MethodIds = list2.ToArray();
		obj.Children = null;
		obj.ChildrenFilled = false;
		return obj;
	}

	private static ConfigurationItem ConvertToHardwareSettingsFolderConfigurationItem(ConfigurationItemPath parentPath)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "HardwareDriverSettingsFolder";
		configurationItem.UIHint = "DirectionHorizontal";
		configurationItem.ItemCategory = ItemCategories.Group;
		configurationItem.DisplayName = ConfigurationApiResources.ItemTypeHardwareDriverSettingsFolder;
		configurationItem.ParentPath = parentPath.Text;
		ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreateFolderPath(parentPath.ItemType, parentPath.Id, configurationItem.ItemType);
		configurationItem.Path = configurationItemPath.Text;
		configurationItem.EnableProperty = null;
		configurationItem.Properties = new Property[0];
		configurationItem.ChildrenFilled = false;
		return configurationItem;
	}

	private static string FetchModelString(Hardware hardware)
	{
		if (hardware.DetectedModel != null)
		{
			return hardware.DetectedModel;
		}
		if (hardware.ProductId != null)
		{
			return hardware.ProductId;
		}
		if (hardware.HardwareDriverId != null)
		{
			return hardware.HardwareDriverId;
		}
		return "";
	}
}
