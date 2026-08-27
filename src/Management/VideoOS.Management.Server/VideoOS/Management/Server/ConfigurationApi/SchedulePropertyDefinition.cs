using VideoOS.ConfigurationAPI;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class SchedulePropertyDefinition
{
	public static Property FrequencyTypeProperty(FrequencyType frequencyType)
	{
		return new Property("FrequencyType", frequencyType.ToString())
		{
			ValueType = "Enum",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyScheduleFrequencyType,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyScheduleFrequencyType",
			ToolTipTranslationId = "PropertyScheduleFrequencyTypeToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100,
			ValueTypeInfos = PropertyTypesHelper.GetPropertyValueInfoItems(typeof(FrequencyType))
		};
	}

	public static Property FrequencyRecurrenceFactorProperty(int frequencyRecurrenceFactor)
	{
		return new Property("FrequencyRecurrenceFactor", PropertyTypesHelper.GetValue(frequencyRecurrenceFactor))
		{
			ValueType = "Int",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyScheduleFrequencyRecurrenceFactor,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyScheduleFrequencyRecurrenceFactor",
			ToolTipTranslationId = "PropertyScheduleFrequencyRecurrenceFactorToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100,
			ValueTypeInfos = PropertyTypesHelper.GetIntegerRange(0, int.MaxValue)
		};
	}

	public static Property FrequencyIntervalProperty(int frequencyInterval)
	{
		return new Property("FrequencyInterval", PropertyTypesHelper.GetValue(frequencyInterval))
		{
			ValueType = "Int",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyScheduleFrequencyInterval,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyScheduleFrequencyInterval",
			ToolTipTranslationId = "PropertyScheduleFrequencyIntervalToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100,
			ValueTypeInfos = PropertyTypesHelper.GetIntegerRange(0, 9)
		};
	}

	public static Property FrequencyRelativeIntervalProperty(FrequencyRelativeInterval frequencyRelativeInterval)
	{
		return new Property("FrequencyRelativeInterval", frequencyRelativeInterval.ToString())
		{
			ValueType = "Enum",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyScheduleFrequencyRelativeInterval,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyScheduleFrequencyRelativeInterval",
			ToolTipTranslationId = "PropertyScheduleFrequencyRelativeIntervalToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100,
			ValueTypeInfos = PropertyTypesHelper.GetPropertyValueInfoItems(typeof(FrequencyRelativeInterval))
		};
	}

	public static Property FrequencySubDayTypeProperty(FrequencySubDayType frequencySubDayType)
	{
		return new Property("FrequencySubDayType", frequencySubDayType.ToString())
		{
			ValueType = "Enum",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyScheduleFrequencySubDayType,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyScheduleFrequencySubDayType",
			ToolTipTranslationId = "PropertyScheduleFrequencySubDayTypeToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100,
			ValueTypeInfos = PropertyTypesHelper.GetPropertyValueInfoItems(typeof(FrequencySubDayType))
		};
	}

	public static Property FrequencySubDayIntervalProperty(int frequencySubDayInterval)
	{
		return new Property("FrequencySubDayInterval", PropertyTypesHelper.GetValue(frequencySubDayInterval))
		{
			ValueType = "Int",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyScheduleFrequencySubDayInterval,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyScheduleFrequencySubDayInterval",
			ToolTipTranslationId = "PropertyScheduleFrequencySubDayIntervalToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100,
			ValueTypeInfos = PropertyTypesHelper.GetIntegerRange(0, int.MaxValue)
		};
	}

	public static Property ActiveStartTimeOfDayProperty(double activeStartTimeOfDayInSeconds)
	{
		return new Property("ActiveStartTimeOfDay", PropertyTypesHelper.GetValue(activeStartTimeOfDayInSeconds))
		{
			ValueType = "Double",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyScheduleActiveStartTimeOfDay,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyScheduleActiveStartTimeOfDay",
			ToolTipTranslationId = "PropertyScheduleActiveStartTimeOfDayToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100,
			ValueTypeInfos = PropertyTypesHelper.GetDoubleRange(0.0, 86399.0)
		};
	}

	public static Property ActiveEndTimeOfDayProperty(double activeEndTimeOfDayInSeconds)
	{
		return new Property("ActiveEndTimeOfDay", PropertyTypesHelper.GetValue(activeEndTimeOfDayInSeconds))
		{
			ValueType = "Double",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyScheduleActiveEndTimeOfDay,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyScheduleActiveEndTimeOfDay",
			ToolTipTranslationId = "PropertyScheduleActiveEndTimeOfDayToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100,
			ValueTypeInfos = PropertyTypesHelper.GetDoubleRange(0.0, 86399.0)
		};
	}
}
