using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Definition;
using VideoOS.Common.Integration.Definition.Schema;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Validation;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPIHardwarePtzSettingsFolder")]
[EntityName("ConfigAPIHardwarePtzSettings")]
public class HardwarePtzSettingsConfigurationApi : ConfigurationApiBase, IConfigurationApiSupport
{
	public ConfigurationItem[] GetChildItems(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		requestContext.NullCheck("", "GetChildItems", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\Hardware\\HardwarePtzSettingsConfigurationApi.cs", 42);
		path.NullCheck("", "GetChildItems", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\Hardware\\HardwarePtzSettingsConfigurationApi.cs", 43);
		ThrowIfPathInvalid(path);
		string itemType = path.ItemType;
		if (string.Equals(itemType, "HardwarePtzSettingsFolder", StringComparison.OrdinalIgnoreCase))
		{
			string id = path.Id;
			Hardware hardware = Hardware.QueryHardwareById(requestContext, id);
			if (hardware == null)
			{
				throw new PathNotFoundException(path.Text);
			}
			ConfigurationItem configurationItem = ConvertToHardwarePtzSettingsConfigurationItem(requestContext, path, hardware);
			if (configurationItem != null)
			{
				return new ConfigurationItem[1] { configurationItem };
			}
			return new ConfigurationItem[0];
		}
		if (string.Equals(itemType, "HardwarePtzSettings", StringComparison.OrdinalIgnoreCase))
		{
			return new ConfigurationItem[0];
		}
		throw new NotSupportedException();
	}

	public ConfigurationItem GetItem(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		requestContext.NullCheck("", "GetItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\Hardware\\HardwarePtzSettingsConfigurationApi.cs", 73);
		path.NullCheck("", "GetItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\Hardware\\HardwarePtzSettingsConfigurationApi.cs", 74);
		ThrowIfPathInvalid(path);
		if (string.Equals(path.ItemType, "HardwarePtzSettingsFolder", StringComparison.OrdinalIgnoreCase))
		{
			ConfigurationItemPath parentPath = ConfigurationItemPath.CreateItemPath(path.ParentItemType, path.Id);
			return GroupConfigurationItemFactory.CreateFolderItem("HardwarePtzSettingsFolder", parentPath);
		}
		string id = path.Id;
		Hardware hardware = Hardware.QueryHardwareById(requestContext, id);
		if (hardware == null)
		{
			throw new PathNotFoundException(path.Text);
		}
		ConfigurationItemPath parentPath2 = ConfigurationItemPath.CreateFolderPath("Hardware", id, "HardwarePtzSettingsFolder");
		return ConvertToHardwarePtzSettingsConfigurationItem(requestContext, parentPath2, hardware);
	}

	public ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, string method)
	{
		throw new NotImplementedException();
	}

	public ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		requestContext.NullCheck("", "SetItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\Hardware\\HardwarePtzSettingsConfigurationApi.cs", 108);
		item.NullCheck("", "SetItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\Hardware\\HardwarePtzSettingsConfigurationApi.cs", 109);
		path.NullCheck("", "SetItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\Hardware\\HardwarePtzSettingsConfigurationApi.cs", 110);
		parentPath.NullCheck("", "SetItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\Hardware\\HardwarePtzSettingsConfigurationApi.cs", 111);
		ThrowIfPathInvalid(path);
		if (!path.IsFolder)
		{
			if (!Hardware.HardwareExists(requestContext, path.Id))
			{
				throw new PathNotFoundException(path.Text);
			}
			return SaveItemInternal(requestContext, item, path);
		}
		throw new NotSupportedException();
	}

