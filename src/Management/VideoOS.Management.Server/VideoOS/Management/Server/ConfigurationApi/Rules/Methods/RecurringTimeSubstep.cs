using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;
using VideoOS.Management.Server.ConfigurationApi.Rules.Builders;
using VideoOS.Management.Server.Properties;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Methods;

public class RecurringTimeSubstep : IWizardStep
{
	private class RecurringTimeProperty
	{
		public string Name { get; }

		public string DisplayName { get; }

		public string TranslationId { get; }

		public Func<string, string, string, string, IDictionary<string, string>, Property> Function { get; }

		public string DefaultValue { get; }

		public IDictionary<string, string> Members { get; }

		public RecurringTimeProperty(string name, string displayName, string translationId, Func<string, string, string, string, IDictionary<string, string>, Property> functionName, string defaultValue = "", IDictionary<string, string> members = null)
		{
			Name = name;
			DisplayName = displayName;
			TranslationId = translationId;
			Function = functionName;
			DefaultValue = defaultValue;
			Members = members;
		}
	}

	private readonly IDictionary<FrequencyType, List<RecurringTimeProperty>> _frequencyProperties;

	private readonly IDictionary<ScheduleTypes.SubdayType, List<RecurringTimeProperty>> _subDayProperties;

	private readonly IDictionary<string, string> _frequencyTypes;

	private readonly IDictionary<string, string> _dailyFrequencyTypes;

	private static readonly Dictionary<string, string> _daysOfWeekEnumValues = Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>().ToDictionary((DayOfWeek t) => t.ToString(), (DayOfWeek t) => ConfigurationItemConditionsHelper._daysOfWeekNamesByValue[t]);

	private static readonly IDictionary<string, string> _dayEnumValues = ConfigurationItemRuleTypeBuilder._relativeDayTypeEnumValues;

	private static readonly IDictionary<string, string> _offsetEnumValues = ConfigurationItemRuleTypeBuilder._offsetTypeEnumValues;

	private readonly List<RecurringTimeProperty> _dailyProperties = new List<RecurringTimeProperty>
	{
		new RecurringTimeProperty(Constants.Properties.Frequency.Daily.Every, ConfigurationApiResources.PropertyRuleFrequencyDailyEvery, "PropertyRuleFrequencyDailyEvery", CreateIntProperty)
	};

	private readonly List<RecurringTimeProperty> _weeklyProperties = new List<RecurringTimeProperty>
	{
		new RecurringTimeProperty(Constants.Properties.Frequency.Weekly.Every, ConfigurationApiResources.PropertyRuleFrequencyWeeklyEvery, "PropertyRuleFrequencyWeeklyEvery", CreateIntProperty),
		new RecurringTimeProperty(Constants.Properties.Frequency.Weekly.Days, ConfigurationApiResources.PropertyRuleFrequencyWeeklyDays, "PropertyRuleFrequencyWeeklyDays", CreateListProperty, "", _daysOfWeekEnumValues)
	};

	private readonly List<RecurringTimeProperty> _monthlyProperties = new List<RecurringTimeProperty>
	{
		new RecurringTimeProperty(Constants.Properties.Frequency.Monthly.Every, ConfigurationApiResources.PropertyRuleFrequencyMonthlyEvery, "PropertyRuleFrequencyMonthlyEvery", CreateIntProperty),
		new RecurringTimeProperty(Constants.Properties.Frequency.Monthly.Day, ConfigurationApiResources.PropertyRuleFrequencyMonthlyDay, "PropertyRuleFrequencyMonthlyDay", CreateIntProperty)
	};

	private readonly List<RecurringTimeProperty> _monthlyRelativeProperties = new List<RecurringTimeProperty>
	{
		new RecurringTimeProperty(Constants.Properties.Frequency.MonthlyRelative.Every, ConfigurationApiResources.PropertyRuleFrequencyMonthlyRelativeEvery, "PropertyRuleFrequencyMonthlyRelativeEvery", CreateIntProperty),
		new RecurringTimeProperty(Constants.Properties.Frequency.MonthlyRelative.Day, ConfigurationApiResources.PropertyRuleFrequencyMonthlyRelativeDay, "PropertyRuleFrequencyMonthlyRelativeDay", CreateEnumProperty, "", _dayEnumValues),
		new RecurringTimeProperty(Constants.Properties.Frequency.MonthlyRelative.Offset, ConfigurationApiResources.PropertyRuleFrequencyMonthlyRelativeOffset, "PropertyRuleFrequencyMonthlyRelativeOffset", CreateEnumProperty, "", _offsetEnumValues)
	};

