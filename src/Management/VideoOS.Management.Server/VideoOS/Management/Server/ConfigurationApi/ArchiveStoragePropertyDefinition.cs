using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class ArchiveStoragePropertyDefinition
{
	public static Property FramerateReductionEnabledProperty(bool isEnabled)
	{
		return new Property("FramerateReductionEnabled", PropertyTypesHelper.GetValue(isEnabled))
		{
			ValueType = "Tick",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyArchiveStorageFramerateReductionEnabled,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyArchiveStorageFramerateReductionEnabled",
			ToolTipTranslationId = "PropertyArchiveStorageFramerateReductionEnabledToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property TargetFramerateProperty(double framerate)
	{
		return new Property("TargetFramerate", PropertyTypesHelper.GetValue(framerate))
		{
			ValueType = "Double",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyArchiveStorageTargetFramerate,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyArchiveStorageTargetFramerate",
			ToolTipTranslationId = "PropertyArchiveStorageTargetFramerateToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100,
			ValueTypeInfos = PropertyTypesHelper.GetDoubleRange(0.00028, 100.0)
		};
	}

	public static Property DiskPathProperty(string diskPath, bool isSettable = false)
	{
		return new Property("DiskPath", diskPath)
		{
			ValueType = "String",
			IsSettable = isSettable,
			DisplayName = ConfigurationApiResources.PropertyArchiveStorageDiskPath,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyArchiveStorageDiskPath",
			ToolTipTranslationId = "PropertyArchiveStorageDiskPathToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property RetainMinutesProperty(int retainMinutes)
	{
		return new Property("RetainMinutes", PropertyTypesHelper.GetValue(retainMinutes))
		{
			ValueType = "Int",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyArchiveStorageRetainMinutes,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyArchiveStorageRetainMinutes",
			ToolTipTranslationId = "PropertyArchiveStorageRetainMinutesToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100,
			ValueTypeInfos = PropertyTypesHelper.GetIntegerRange(1, 525600000)
		};
	}

	public static Property MaxSizeProperty(long maxSize)
	{
		return new Property("MaxSize", PropertyTypesHelper.GetValue(maxSize))
		{
			ValueType = "Int",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyArchiveStorageMaxSize,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyArchiveStorageMaxSize",
			ToolTipTranslationId = "PropertyArchiveStorageMaxSizeToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100,
			ValueTypeInfos = PropertyTypesHelper.GetIntegerRange(1, 1099511627776L)
		};
	}

	public static Property DeleteIfOfflineProperty(bool deleteIfOffline)
	{
		return new Property("DeleteIfOffline", PropertyTypesHelper.GetValue(deleteIfOffline))
		{
			ValueType = "Tick",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyArchiveStorageDeleteIfOffline,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyArchiveStorageDeleteIfOffline",
			ToolTipTranslationId = "PropertyArchiveStorageDeleteIfOfflineToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property MountedProperty(bool mounted)
	{
		return new Property("Mounted", PropertyTypesHelper.GetValue(mounted))
		{
			ValueType = "Tick",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyArchiveStorageMounted,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyArchiveStorageMounted",
			ToolTipTranslationId = "PropertyArchiveStorageMountedToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}
}
