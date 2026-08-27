using System;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class HardwarePropertyDefinition
{
	public static Property AddressProperty(string address)
	{
		return new Property("Address", address)
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyHardwareAddress,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyHardwareAddress",
			ToolTipTranslationId = "PropertyHardwareAddressToolTip"
		};
	}

	public static Property UserNameProperty(string userName)
	{
		return new Property("UserName", userName)
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyUserName,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyUserName"
		};
	}

	public static Property ModelProperty(string productId)
	{
		return new Property("Model", productId)
		{
			ValueType = "String",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyModel,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyModel",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property PasswordProperty(string password)
	{
		return new Property("Password", password)
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyHardwarePassword,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyHardwarePassword",
			ToolTipTranslationId = "PropertyHardwarePassword",
			UIImportance = UIImportance.Password,
			SortKey = 100
		};
	}

	public static Property PasswordLastModifiedProperty(DateTime? lastModified)
	{
		return new Property("PasswordLastModified", PropertyTypesHelper.GetValue(lastModified ?? DateTime.MinValue))
		{
			ValueType = "DateTime",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyPasswordLastModified,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyPasswordLastModified",
			ToolTipTranslationId = "PropertyPasswordLastModified",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property IgnoreSourceRecordingServerProperty(bool ignoreSourceRecordingServer)
	{
		return new Property("ignoreSourceRecordingServer", PropertyTypesHelper.GetValue(ignoreSourceRecordingServer))
		{
			ValueType = "Tick",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyHardwareIgnoreSourceRecordingServer,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyHardwareIgnoreSourceRecordingServer",
			ToolTipTranslationId = "PropertyHardwareIgnoreSourceRecordingServerToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property HardwareDriverProperty(ConfigurationItemPath path)
	{
		return new Property("HardwareDriverPath", path.Text)
		{
			ValueType = "Path",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyHardwareDriverPath,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyHardwareDriverPath",
			ToolTipTranslationId = "PropertyHardwareDriverPathToolTip"
		};
	}

	public static Property DevicePathProperty(ConfigurationItemPath path)
	{
		return new Property("DevicePath", path.Text)
		{
			ValueType = "Path",
			IsSettable = false,
			UIImportance = UIImportance.Summary,
			DisplayName = ConfigurationApiResources.PropertyUpdateHardwareDevicePath,
			ItemReference = true,
			ServerValidation = false,
			TranslationId = "PropertyUpdateHardwareDevicePath",
			ToolTipTranslationId = "PropertyDevicePathToolTip"
		};
	}

	public static Property DeviceNameProperty(string name)
	{
		return new Property("DeviceName", name)
		{
			ValueType = "String",
			IsSettable = false,
			UIImportance = UIImportance.Summary,
			DisplayName = ConfigurationApiResources.PropertyDeviceName,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyDeviceName",
			ToolTipTranslationId = "PropertyDeviceNameToolTip"
		};
	}

	public static Property DeviceTypeProperty(string typeOfDevice)
	{
		return new Property("DeviceType", typeOfDevice)
		{
			ValueType = "String",
			IsSettable = false,
			UIImportance = UIImportance.Summary,
			DisplayName = ConfigurationApiResources.PropertyDeviceType,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyDeviceType",
			ToolTipTranslationId = "PropertyDeviceTypeToolTip"
		};
	}

	public static Property ChangeProperty(string change)
	{
		return new Property("ChangeType", change)
		{
			ValueType = "String",
			IsSettable = false,
			UIImportance = UIImportance.Summary,
			DisplayName = ConfigurationApiResources.PropertyUpdateHardwareEventsChanged,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyUpdateHardwareEventsChanged",
			ToolTipTranslationId = "PropertyDeviceChangeToolTip"
		};
	}

	public static Property UpdatePasswordOnDeviceProperty()
	{
		return new Property("UpdatePasswordOnDevice", PropertyTypesHelper.GetValue(value: false))
		{
			ValueType = "Tick",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyUpdatePasswordOnDevice,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyUpdatePasswordOnDevice",
			ToolTipTranslationId = "PropertyUpdatePasswordOnDeviceTooltip",
			ValueTypeInfos = PropertyTypesHelper.GetBoolValueTypeInfos()
		};
	}

	public static Property ReplaceHardwareAddressProperty(string value)
	{
		return new Property("Address", PropertyTypesHelper.GetValue(value))
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyReplaceHardwareAddress,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyReplaceHardwareAddress",
			ToolTipTranslationId = "PropertyReplaceHardwareAddressToolTip"
		};
	}

	public static Property ReplaceHardwarePortProperty(int value)
	{
		return new Property("Port", PropertyTypesHelper.GetValue(value))
		{
			ValueType = "Int",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyReplaceHardwarePort,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyReplaceHardwarePort",
			ToolTipTranslationId = "PropertyReplaceHardwarePortToolTip",
			ValueTypeInfos = PropertyTypesHelper.GetIntegerRange(1, 65535)
		};
	}

	public static Property ReplaceHardwareUseHttpsProperty(bool value)
	{
		return new Property("UseHttps", PropertyTypesHelper.GetValue(value))
		{
			ValueType = "Tick",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyReplaceHardwareUseHttps,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyReplaceHardwareUseHttps",
			ToolTipTranslationId = "PropertyReplaceHardwareUseHttpsToolTip",
			ValueTypeInfos = PropertyTypesHelper.GetBoolValueTypeInfos()
		};
	}

	public static Property ReplaceHardwareHttpsPortProperty(int value)
	{
		return new Property("HttpsPort", PropertyTypesHelper.GetValue(value))
		{
			ValueType = "Int",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyReplaceHardwareHttpsPort,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyReplaceHardwareHttpsPort",
			ToolTipTranslationId = "PropertyReplaceHardwareHttpsPortToolTip",
			ValueTypeInfos = PropertyTypesHelper.GetIntegerRange(1, 65535)
		};
	}

	public static Property ReplaceHardwareUserNameProperty(string value)
	{
		return new Property("UserName", PropertyTypesHelper.GetValue(value))
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyReplaceHardwareUserName,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyReplaceHardwareUserName",
			ToolTipTranslationId = "PropertyReplaceHardwareUserNameToolTip"
		};
	}

	public static Property ReplaceHardwarePasswordProperty()
	{
		return new Property("Password", PropertyTypesHelper.GetValue(""))
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyReplaceHardwarePassword,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyReplaceHardwarePassword",
			ToolTipTranslationId = "PropertyReplaceHardwarePasswordToolTip"
		};
	}

	public static Property ReplaceHardwareCustomDriverDataProperty()
	{
		return new Property("CustomDriverData", PropertyTypesHelper.GetValue(""))
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyReplaceHardwareCustomDriverData,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyReplaceHardwareCustomDriverData",
			ToolTipTranslationId = "PropertyReplaceHardwareCustomDriverDataToolTip"
		};
	}

	public static Property ReplaceHardwareAllowDeletingDisabledDevicesProperty(bool value)
	{
		return new Property("AllowDeletingDisabledDevices", PropertyTypesHelper.GetValue(value))
		{
			ValueType = "Tick",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyReplaceHardwareAllowDeletingDisabledDevices,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyReplaceHardwareAllowDeletingDisabledDevices",
			ToolTipTranslationId = "PropertyReplaceHardwareAllowDeletingDisabledDevicesToolTip",
			ValueTypeInfos = PropertyTypesHelper.GetBoolValueTypeInfos()
		};
	}
}
