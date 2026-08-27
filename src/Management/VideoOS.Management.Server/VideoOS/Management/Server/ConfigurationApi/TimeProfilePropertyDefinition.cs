using System;
using System.Collections.Generic;
using System.Globalization;
using VideoOS.Common.SharedTimeProfile;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class TimeProfilePropertyDefinition
{
	public static Property TimeProfileTypeProperty(TimeProfileType timeProfileType, bool isSettable)
	{
		return new Property("TimeProfileType", timeProfileType.ToString())
		{
			ValueType = "Enum",
			IsSettable = isSettable,
			DisplayName = ConfigurationApiResources.TimeProfileTypeName,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "TimeProfileTypeName",
			UIImportance = UIImportance.Default,
			SortKey = 100,
			ValueTypeInfos = ConvertToEnumArray(typeof(TimeProfileType))
		};
	}

	public static Property SubjectProperty(string subject, bool isSettable)
	{
		return new Property("Subject", subject)
		{
			ValueType = "String",
			IsSettable = isSettable,
			DisplayName = ConfigurationApiResources.TimeProfileAppointmentSubjectName,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "TimeProfileAppointmentSubjectName",
			UIImportance = UIImportance.Summary,
			SortKey = 111
		};
	}

	public static Property AllDayEventProperty(bool isAllDay, bool isSettable)
	{
		return new Property("AllDayEvent", isAllDay ? "True" : "False")
		{
			ValueType = "Tick",
			IsSettable = isSettable,
			DisplayName = ConfigurationApiResources.TimeProfileAllDayEventName,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "TimeProfileAllDayEventName",
			UIImportance = UIImportance.Summary,
			SortKey = 101
		};
	}

	public static Property OriginalStartDateTimeProperty(DateTime dateTime)
	{
		return new Property("OriginalStartDateTime", dateTime.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture))
		{
			ValueType = "DateTime",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.TimeProfileOriginalStartDateName,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "TimeProfileOriginalStartDateName",
			UIImportance = UIImportance.Hidden,
			SortKey = 102
		};
	}

	public static Property StartDateTimeProperty(DateTime dateTime, bool isSettable)
	{
		return new Property("StartDateTime", dateTime.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture))
		{
			ValueType = "DateTime",
			IsSettable = isSettable,
			DisplayName = ConfigurationApiResources.TimeProfileStartDateName,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "TimeProfileStartDateName",
			UIImportance = UIImportance.Summary,
			SortKey = 102
		};
	}

	public static Property EndDateTimeProperty(DateTime dateTime, bool isSettable)
	{
		return new Property("EndDateTime", dateTime.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture))
		{
			ValueType = "DateTime",
			IsSettable = isSettable,
			DisplayName = ConfigurationApiResources.TimeProfileEndDateName,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "TimeProfileEndDateName",
			UIImportance = UIImportance.Summary,
			SortKey = 102
		};
	}

	public static Property VarianceSubjectProperty(string subject)
	{
		return new Property("VarianceSubject", subject)
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.VarianceSubjectName,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "VarianceSubjectName",
			UIImportance = UIImportance.Summary,
			SortKey = 111
		};
	}

	public static Property AppointmentRootIdProperty(Guid id)
	{
		return new Property("AppointmentRootId", id.ToString())
		{
			ValueType = "String",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.TimeProfileRecurrenceAppointmentRootIdName,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "TimeProfileRecurrenceAppointmentRootIdName",
			UIImportance = UIImportance.Hidden,
			SortKey = 110
		};
	}

	public static Property VarianceStartDateTimeProperty(DateTime dateTime)
	{
		return new Property("VarianceStartDateTime", dateTime.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture))
		{
			ValueType = "DateTime",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.TimeProfileVarianceStartDateName,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "TimeProfileVarianceStartDateName",
			UIImportance = UIImportance.Summary,
			SortKey = 112
		};
	}

	public static Property VarianceOriginalStartDateTimeProperty(DateTime dateTime, bool isSettable)
	{
		return new Property("VarianceOriginalStartDateTime", dateTime.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture))
		{
			ValueType = "DateTime",
			IsSettable = isSettable,
			DisplayName = ConfigurationApiResources.TimeProfileVarianceOriginalStartDateName,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "TimeProfileVarianceOriginalStartDateName",
			UIImportance = UIImportance.Summary,
			SortKey = 112
		};
	}

	public static Property VarianceEndDateTimeProperty(DateTime dateTime)
	{
		return new Property("VarianceEndDateTime", dateTime.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture))
		{
			ValueType = "DateTime",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.TimeProfileVarianceEndDateName,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "TimeProfileVarianceEndDateName",
			UIImportance = UIImportance.Summary,
			SortKey = 113
		};
	}

	public static Property RecurrenceDescriptionProperty(string description)
	{
		return new Property("RecurrenceDescription", description)
		{
			ValueType = "String",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.TimeProfileRecurrenceDescriptionName,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "TimeProfileRecurrenceDescriptionName",
			UIImportance = UIImportance.Summary,
			SortKey = 120
		};
	}

	public static Property RecurrenceOccurrenceDurationProperty(TimeSpan timeSpan)
	{
		return new Property("RecurrenceOccurrenceDuration", timeSpan.ToString())
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.TimeProfileRecurrenceOccurrenceDurationName,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "TimeProfileRecurrenceOccurrenceDurationName",
			UIImportance = UIImportance.Summary,
			SortKey = 121
		};
	}

	public static Property RecurrenceOccurrenceStartTimeProperty(DateTime dateTime)
	{
		return new Property("RecurrenceOccurrenceStartTime", dateTime.ToString("HH:mm:ss", CultureInfo.InvariantCulture))
		{
			ValueType = "Time",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.TimeProfileRecurrenceOccurrenceStartTimeName,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "TimeProfileRecurrenceOccurrenceStartTimeName",
			ToolTipTranslationId = "TimeProfileRecurrenceOccurrenceStartTimeNameTip",
			UIImportance = UIImportance.Summary,
			SortKey = 122
		};
	}

	public static Property RecurrencePatternDayOfMonthProperty(int day)
	{
		return new Property("RecurrencePatternDayOfMonth", day.ToString(CultureInfo.InvariantCulture))
		{
			ValueType = "Int",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.TimeProfileRecurrencePatternDayOfMonthName,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "TimeProfileRecurrencePatternDayOfMonthName",
			ToolTipTranslationId = "TimeProfileRecurrencePatternDayOfMonthTip",
			UIImportance = UIImportance.Summary,
			SortKey = 123
		};
	}

	public static Property RecurrencePatternDaysOfWeekProperty(int dayMask)
	{
		return new Property("RecurrencePatternDaysOfWeek", dayMask.ToString(CultureInfo.InvariantCulture))
		{
			ValueType = "Int",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.TimeProfileRecurrencePatternDaysOfWeekName,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "TimeProfileRecurrencePatternDaysOfWeekName",
			ToolTipTranslationId = "TimeProfileRecurrencePatternDaysOfWeekTip",
			UIImportance = UIImportance.Summary,
			SortKey = 124
		};
	}

	public static Property RecurrencePatternFrequencyProperty(RecurrencePatternFrequencyType frequencyType)
	{
		return new Property("RecurrencePatternFrequency", frequencyType.ToString())
		{
			ValueType = "Enum",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.TimeProfileRecurrencePatternFrequencyName,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "TimeProfileRecurrencePatternFrequencyName",
			UIImportance = UIImportance.Summary,
			SortKey = 125,
			ValueTypeInfos = ConvertToEnumArray(typeof(RecurrencePatternFrequencyType))
		};
	}

	public static Property RecurrencePatternMonthOfYearProperty(int month)
	{
		return new Property("RecurrencePatternMonthOfYear", month.ToString(CultureInfo.InvariantCulture))
		{
			ValueType = "Int",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.TimeProfileRecurrencePatternMonthOfYearName,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "TimeProfileRecurrencePatternMonthOfYearName",
			ToolTipTranslationId = "TimeProfileRecurrencePatternMonthOfYearTip",
			UIImportance = UIImportance.Summary,
			SortKey = 126
		};
	}

	public static Property RecurrencePatternOccurrenceOfDayInMonthProperty(PatternOccurrenceOfDayInMonthType dayInMonth)
	{
		return new Property("RecurrencePatternOccurrenceOfDayInMonth", dayInMonth.ToString())
		{
			ValueType = "Enum",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.TimeProfileRecurrencePatternOccurrenceOfDayInMonthName,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "TimeProfileRecurrencePatternOccurrenceOfDayInMonthName",
			ToolTipTranslationId = "TimeProfileRecurrencePatternOccurrenceOfDayInMonthTip",
			UIImportance = UIImportance.Summary,
			SortKey = 127,
			ValueTypeInfos = ConvertToEnumArray(typeof(PatternOccurrenceOfDayInMonthType))
		};
	}

	public static Property RecurrencePatternIntervalProperty(int interval)
	{
		return new Property("RecurrencePatternInterval", interval.ToString(CultureInfo.InvariantCulture))
		{
			ValueType = "Int",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.TimeProfileRecurrencePatternIntervalName,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "TimeProfileRecurrencePatternIntervalName",
			UIImportance = UIImportance.Summary,
			SortKey = 128
		};
	}

	public static Property RecurrencePatternTypeProperty(PatternType patternType)
	{
		return new Property("RecurrencePatternType", patternType.ToString())
		{
			ValueType = "Enum",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.TimeProfileRecurrencePatternTypeName,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "TimeProfileRecurrencePatternTypeName",
			ToolTipTranslationId = "TimeProfileRecurrencePatternTypeTip",
			UIImportance = UIImportance.Summary,
			SortKey = 129,
			ValueTypeInfos = ConvertToEnumArray(typeof(PatternType))
		};
	}

	public static Property RecurrenceRangeEndDateProperty(DateTime dateTime)
	{
		return new Property("RecurrenceRangeEndDate", dateTime.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture))
		{
			ValueType = "DateTime",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.TimeProfileRecurrenceRangeEndDateName,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "TimeProfileRecurrenceRangeEndDateName",
			UIImportance = UIImportance.Summary,
			SortKey = 130
		};
	}

	public static Property RecurrenceRangeStartDateProperty(DateTime dateTime)
	{
		return new Property("RecurrenceRangeStartDate", dateTime.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture))
		{
			ValueType = "DateTime",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.TimeProfileRecurrenceRangeStartDateName,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "TimeProfileRecurrenceRangeStartDateName",
			UIImportance = UIImportance.Summary,
			SortKey = 131
		};
	}

	public static Property RecurrenceRangeLimitProperty(RangeLimit rangeLimit)
	{
		return new Property("RecurrenceRangeLimit", rangeLimit.ToString())
		{
			ValueType = "Enum",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.TimeProfileRecurrenceRangeLimitName,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "TimeProfileRecurrenceRangeLimitName",
			UIImportance = UIImportance.Summary,
			SortKey = 132,
			ValueTypeInfos = ConvertToEnumArray(typeof(RangeLimit))
		};
	}

	public static Property RecurrenceRangeMaxOccurrencesProperty(int max)
	{
		return new Property("RecurrenceRangeMaxOccurrences", max.ToString(CultureInfo.InvariantCulture))
		{
			ValueType = "Int",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.TimeProfileRecurrenceRangeMaxOccurrencesName,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "TimeProfileRecurrenceRangeMaxOccurrencesName",
			UIImportance = UIImportance.Summary,
			SortKey = 133
		};
	}

	public static Property SunClockGpsCoordinateProperty(GPSCoordinate position)
	{
		if (position == null)
		{
			throw new ArgumentNullException("position");
		}
		return new Property("SunclockGPSCoordinate", position.DisplayName)
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.TimeProfileSunclockGPSCoordinateName,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "TimeProfileSunclockGPSCoordinateName",
			ToolTipTranslationId = "TimeProfileSunclockGPSCoordinateTip",
			UIImportance = UIImportance.Summary,
			SortKey = 110
		};
	}

	public static Property SunClockSunriseOffsetProperty(int sunrise)
	{
		return new Property("SunclockSunRise", sunrise.ToString(CultureInfo.InvariantCulture))
		{
			ValueType = "Int",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.TimeProfileSunclockSunriseName,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "TimeProfileSunclockSunriseName",
			ToolTipTranslationId = "TimeProfileSunclockSunriseTip",
			UIImportance = UIImportance.Summary,
			SortKey = 110
		};
	}

	public static Property SunClockSunsetOffsetProperty(int sunset)
	{
		return new Property("SunclockSunSet", sunset.ToString(CultureInfo.InvariantCulture))
		{
			ValueType = "Int",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.TimeProfileSunclockSunsetName,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "TimeProfileSunclockSunsetName",
			ToolTipTranslationId = "TimeProfileSunclockSunsetTip",
			UIImportance = UIImportance.Summary,
			SortKey = 110
		};
	}

	public static Property SunClockTimeZoneProperty(TimeZoneInfo timeZoneInfo)
	{
		if (timeZoneInfo == null)
		{
			throw new ArgumentNullException("timeZoneInfo");
		}
		Property property = new Property("SunclockTimeZone", timeZoneInfo.Id)
		{
			ValueType = "Enum",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.TimeProfileSunclockTimeZoneName,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "TimeProfileSunclockTimeZoneName",
			ToolTipTranslationId = "TimeProfileSunclockTimeZoneTip",
			UIImportance = UIImportance.Summary,
			SortKey = 110
		};
		List<ValueTypeInfo> list = new List<ValueTypeInfo>();
		foreach (TimeZoneInfo systemTimeZone in TimeZoneInfo.GetSystemTimeZones())
		{
			list.Add(new ValueTypeInfo
			{
				Name = systemTimeZone.DisplayName,
				Value = systemTimeZone.Id
			});
		}
		property.ValueTypeInfos = list.ToArray();
		return property;
	}

	private static ValueTypeInfo[] ConvertToEnumArray(Type type)
	{
		List<ValueTypeInfo> list = new List<ValueTypeInfo>();
		string[] enumNames = type.GetEnumNames();
		Array enumValues = type.GetEnumValues();
		for (int i = 0; i < enumNames.Length; i++)
		{
			list.Add(new ValueTypeInfo
			{
				Name = enumNames[i],
				Value = enumValues.GetValue(i).ToString()
			});
		}
		return list.ToArray();
	}
}
