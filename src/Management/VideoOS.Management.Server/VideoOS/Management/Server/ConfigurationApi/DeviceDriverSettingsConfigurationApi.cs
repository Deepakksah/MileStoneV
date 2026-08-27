using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Definition;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPIDeviceDriverSettings")]
[EntityName("ConfigAPIDeviceDriverSettingsFolder")]
public class DeviceDriverSettingsConfigurationApi : ConfigurationApiBase, IConfigurationApiSupport
{
	public ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
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
		if (parentPath == null)
		{
			throw new ArgumentNullException("parentPath");
		}
		ThrowIfPathInvalid(path);
		ThrowIfPathInvalid(parentPath);
		if (!path.IsFolder)
		{
			if (!Device.DeviceExists(requestContext, path.Id))
			{
				throw new PathNotFoundException(path.Text);
			}
			ValidateResult validateResult = SaveItemInternal(requestContext, item, path);
			if (!validateResult.ValidatedOk)
			{
				return validateResult;
			}
			validateResult = new ValidateResult();
			validateResult.ValidatedOk = true;
			validateResult.ResultItem = GetItem(requestContext, path);
			return validateResult;
		}
		throw new NotSupportedException();
	}

	public ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
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
		if (parentPath == null)
		{
			throw new ArgumentNullException("parentPath");
		}
		ThrowIfPathInvalid(path);
		ThrowIfPathInvalid(parentPath);
		if (!path.IsFolder)
		{
			if (!Device.DeviceExists(requestContext, path.Id))
			{
				throw new PathNotFoundException(path.Text);
			}
			return ValidateItemInternal(requestContext, item, path);
		}
		throw new NotSupportedException();
	}

	private static ValidateResult ValidateItemInternal(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		try
		{
			if (!path.IsFolder)
			{
				ValidateResult validateResult = new ValidateResult();
				validateResult.ValidatedOk = true;
				validateResult.ResultItem = item;
				DriverSettings updatedDriverSettings = new DriverSettings();
				MainDefinition mainDefinition = null;
				DeviceHwid hwid = null;
				DriverSettings deviceSettings = null;
				int? ptzManualSessionTimeout = null;
				int? ptzReservedSessionTimeout = null;
				int? ptzExtendedManualSessionTimeout = null;
				DriverSettingsBusiness.QueryDeviceSettings(requestContext, path.Id, out mainDefinition, out hwid, out deviceSettings);
				List<ErrorResult> list = SettingsUtil.ConvertToDriverSettings(item, mainDefinition, out updatedDriverSettings);
				list.AddRange(ValidatePTZSessionTimeouts(item, out ptzManualSessionTimeout, out ptzReservedSessionTimeout, out ptzExtendedManualSessionTimeout));
				list.AddRange(ValidateUpdatedSettings(requestContext, path.Id, updatedDriverSettings));
				if (list.Count > 0)
				{
					validateResult.ErrorResults = list.ToArray();
					validateResult.ValidatedOk = false;
				}
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

	private static ValidateResult SaveItemInternal(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		try
		{
			if (!path.IsFolder)
			{
				ValidateResult validateResult = new ValidateResult();
				validateResult.ValidatedOk = true;
				validateResult.ResultItem = item;
				DriverSettings updatedDriverSettings = new DriverSettings();
				MainDefinition mainDefinition = null;
				DeviceHwid hwid = null;
				DriverSettings deviceSettings = null;
				DriverSettingsBusiness.QueryDeviceSettings(requestContext, path.Id, out mainDefinition, out hwid, out deviceSettings);
				List<ErrorResult> list = SettingsUtil.ConvertToDriverSettings(item, mainDefinition, out updatedDriverSettings);
				if (list.Count > 0)
				{
					validateResult.ErrorResults = list.ToArray();
					validateResult.ValidatedOk = false;
				}
				int? ptzManualSessionTimeout = null;
				int? ptzReservedSessionTimeout = null;
				int? ptzExtendedManualSessionTimeout = null;
				list = ValidatePTZSessionTimeouts(item, out ptzManualSessionTimeout, out ptzReservedSessionTimeout, out ptzExtendedManualSessionTimeout);
				Device.UpdateDevice(requestContext, path.Id, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, ptzManualSessionTimeout, ptzReservedSessionTimeout, ptzExtendedManualSessionTimeout, null, null);
				list.AddRange(SaveUpdatedSettings(requestContext, path.Id, updatedDriverSettings));
				if (list.Count > 0)
				{
					validateResult.ErrorResults = list.ToArray();
					validateResult.ValidatedOk = false;
				}
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

	private static List<ErrorResult> ValidateUpdatedSettings(IGeminiRequestContext requestContext, string id, DriverSettings updatedDriverSettings)
	{
		List<ErrorResult> list = new List<ErrorResult>();
		try
		{
			DriverSettingsBusiness.ValidateDeviceSettings(requestContext, id, updatedDriverSettings);
		}
		catch (UpdateDriverSettingsException e)
		{
			list.Add(SettingsUtil.ConvertException(e));
		}
		catch (InvalidDefinitionException)
		{
			list.Add(SettingsUtil.ConstructErrorResult(ConfigurationApiResources.ErrorPropertyInvalidDefinition, "ErrorPropertyInvalidDefinition"));
		}
		return list;
	}

	private static List<ErrorResult> ValidatePTZSessionTimeouts(ConfigurationItem item, out int? ptzManualSessionTimeout, out int? ptzReservedSessionTimeout, out int? ptzExtendedManualSessionTimeout)
	{
		ptzManualSessionTimeout = null;
		ptzReservedSessionTimeout = null;
		ptzExtendedManualSessionTimeout = null;
		List<ErrorResult> list = new List<ErrorResult>();
		if (item.Children != null)
		{
			ConfigurationItem configurationItem = item.Children.FirstOrDefault((ConfigurationItem i) => i.ItemType == "PTZSessionTimeout");
			if (configurationItem != null)
			{
				Property property = configurationItem.Properties.FirstOrDefault((Property p) => p.Key == "ManualPTZTimeout");
				if (property != null && int.TryParse(property.Value, out var result))
				{
					if (IsValidTimeout(result))
					{
						ptzManualSessionTimeout = result;
					}
					else
					{
						list.Add(ErrorHelper.ErrorPropertyOutOfRange(property));
					}
				}
				property = configurationItem.Properties.FirstOrDefault((Property p) => p.Key == "ReservedPTZTimeout");
				if (property != null && int.TryParse(property.Value, out result))
				{
					if (IsValidTimeout(result))
					{
						ptzReservedSessionTimeout = result;
					}
					else
					{
						list.Add(ErrorHelper.ErrorPropertyOutOfRange(property));
					}
				}
				property = configurationItem.Properties.FirstOrDefault((Property p) => p.Key == "PausePatrollingTimeout");
				if (property != null && int.TryParse(property.Value, out result))
				{
					if (IsValidTimeout(result))
					{
						ptzExtendedManualSessionTimeout = result;
					}
					else
					{
						list.Add(ErrorHelper.ErrorPropertyOutOfRange(property));
					}
				}
			}
		}
		return list;
	}

	private static bool IsValidTimeout(int timeout)
	{
		if (timeout < -1)
		{
			return false;
		}
		if (timeout == -1)
		{
			return true;
		}
		if (timeout == 0)
		{
			return false;
		}
		if (timeout < 1000)
		{
			return true;
		}
		if (timeout / 60 < 1000 && timeout % 60 == 0)
		{
			return true;
		}
		if (timeout / 3600 < 1000 && timeout % 3600 == 0)
		{
			return true;
		}
		return false;
	}

	private static List<ErrorResult> SaveUpdatedSettings(IGeminiRequestContext requestContext, string deviceId, DriverSettings updatedDriverSettings)
	{
		List<ErrorResult> list = new List<ErrorResult>();
		try
		{
			DriverSettingsBusiness.UpdateDeviceSettings(requestContext, deviceId, updatedDriverSettings);
		}
		catch (UpdateDriverSettingsException e)
		{
			list.Add(SettingsUtil.ConvertException(e));
		}
		catch (InvalidDefinitionException)
		{
			list.Add(SettingsUtil.ConstructErrorResult(ConfigurationApiResources.ErrorPropertyInvalidDefinition, "ErrorPropertyInvalidDefinition"));
		}
		return list;
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
		ThrowIfPathInvalid(path);
		if (path.IsFolder)
		{
			Device device = Device.QueryDeviceById(requestContext, path.Id);
			if (device == null)
			{
				throw new PathNotFoundException("Device not found. " + path.Text);
			}
			MainDefinition mainDefinition = null;
			DeviceHwid hwid = null;
			DriverSettings deviceSettings = null;
			DriverSettingsBusiness.QueryDeviceSettings(requestContext, path.Id, out mainDefinition, out hwid, out deviceSettings);
			ConfigurationItem configurationItem = SettingsUtil.ConstructDriverSettingsItemAndChildren(requestContext, mainDefinition, hwid, "DeviceDriverSettings", deviceSettings, path);
			if (device.SettingUtilities.IsPtzEnabled)
			{
				AddPtzSessionChild(configurationItem, device);
			}
			return new ConfigurationItem[1] { configurationItem };
		}
		return new ConfigurationItem[0];
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
			DeviceTypeRelation deviceTypeRelation = DeviceTypeRelation.Relations.FirstOrDefault((DeviceTypeRelation r) => string.Equals(r.ItemType, path.ParentItemType, StringComparison.Ordinal));
			if (deviceTypeRelation != null)
			{
				string id = path.Id;
				Device device = Device.QueryDeviceById(requestContext, id);
				if (device == null || device.DeviceType != deviceTypeRelation.DeviceType)
				{
					throw new PathNotFoundException(path.Text);
				}
				ConfigurationItemPath parentPath = ConfigurationItemPath.CreateItemPath(deviceTypeRelation.ItemType, id);
				return GroupConfigurationItemFactory.CreateFolderItem(path.ItemType, parentPath);
			}
			throw new NotImplementedException();
		}
		Device device2 = Device.QueryDeviceById(requestContext, path.Id);
		if (device2 == null)
		{
			throw new PathNotFoundException("Device not found. " + path.Text);
		}
		DeviceTypeRelation obj = DeviceTypeRelation.Relations.FirstOrDefault((DeviceTypeRelation r) => r.DeviceType == device2.DeviceType) ?? throw new InternalConfigurationApiException("Device retrieved from database had an unknown device type. " + device2.DeviceType);
		ConfigurationItemPath parentPath2 = ConfigurationItemPath.CreateFolderPath(parentId: path.Id, parentType: obj.ItemType, folderType: "DeviceDriverSettingsFolder");
		MainDefinition mainDefinition = null;
		DeviceHwid hwid = null;
		DriverSettings deviceSettings = null;
		DriverSettingsBusiness.QueryDeviceSettings(requestContext, path.Id, out mainDefinition, out hwid, out deviceSettings);
		ConfigurationItem configurationItem = SettingsUtil.ConstructDriverSettingsItemAndChildren(requestContext, mainDefinition, hwid, "DeviceDriverSettings", deviceSettings, parentPath2);
		if (device2.SettingUtilities.IsPtzEnabled)
		{
			AddPtzSessionChild(configurationItem, device2);
		}
		return configurationItem;
	}

	public ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, string methodId)
	{
		throw new NotSupportedException();
	}

	private static void AddPtzSessionChild(ConfigurationItem settingsFolder, Device device)
	{
		string path = settingsFolder.Path;
		List<Property> list = new List<Property>();
		list.Add(DeviceDriverSettingsProperties.GetManualPTZTimeoutProperty(device.PtzManualSessionTimeout, isSettable: true));
		list.Add(DeviceDriverSettingsProperties.GetPausePatrollingTimeoutProperty(device.PtzExtendedManualSessionTimeout, isSettable: true));
		list.Add(DeviceDriverSettingsProperties.GetReservedPTZTimeoutProperty(device.PtzReservedSessionTimeout, isSettable: true));
		ConfigurationItem item = new ConfigurationItem
		{
			Children = null,
			ChildrenFilled = true,
			DisplayName = ConfigurationApiResources.PtzSessionTimeoutDisplayName,
			ItemType = "PTZSessionTimeout",
			ItemCategory = ItemCategories.ChildItem,
			Path = path,
			ParentPath = path,
			Properties = list.ToArray()
		};
		List<ConfigurationItem> list2 = new List<ConfigurationItem>(settingsFolder.Children);
		list2.Add(item);
		settingsFolder.Children = list2.ToArray();
	}

	private static void ThrowIfPathInvalid(ConfigurationItemPath path)
	{
		if (path.IsFolder)
		{
			if (!DeviceTypeRelation.Relations.Any((DeviceTypeRelation r) => string.Equals(r.ItemType, path.ParentItemType, StringComparison.Ordinal)))
			{
				throw new PathInvalidException("Invalid parent. " + path.Text);
			}
			if (!path.Id.IsGuid())
			{
				throw new PathInvalidException("Id format invalid. " + path.Text);
			}
		}
		else
		{
			if (!string.Equals("DeviceDriverSettings", path.ItemType, StringComparison.Ordinal))
			{
				throw new PathInvalidException("Unsupported item type. " + path.Text);
			}
			if (!path.Id.IsGuid())
			{
				throw new PathInvalidException("Id format invalid. " + path.Text);
			}
		}
	}
}
