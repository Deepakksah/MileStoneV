using VideoOS.ConfigurationAPI;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class HardwareDriverPropertyDefinition
{
	public static Property DriverNumberProperty(int driverNumber)
	{
		return new Property("Number", PropertyTypesHelper.GetValue(driverNumber))
		{
			ValueType = "Int",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyHardwareDriverNumber,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyHardwareDriverNumber"
		};
	}

	public static Property GroupNameProperty(string groupName)
	{
		return new Property("GroupName", groupName)
		{
			ValueType = "String",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyHardwareDriverGroupName,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyHardwareDriverGroupName"
		};
	}

	public static Property UseCountProperty(int useCount)
	{
		return new Property("UseCount", PropertyTypesHelper.GetValue(useCount))
		{
			ValueType = "Int",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyHardwareDriverUseCount,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyHardwareDriverUseCount"
		};
	}

	public static Property DriverVersionProperty(string version)
	{
		return new Property("DriverVersion", version)
		{
			ValueType = "String",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyHardwareDriverDriverVersion,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyHardwareDriverDriverVersion"
		};
	}

	public static Property DriverRevisionProperty(string revision)
	{
		return new Property("DriverRevision", revision)
		{
			ValueType = "String",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyHardwareDriverDriverRevision,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyHardwareDriverDriverRevision"
		};
	}

	public static Property DriverTypeProperty(DriverType driverType)
	{
		return new Property("DriverType", driverType.ToString())
		{
			ValueType = "Enum",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyHardwareDriverDriverType,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyHardwareDriverDriverType",
			ValueTypeInfos = PropertyTypesHelper.GetPropertyValueInfoItems(typeof(DriverType))
		};
	}
}