	private readonly List<RecurringTimeProperty> _onceProperties = new List<RecurringTimeProperty>
	{
		new RecurringTimeProperty(Constants.Properties.DailyFrequency.Once.StartTime, ConfigurationApiResources.PropertyRuleDailyFrequencyStartTime, "PropertyRuleDailyFrequencyStartTime", CreateTimeProperty, "00:00")
	};

	private readonly List<RecurringTimeProperty> _recurringProperties = new List<RecurringTimeProperty>
	{
		new RecurringTimeProperty(Constants.Properties.DailyFrequency.Recurring.Every, ConfigurationApiResources.PropertyRuleDailyFrequencyRecurringEvery, "PropertyRuleDailyFrequencyRecurringEvery", CreateIntProperty),
		new RecurringTimeProperty(Constants.Properties.DailyFrequency.Recurring.StartTime, ConfigurationApiResources.PropertyRuleDailyFrequencyStartTime, "PropertyRuleDailyFrequencyStartTime", CreateTimeProperty, "00:00"),
		new RecurringTimeProperty(Constants.Properties.DailyFrequency.Recurring.EndTime, ConfigurationApiResources.PropertyRuleDailyFrequencyEndTime, "PropertyRuleDailyFrequencyEndTime", CreateTimeProperty, "23:59")
	};

	public RecurringTimeSubstep()
	{
		_frequencyProperties = new Dictionary<FrequencyType, List<RecurringTimeProperty>>
		{
			{
				FrequencyType.Daily,
				_dailyProperties
			},
			{
				FrequencyType.Weekly,
				_weeklyProperties
			},
			{
				FrequencyType.Monthly,
				_monthlyProperties
			},
			{
				FrequencyType.MonthlyRelative,
				_monthlyRelativeProperties
			}
		};
		_subDayProperties = new Dictionary<ScheduleTypes.SubdayType, List<RecurringTimeProperty>>
		{
			{
				ScheduleTypes.SubdayType.Once,
				_onceProperties
			},
			{
				ScheduleTypes.SubdayType.Recurring,
				_recurringProperties
			}
		};
		_frequencyTypes = (from FrequencyType v in Enum.GetValues(typeof(FrequencyType))
			where v != FrequencyType.None
			select v).ToDictionary((FrequencyType t) => t.ToString(), (FrequencyType t) => ConfigurationApiResources.ResourceManager.GetString("FrequencyTypeEnum" + t));
		_dailyFrequencyTypes = (from ScheduleTypes.SubdayType v in Enum.GetValues(typeof(ScheduleTypes.SubdayType))
			where v != ScheduleTypes.SubdayType.Undefined
			select v).ToDictionary((ScheduleTypes.SubdayType t) => t.ToString(), (ScheduleTypes.SubdayType t) => ConfigurationApiResources.ResourceManager.GetString("SubDayTypeEnum" + t));
	}

	public bool IsComplete(IGeminiRequestContext requestContext, ConfigurationItem item)
	{
		if (!item.TryGetPropertyValue("Schedule.Frequency", out FrequencyType result) || !item.TryGetPropertyValue("Schedule.DailyFrequency", out ScheduleTypes.SubdayType result2))
		{
			return false;
		}
		foreach (RecurringTimeProperty item2 in _frequencyProperties[result])
		{
			if (!item.TryGetPropertyValue(item2.Name, out var _))
			{
				return false;
			}
		}
		foreach (RecurringTimeProperty item3 in _subDayProperties[result2])
		{
			if (!item.TryGetPropertyValue(item3.Name, out var _))
			{
				return false;
			}
		}
		return true;
	}

