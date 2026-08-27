using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class PtzPresetPropertyDefinition
{
	public static Property DevicePresetProperty(bool isDevicePtzPreset)
	{
		return new Property("DevicePreset", isDevicePtzPreset.ToString())
		{
			ValueType = "Tick",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyPresetDevicePreset,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyPresetDevicePreset",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property DefaultPresetProperty(bool isDefaultPtzPreset)
	{
		return new Property("DefaultPreset", isDefaultPtzPreset.ToString())
		{
			ValueType = "Tick",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyPresetDefaultPreset,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyPresetDefaultPreset",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property PanProperty(double number)
	{
		return new Property("Pan", PropertyTypesHelper.GetValue(number))
		{
			ValueType = "Double",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyPresetPan,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyPresetPan",
			UIImportance = UIImportance.Default,
			SortKey = 100,
			ValueTypeInfos = PropertyTypesHelper.GetDoubleRange(-1.0, 1.0)
		};
	}

	public static Property TiltProperty(double number)
	{
		return new Property("Tilt", PropertyTypesHelper.GetValue(number))
		{
			ValueType = "Double",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyPresetTilt,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyPresetTilt",
			UIImportance = UIImportance.Default,
			SortKey = 100,
			ValueTypeInfos = PropertyTypesHelper.GetDoubleRange(-1.0, 1.0)
		};
	}

	public static Property ZoomProperty(double number)
	{
		return new Property("Zoom", PropertyTypesHelper.GetValue(number))
		{
			ValueType = "Double",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyPresetZoom,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyPresetZoom",
			UIImportance = UIImportance.Default,
			SortKey = 100,
			ValueTypeInfos = PropertyTypesHelper.GetDoubleRange(0.0, 1.0)
		};
	}

	public static Property DevicePresetInternalIdProperty(string value)
	{
		return new Property("DevicePresetInternalId", value)
		{
			ValueType = "String",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyPresetDevicePresetInternalId,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyPresetDevicePresetInternalId",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property LockedProperty(bool locked)
	{
		return new Property("Locked", locked.ToString())
		{
			ValueType = "Tick",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyPresetLocked,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyPresetLocked",
			UIImportance = UIImportance.Default,
			SortKey = 120
		};
	}
}
