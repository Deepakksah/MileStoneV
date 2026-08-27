using System;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class ManagementServerPropertyDefinition
{
	public const string MasterSiteAddress = "MasterSiteAddress";

	public const string IncludeRecordingServerTasks = "IncludeRecordingServerTasks";

	public static Property ComputerNameProperty(string computerName)
	{
		return new Property("ComputerName", computerName)
		{
			ValueType = "String",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyManagementServerComputerName,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyManagementServerComputerName"
		};
	}

	public static Property DomainNameProperty(string domainName)
	{
		return new Property("DomainName", domainName)
		{
			ValueType = "String",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyManagementServerDomainName,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyManagementServerDomainName"
		};
	}

	public static Property LastStatusHandshakeProperty(DateTime lastStatusHandshake)
	{
		return new Property("LastStatusHandshake", PropertyTypesHelper.GetValue(lastStatusHandshake))
		{
			ValueType = "DateTime",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyManagementServerLastStatusHandshake,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyManagementServerLastStatusHandshake"
		};
	}

	public static Property PhysicalMemoryProperty(long physicalMemory)
	{
		return new Property("PhysicalMemory", PropertyTypesHelper.GetValue(physicalMemory))
		{
			ValueType = "Int",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyManagementServerPhysicalMemory,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyManagementServerPhysicalMemory"
		};
	}

	public static Property PlatformProperty(string platform)
	{
		return new Property("Platform", platform)
		{
			ValueType = "String",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyManagementServerPlatform,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyManagementServerPlatform"
		};
	}

	public static Property ProcessorsProperty(long processors)
	{
		return new Property("Processors", PropertyTypesHelper.GetValue(processors))
		{
			ValueType = "Int",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyManagementServerProcessors,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyManagementServerProcessors"
		};
	}

	public static Property ServiceAccountProperty(string serviceAccount)
	{
		return new Property("ServiceAccount", serviceAccount)
		{
			ValueType = "String",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyManagementServerServiceAccount,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyManagementServerServiceAccount"
		};
	}

	public static Property SynchronizationStatusProperty(long synchronizationStatus)
	{
		return new Property("SynchronizationStatus", PropertyTypesHelper.GetValue(synchronizationStatus))
		{
			ValueType = "Int",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyManagementServerSynchronizationStatus,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyManagementServerSynchronizationStatus"
		};
	}

	public static Property MasterSiteAddressPropery(string url)
	{
		return new Property("MasterSiteAddress", url)
		{
			ValueType = "String",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyManagementServerMasterSite,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyManagementServerMasterSite"
		};
	}

	public static Property VersionProperty(Version version)
	{
		return new Property("Version", PropertyTypesHelper.GetValue(version))
		{
			ValueType = "String",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyManagementServerVersion,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyManagementServerVersion"
		};
	}

	public static Property TimeZoneProperty(string timeZone)
	{
		return new Property("TimeZone", timeZone)
		{
			ValueType = "String",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyGeneralTimeZone,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyGeneralTimeZone",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property ServerAddressUrlProperty(string url)
	{
		return new Property("Url", url)
		{
			ValueType = "String",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyManagementServerAddressUrl,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyManagementServerAddressUrl"
		};
	}

	public static Property ServerAddressTypeProperty(ServerAddressType addressType)
	{
		return new Property("AddressType", addressType.ToString())
		{
			ValueType = "Enum",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyManagementServerAddressType,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyManagementServerAddressType",
			ValueTypeInfos = PropertyTypesHelper.GetPropertyValueInfoItems(typeof(ServerAddressType))
		};
	}

	public static Property IncludeRecordingServerTasksProperty(bool value)
	{
		Property tickProperty = GeneralPropertyDefinitions.GetTickProperty("IncludeRecordingServerTasks", ConfigurationApiResources.PropertyIncludeRecordingServerTasks, value.ToString(), "PropertyIncludeRecordingServerTasks", isSettable: true);
		tickProperty.ToolTipTranslationId = "PropertyIncludeRecordingServerTasksToolTip";
		return tickProperty;
	}
}
