using VideoOS.ConfigurationAPI;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class MotionDetectionPropertyDefinition
{
	public static Property ManualSensitivityEnabled(bool isEnabled)
	{
		return new Property("ManualSensitivityEnabled", PropertyTypesHelper.GetValue(isEnabled))
		{
			ValueType = "Tick",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyMotionDetectionManualSensitivityEnabled,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyMotionDetectionManualSensitivityEnabled",
			ToolTipTranslationId = "PropertyMotionDetectionManualSensitivityEnabledToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100,
			ValueTypeInfos = PropertyTypesHelper.GetBoolValueTypeInfos()
		};
	}

	public static Property ManualSensitivity(int level)
	{
		return new Property("ManualSensitivity", PropertyTypesHelper.GetValue(level))
		{
			ValueType = "Int",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyMotionDetectionManualSensitivity,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyMotionDetectionManualSensitivity",
			ToolTipTranslationId = "PropertyMotionDetectionManualSensitivityToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property Threshold(int thresholdValue)
	{
		return new Property("Threshold", PropertyTypesHelper.GetValue(thresholdValue))
		{
			ValueType = "Int",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyMotionDetectionThreshold,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyMotionDetectionThreshold",
			ToolTipTranslationId = "PropertyMotionDetectionThresholdToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property KeyframesOnly(bool isEnabled)
	{
		return new Property("KeyframesOnly", PropertyTypesHelper.GetValue(isEnabled))
		{
			ValueType = "Tick",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyMotionDetectionKeyFramesOnly,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyMotionDetectionKeyFramesOnly",
			ToolTipTranslationId = "PropertyMotionDetectionKeyFramesOnlyToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100,
			ValueTypeInfos = PropertyTypesHelper.GetBoolValueTypeInfos()
		};
	}

	public static Property ProcessTime(ProcessTime.ProcessTimeType time)
	{
		return new Property("ProcessTime", PropertyTypesHelper.GetValue(time))
		{
			ValueType = "Enum",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyMotionDetectionProcessTime,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyMotionDetectionProcessTime",
			ToolTipTranslationId = "PropertyMotionDetectionProcessTimeToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100,
			ValueTypeInfos = PropertyTypesHelper.GetPropertyValueInfoItems(typeof(ProcessTime.ProcessTimeType))
		};
	}

	public static Property DetectionMethod(DetectionMethod.DetectionMethodType method)
	{
		return new Property("DetectionMethod", PropertyTypesHelper.GetValue(method))
		{
			ValueType = "Enum",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyMotionDetectionDetectionMethod,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyMotionDetectionDetectionMethod",
			ToolTipTranslationId = "PropertyMotionDetectionDetectionMethodToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100,
			ValueTypeInfos = PropertyTypesHelper.GetPropertyValueInfoItems(typeof(DetectionMethod.DetectionMethodType))
		};
	}

	public static Property GenerateMotionMetadata(bool isEnabled)
	{
		return new Property("GenerateMotionMetadata", PropertyTypesHelper.GetValue(isEnabled))
		{
			ValueType = "Tick",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyMotionDetectionGenerateMotionMetadata,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyMotionDetectionGenerateMotionMetadata",
			ToolTipTranslationId = "PropertyMotionDetectionGenerateMotionMetadataToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100,
			ValueTypeInfos = PropertyTypesHelper.GetBoolValueTypeInfos()
		};
	}

	public static Property UseExcludeRegions(bool value)
	{
		return new Property("UseExcludeRegions", PropertyTypesHelper.GetValue(value))
		{
			ValueType = "Tick",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyMotionDetectionUseExcludeRegions,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyMotionDetectionUseExcludeRegions",
			ToolTipTranslationId = "PropertyMotionDetectionUseExcludeRegionsToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100,
			ValueTypeInfos = PropertyTypesHelper.GetBoolValueTypeInfos()
		};
	}

	public static Property GridSize(GridSize.GridSizeType size)
	{
		return new Property("GridSize", PropertyTypesHelper.GetValue(size))
		{
			ValueType = "Enum",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyMotionDetectionGridSize,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyMotionDetectionGridSize",
			ToolTipTranslationId = "PropertyMotionDetectionGridSizeToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100,
			ValueTypeInfos = PropertyTypesHelper.GetPropertyValueInfoItems(typeof(GridSize.GridSizeType))
		};
	}

	public static Property ExcludeRegions(string regions)
	{
		return new Property("ExcludeRegions", PropertyTypesHelper.GetValue(regions))
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyMotionDetectionExcludeRegions,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyMotionDetectionExcludeRegions",
			ToolTipTranslationId = "PropertyMotionDetectionExcludeRegionsToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property HardwareAccelerationMode(HardwareAccelerationMode.HardwareAccelerationModeType mode)
	{
		return new Property("HardwareAccelerationMode", PropertyTypesHelper.GetValue(mode))
		{
			ValueType = "Enum",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyMotionDetectionHardwareAccelerationMode,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyMotionDetectionHardwareAccelerationMode",
			ToolTipTranslationId = "PropertyMotionDetectionHardwareAccelerationModeToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100,
			ValueTypeInfos = PropertyTypesHelper.GetPropertyValueInfoItems(typeof(HardwareAccelerationMode.HardwareAccelerationModeType))
		};
	}
}
