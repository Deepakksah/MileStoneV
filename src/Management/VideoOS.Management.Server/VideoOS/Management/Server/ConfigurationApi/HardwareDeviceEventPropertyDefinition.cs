using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class HardwareDeviceEventPropertyDefinition
{
	public static Property EventUsedProperty(bool eventUsed)
	{
		return new Property("EventUsed", PropertyTypesHelper.GetValue(eventUsed))
		{
			ValueType = "Tick",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyHardwareDeviceEventUsed,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyHardwareDeviceEventUsed",
			ToolTipTranslationId = "PropertyHardwareDeviceEventUsedTooltip",
			ValueTypeInfos = PropertyTypesHelper.GetBoolValueTypeInfos()
		};
	}

	public static Property EventIndexProperty(string eventIndex, string eventIndexDisplayName)
	{
		return new Property("EventIndex", eventIndex)
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = eventIndexDisplayName,
			ItemReference = false,
			ServerValidation = false
		};
	}

	public static Property DriverEventIdProperty(string driverEventId)
	{
		return new Property("DriverEventId", driverEventId)
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyHardwareDeviceEventDriverId,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyHardwareDeviceEventDriverId"
		};
	}

	public static Property EventDisplayNameProperty(string eventDisplayName)
	{
		return new Property("EventDisplayName", eventDisplayName)
		{
			ValueType = "String",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyHardwareDeviceEventDisplayName,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyHardwareDeviceEventDisplayName"
		};
	}
}
