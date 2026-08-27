using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class RecordingServerPropertyDefinition
{
	public const string Warning = "Warning";

	public const string Ready = "Ready";

	public const string MarkedData = "MarkedData";

	public const string NoMarkedData = "NoMarkedData";

	public static Property HostNameProperty(string hostName)
	{
		return new Property("HostName", hostName)
		{
			ValueType = "String",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyRecordingServerHostName,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyRecordingServerHostName",
			ToolTipTranslationId = "PropertyRecordingServerHostNameToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property TimeZoneProperty(string timeZone)
	{
		return new Property("TimeZoneName", timeZone)
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

	public static Property SynchronizationTimeProperty(int synchronizationTime)
	{
		return new Property("SynchronizationTime", PropertyTypesHelper.GetValue(synchronizationTime))
		{
			ValueType = "Int",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyRecordingServerSynchronizationTime,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyRecordingServerSynchronizationTime",
			ToolTipTranslationId = "PropertyRecordingServerSynchronizationTimeToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100,
			ValueTypeInfos = PropertyTypesHelper.GetIntegerRange(-1, 1439)
		};
	}

	public static Property PortNumberProperty(int portNumber)
	{
		return new Property("PortNumber", PropertyTypesHelper.GetValue(portNumber))
		{
			ValueType = "Int",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyRecordingServerPortNumber,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyRecordingServerPortNumber",
			ToolTipTranslationId = "PropertyRecordingServerPortNumberToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property WebServerUriProperty(string webServerUri)
	{
		return new Property("WebServerUri", webServerUri)
		{
			ValueType = "String",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyRecordingServerWebServerUri,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyRecordingServerWebServerUri",
			ToolTipTranslationId = "PropertyRecordingServerWebServerUriToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property ActiveWebServerUriProperty(string webServerUri)
	{
		return new Property("ActiveWebServerUri", webServerUri)
		{
			ValueType = "String",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyRecordingServerActiveWebServerUri,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyRecordingServerActiveWebServerUri",
			ToolTipTranslationId = "PropertyRecordingServerActiveWebServerUriToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property PublicAccessEnabledProperty(bool enabled)
	{
		return new Property("PublicAccessEnabled", enabled.ToString())
		{
			ValueType = "Tick",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyPublicAccessEnabled,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyPublicAccessEnabled",
			ToolTipTranslationId = "PropertyPublicAccessEnabledToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property PublicWebServerHostnameProperty(string name)
	{
		return new Property("PublicWebserverHostName", name)
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyPublicWebServerHostname,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyPublicWebServerHostname",
			ToolTipTranslationId = "PropertyPublicWebServerHostnameToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property PublicWebServerPortProperty(int port)
	{
		return new Property("PublicWebserverPort", port.ToString(CultureInfo.InvariantCulture))
		{
			ValueType = "Int",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyPublicWebServerPort,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyPublicWebServerPort",
			ToolTipTranslationId = "PropertyPublicWebServerPortToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property ShutdownOnStorageFailureProperty(bool shutdown)
	{
		return new Property("ShutdownOnStorageFailure", shutdown.ToString(CultureInfo.InvariantCulture))
		{
			ValueType = "Tick",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyShutdownOnStorageFailure,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyShutdownOnStorageFailure",
			ToolTipTranslationId = "PropertyShutdownOnStorageFailureToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property MulticastServerAddressProperty(string address)
	{
		return new Property("MulticastServerAddress", address)
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyMulticastServerAddress,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyMulticastServerAddress",
			ToolTipTranslationId = "PropertyMulticastServerAddressToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property PreselectedRecorderProperty(string recorderPath)
	{
		return new Property("ItemSelection", recorderPath)
		{
			ValueType = "String",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyRemoveRecordingServer,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyRemoveRecordingServer",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property DeleteEvidenceLockedDataProperty(string enumName)
	{
		ValueTypeInfo[] valueTypeInfos = new ValueTypeInfo[2]
		{
			new ValueTypeInfo
			{
				Value = "MarkedData",
				TranslationId = "PropertyRemoveRecordingServerHasMarkedData",
				Name = ConfigurationApiResources.PropertyRemoveRecordingServerHasMarkedData
			},
			new ValueTypeInfo
			{
				Value = "NoMarkedData",
				TranslationId = "PropertyRemoveRecordingServerNoMarkedData",
				Name = ConfigurationApiResources.PropertyRemoveRecordingServerNoMarkedData
			}
		};
		return new Property("DeleteLockedData", enumName)
		{
			ValueType = "Enum",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyRemoveRecordingServerLockedData,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyRemoveRecordingServerLockedData",
			ToolTipTranslationId = "PropertyRemoveRecordingServerLockedDataToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100,
			ValueTypeInfos = valueTypeInfos
		};
	}

	public static Property UserAcceptProperty()
	{
		return new Property("UserAccept", "False")
		{
			ValueType = "Tick",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyUserAcceptance,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyUserAcceptance",
			ToolTipTranslationId = "PropertyUserAcceptanceToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property StorageNotificationProperty(string enumName)
	{
		ValueTypeInfo[] valueTypeInfos = new ValueTypeInfo[2]
		{
			new ValueTypeInfo
			{
				Value = "Warning",
				TranslationId = "PropertyRemoveRecordingServerWarningStorage",
				Name = ConfigurationApiResources.PropertyRemoveRecordingServerWarningStorage
			},
			new ValueTypeInfo
			{
				Value = "Ready",
				TranslationId = "PropertyRemoveRecordingServerReadyStorage",
				Name = ConfigurationApiResources.PropertyRemoveRecordingServerReadyStorage
			}
		};
		return new Property("StorageInfo", enumName)
		{
			ValueType = "Enum",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyDeleteStorageInfo,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyDeleteStorageInfo",
			UIImportance = UIImportance.Default,
			SortKey = 100,
			ValueTypeInfos = valueTypeInfos
		};
	}

	public static Property OtherDeviceRecordingsProperty(string enumName)
	{
		ValueTypeInfo[] valueTypeInfos = new ValueTypeInfo[2]
		{
			new ValueTypeInfo
			{
				Value = "Warning",
				TranslationId = "PropertyRemoveRecordingServerWarningHasOther",
				Name = ConfigurationApiResources.PropertyRemoveRecordingServerWarningHasOther
			},
			new ValueTypeInfo
			{
				Value = "Ready",
				TranslationId = "PropertyRemoveRecordingServerReadyHasOther",
				Name = ConfigurationApiResources.PropertyRemoveRecordingServerReadyHasOther
			}
		};
		return new Property("OtherDeviceRecordings", enumName)
		{
			ValueType = "Enum",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyOtherDeviceRecordings,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyOtherDeviceRecordings",
			UIImportance = UIImportance.Default,
			SortKey = 100,
			ValueTypeInfos = valueTypeInfos
		};
	}

	public static Property CurrentDirectoryProperty(string diskPath)
	{
		return new Property("CurrentDirectory", diskPath)
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyCurrentDirectoryOnRecorder,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyCurrentDirectoryOnRecorder",
			ToolTipTranslationId = "PropertyCurrentDirectoryOnRecorderToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 1
		};
	}

	public static Property NewDirectoryNameProperty(string diskPath)
	{
		return new Property("NewDirectoryFullName", diskPath)
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyNewDirectoryNameOnRecorder,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyNewDirectoryNameOnRecorder",
			ToolTipTranslationId = "PropertyNewDirectoryNameOnRecorderToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 1
		};
	}

	public static Property DirectoriesProperty(IEnumerable<string> directories)
	{
		Property property = new Property("Directories", directories.ToArray());
		property.ValueType = "Array";
		property.IsSettable = false;
		property.DisplayName = ConfigurationApiResources.PropertyDirectoriesOnRecorder;
		property.ItemReference = false;
		property.ServerValidation = false;
		property.TranslationId = "PropertyDirectoriesOnRecorder";
		property.ToolTipTranslationId = "PropertyDirectoriesOnRecorderToolTip";
		property.UIImportance = UIImportance.Default;
		property.SortKey = 1;
		property.ValueTypeInfos = new ValueTypeInfo[1]
		{
			new ValueTypeInfo
			{
				Name = "ArrayValueType",
				Value = "String"
			}
		};
		return property;
	}

	public static Property DrivesProperty(IEnumerable<string> drives)
	{
		Property property = new Property("Drives", drives.ToArray());
		property.ValueType = "Array";
		property.IsSettable = false;
		property.DisplayName = ConfigurationApiResources.PropertyDrivesOnRecorder;
		property.ItemReference = false;
		property.ServerValidation = false;
		property.TranslationId = "PropertyDrivesOnRecorder";
		property.ToolTipTranslationId = "PropertyDrivesOnRecorderToolTip";
		property.UIImportance = UIImportance.Default;
		property.SortKey = 1;
		property.ValueTypeInfos = new ValueTypeInfo[1]
		{
			new ValueTypeInfo
			{
				Name = "ArrayValueType",
				Value = "String"
			}
		};
		return property;
	}
}