	public List<Property> GetCurrentProperties(IGeminiRequestContext requestContext, ConfigurationItem item, out string lastPropertyKey)
	{
		List<Property> list = new List<Property>();
		lastPropertyKey = null;
		if (item.TryGetPropertyValue("Schedule.Frequency", out FrequencyType result) && result != FrequencyType.None)
		{
			list.Add(CreateEnumProperty("Schedule.Frequency", result.ToString(), ConfigurationApiResources.PropertyRuleFrequency, "PropertyRuleFrequency", _frequencyTypes));
			lastPropertyKey = "Schedule.Frequency";
			foreach (RecurringTimeProperty item2 in _frequencyProperties[result])
			{
				if (!item.TryGetPropertyValue(item2.Name, out var result2))
				{
					return list;
				}
				list.Add(item2.Function(item2.Name, result2, item2.DisplayName, item2.TranslationId, item2.Members));
				lastPropertyKey = item2.Name;
			}
		}
		if (item.TryGetPropertyValue("Schedule.DailyFrequency", out ScheduleTypes.SubdayType result3) && result3 != ScheduleTypes.SubdayType.Undefined)
		{
			list.Add(CreateEnumProperty("Schedule.DailyFrequency", result3.ToString(), ConfigurationApiResources.PropertyRuleDailyFrequency, "PropertyRuleDailyFrequency", _dailyFrequencyTypes));
			lastPropertyKey = "Schedule.DailyFrequency";
			foreach (RecurringTimeProperty item3 in _subDayProperties[result3])
			{
				if (!item.TryGetPropertyValue(item3.Name, out var result4))
				{
					return list;
				}
				list.Add(item3.Function(item3.Name, result4, item3.DisplayName, item3.TranslationId, item3.Members));
				lastPropertyKey = item3.Name;
			}
		}
		return list;
	}

	public List<Property> GetNextProperties(IGeminiRequestContext requestContext, ConfigurationItem item, string lastPropertyKey)
	{
		List<Property> list = new List<Property>();
		if (lastPropertyKey == "StartRuleType")
		{
			list.Add(CreateEnumProperty("Schedule.Frequency", "", ConfigurationApiResources.PropertyRuleFrequency, "PropertyRuleFrequency", _frequencyTypes));
			return list;
		}
		ScheduleTypes.SubdayType result2;
		if (item.TryGetPropertyValue("Schedule.Frequency", out FrequencyType result) && result != FrequencyType.None && IsLastPropertyKeyWithinType(lastPropertyKey, "Schedule.Frequency", _frequencyProperties[result]))
		{
			list.AddRange(GetNextProperties(_frequencyProperties[result], lastPropertyKey));
			list.Add(CreateEnumProperty("Schedule.DailyFrequency", "", ConfigurationApiResources.PropertyRuleDailyFrequency, "PropertyRuleDailyFrequency", _dailyFrequencyTypes));
		}
		else if (item.TryGetPropertyValue("Schedule.DailyFrequency", out result2) && result2 != ScheduleTypes.SubdayType.Undefined && IsLastPropertyKeyWithinType(lastPropertyKey, "Schedule.DailyFrequency", _subDayProperties[result2]))
		{
			list.AddRange(GetNextProperties(_subDayProperties[result2], lastPropertyKey));
		}
		return list;
	}

	private static bool IsLastPropertyKeyWithinType(string lastPropertyKey, string typeProperty, List<RecurringTimeProperty> typeProperties)
	{
		if (!(lastPropertyKey == typeProperty))
		{
			return typeProperties.Any((RecurringTimeProperty f) => f.Name == lastPropertyKey);
		}
		return true;
	}

	private static List<Property> GetNextProperties(List<RecurringTimeProperty> typeProperties, string lastPropertyKey)
	{
		List<Property> list = new List<Property>();
		int num = typeProperties.FindIndex((RecurringTimeProperty f) => f.Name == lastPropertyKey) + 1;
		foreach (RecurringTimeProperty item in typeProperties.GetRange(num, typeProperties.Count - num))
		{
			list.Add(item.Function(item.Name, item.DefaultValue, item.DisplayName, item.TranslationId, item.Members));
		}
		return list;
	}

	private static Property CreateEnumProperty(string propertyName, string value, string displayName, string translationId, IDictionary<string, string> members)
	{
		return GeneralPropertyDefinitions.GetEnumProperty(propertyName, value, displayName, translationId, string.Empty, members);
	}

	private static Property CreateIntProperty(string propertyName, string value, string displayName, string translationId, IDictionary<string, string> members)
	{
		return GeneralPropertyDefinitions.GetIntProperty(propertyName, displayName, value, translationId, isSettable: true);
	}

	private static Property CreateTimeProperty(string propertyName, string value, string displayName, string translationId, IDictionary<string, string> members)
	{
		return GeneralPropertyDefinitions.GetTimeProperty(propertyName, displayName, value, translationId, isSettable: true);
	}

	private static Property CreateListProperty(string propertyName, string values, string displayName, string translationId, IDictionary<string, string> members)
	{
		return GeneralPropertyDefinitions.GetListProperty(values, propertyName, displayName, translationId, isSettable: true, members);
	}
}