	public ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		requestContext.NullCheck("", "ValidateItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\Hardware\\HardwarePtzSettingsConfigurationApi.cs", 131);
		item.NullCheck("", "ValidateItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\Hardware\\HardwarePtzSettingsConfigurationApi.cs", 132);
		path.NullCheck("", "ValidateItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\Hardware\\HardwarePtzSettingsConfigurationApi.cs", 133);
		parentPath.NullCheck("", "ValidateItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\Hardware\\HardwarePtzSettingsConfigurationApi.cs", 134);
		ThrowIfPathInvalid(path);
		if (!path.IsFolder)
		{
			if (!Hardware.HardwareExists(requestContext, path.Id))
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
			if (path.IsFolder)
			{
				throw new NotSupportedException();
			}
			ValidateResult validateResult = new ValidateResult();
			validateResult.ValidatedOk = true;
			validateResult.ResultItem = item;
			if (item.Children == null || item.Children.Length == 0)
			{
				return validateResult;
			}
			ConfigurationItem[] children = item.Children;
			foreach (ConfigurationItem obj in children)
			{
				DriverSettings updatedDriverSettings = new DriverSettings();
				MainDefinition mainDefinition = null;
				DeviceHwid hwid = null;
				DriverSettings deviceSettings = null;
				ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreatePath(obj.Path);
				DriverSettingsBusiness.QueryDeviceSettings(requestContext, configurationItemPath.Id, out mainDefinition, out hwid, out deviceSettings);
				List<ErrorResult> list = SettingsUtil.ConvertToDriverSettings(obj, mainDefinition, out updatedDriverSettings);
				if (list.Count > 0)
				{
					validateResult.ErrorResults = list.ToArray();
					validateResult.ValidatedOk = false;
					break;
				}
				list = ValidateUpdatedSettings(requestContext, configurationItemPath.Id, updatedDriverSettings);
				if (list.Count > 0)
				{
					validateResult.ErrorResults = list.ToArray();
					validateResult.ValidatedOk = false;
					break;
				}
			}
			return validateResult;
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
			if (path.IsFolder)
			{
				throw new NotSupportedException();
			}
			ValidateResult validateResult = new ValidateResult();
			validateResult.ValidatedOk = true;
			validateResult.ResultItem = item;
			if (item.Children == null || item.Children.Length == 0)
			{
				return validateResult;
			}
			ConfigurationItem[] children = item.Children;
			foreach (ConfigurationItem obj in children)
			{
				DriverSettings updatedDriverSettings = new DriverSettings();
				MainDefinition mainDefinition = null;
				DeviceHwid hwid = null;
				DriverSettings deviceSettings = null;
				ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreatePath(obj.Path);
				DriverSettingsBusiness.QueryDeviceSettings(requestContext, configurationItemPath.Id, out mainDefinition, out hwid, out deviceSettings);
				List<ErrorResult> list = SettingsUtil.ConvertToDriverSettings(item, mainDefinition, out updatedDriverSettings);
				if (list.Count > 0)
				{
					validateResult.ErrorResults = list.ToArray();
					validateResult.ValidatedOk = false;
					break;
				}
				list = SaveUpdatedSettings(requestContext, configurationItemPath.Id, updatedDriverSettings);
				if (list.Count > 0)
				{
					validateResult.ErrorResults = list.ToArray();
					validateResult.ValidatedOk = false;
					break;
				}
			}
			return validateResult;
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

	private static ConfigurationItem ConvertToHardwarePtzSettingsConfigurationItem(IGeminiRequestContext requestContext, ConfigurationItemPath parentPath, Hardware hardware)
	{
		MainDefinition mainDefinition = null;
		DriverSettings hardwareSettings = null;
		DriverSettingsBusiness.QueryHardwareSettings(requestContext, hardware.Id, out mainDefinition, out hardwareSettings);
		if (mainDefinition.Base.hardware.ptzsettings == null)
		{
			return null;
		}
		ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreateItemPath("HardwarePtzSettings", hardware.Id);
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "HardwarePtzSettings";
		configurationItem.UIHint = "DirectionHorizontal";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.ItemTypeHardwarePtzSettings;
		configurationItem.ParentPath = parentPath.Text;
		configurationItem.Path = configurationItemPath.Text;
		bool usePtzDeviceId = UsePtzDeviceId(mainDefinition, hardwareSettings);
		configurationItem.Children = ExtractDevicePtzComport(requestContext, mainDefinition, hardware, configurationItemPath, usePtzDeviceId);
		configurationItem.ChildrenFilled = true;
		List<Property> list = new List<Property>();
		configurationItem.Properties = list.ToArray();
		configurationItem.MethodIds = new string[0];
		return configurationItem;
	}

	private static ConfigurationItem[] ExtractDevicePtzComport(IGeminiRequestContext requestContext, MainDefinition mainDefinition, Hardware hardware, ConfigurationItemPath path, bool usePtzDeviceId)
	{
		List<ConfigurationItem> list = new List<ConfigurationItem>();
		string[] ids = Device.QueryDeviceIdsGeneric(requestContext, hardware.RecordingServerId, hardware.Id, DeviceTypeSearch.Camera);
		Device[] source = Device.QueryDevicesByIds(requestContext, ids);
		foreach (DeviceDefinition deviceDefinition in mainDefinition.HardwareDefinition.DeviceDefinitions)
		{
			try
			{
				if (deviceDefinition.PtzSettings == null)
				{
					continue;
				}
				CapabilityDefinition capabilityDefinition = deviceDefinition.PtzSettings.Capabilities.FindByTypeReferenceId(TypeReferenceIds.PtzEnabled);
				CapabilityDefinition capabilityDefinition2;
				if (capabilityDefinition != null && capabilityDefinition.TypeDefinition.ActiveDataType == datatype.@bool)
				{
					capabilityDefinition2 = null;
					if (!usePtzDeviceId)
					{
						goto IL_00cf;
					}
					capabilityDefinition2 = deviceDefinition.PtzSettings.Capabilities.FindByTypeReferenceId(TypeReferenceIds.PtzDeviceId);
					if (capabilityDefinition2 != null && capabilityDefinition2.TypeDefinition.ActiveDataType == datatype.@int)
					{
						goto IL_00cf;
					}
				}
				goto end_IL_004c;
				IL_00cf:
				CapabilityDefinition capabilityDefinition3 = deviceDefinition.PtzSettings.Capabilities.FindByTypeReferenceId(TypeReferenceIds.PtzComPort);
				if (capabilityDefinition3 == null || capabilityDefinition3.TypeDefinition.ActiveDataType != datatype.@int)
				{
					continue;
				}
				CapabilityDefinition capabilityDefinition4 = deviceDefinition.PtzSettings.Capabilities.FindByTypeReferenceId(TypeReferenceIds.PtzProtocol);
				if (capabilityDefinition4 != null && capabilityDefinition4.TypeDefinition.ActiveDataType == datatype.@int)
				{
					Device device = source.FirstOrDefault((Device d) => d.DeviceIndex == deviceDefinition.Hwid.Index);
					if (device != null)
					{
						ConfigurationItemPath path2 = ConfigurationItemPath.CreateItemPath("HardwarePtzDeviceSetting", device.Id);
						DriverSettings driverSettings = new DriverSettings(device.SettingsXml);
						int sort = 1;
						List<Property> list2 = new List<Property>();
						list2.AddRange(SettingsUtil.HandleCapability(requestContext, mainDefinition, capabilityDefinition, driverSettings, ref sort));
						list2.AddRange(SettingsUtil.HandleCapability(requestContext, mainDefinition, capabilityDefinition2, driverSettings, ref sort));
						list2.AddRange(SettingsUtil.HandleCapability(requestContext, mainDefinition, capabilityDefinition3, driverSettings, ref sort));
						list2.AddRange(SettingsUtil.HandleCapability(requestContext, mainDefinition, capabilityDefinition4, driverSettings, ref sort));
						list.Add(CreatePtzDeviceComSetting(device, path, path2, list2));
					}
				}
				end_IL_004c:;
			}
			catch
			{
			}
		}
		return list.ToArray();
	}

	private static ConfigurationItem CreatePtzDeviceComSetting(Device device, ConfigurationItemPath parentPath, ConfigurationItemPath path, List<Property> properties)
	{
		return new ConfigurationItem
		{
			ItemType = "HardwarePtzDeviceSetting",
			UIHint = "DirectionHorizontal",
			ItemCategory = ItemCategories.ChildItem,
			DisplayName = device.Name,
			ParentPath = parentPath.Text,
			Path = path.Text,
			MethodIds = new string[0],
			Properties = properties.ToArray(),
			Children = new ConfigurationItem[0],
			ChildrenFilled = true
		};
	}

	private static bool UsePtzDeviceId(MainDefinition mainDefinition, DriverSettings driverSettings)
	{
		try
		{
			HardwarePtzSettings ptzSettings = mainDefinition.HardwareDefinition.PtzSettings;
			if (ptzSettings == null)
			{
				return false;
			}
			CapabilityDefinition capabilityDefinition = ptzSettings.Capabilities.FindByTypeReferenceId(TypeReferenceIds.PtzUsePtzDeviceId);
			if (capabilityDefinition == null)
			{
				return false;
			}
			return Convert.ToBoolean(driverSettings.Get(ptzSettings.Hwid.Text, capabilityDefinition.Name), CultureInfo.InvariantCulture);
		}
		catch
		{
			return false;
		}
	}

	private static void ThrowIfPathInvalid(ConfigurationItemPath path)
	{
		if (path.IsFolder)
		{
			if (!string.Equals(path.ParentItemType, "Hardware", StringComparison.Ordinal))
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
}
