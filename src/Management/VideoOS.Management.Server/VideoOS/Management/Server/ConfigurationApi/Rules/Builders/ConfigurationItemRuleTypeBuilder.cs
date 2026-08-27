using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;
using VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;
using VideoOS.Management.Server.Properties;
using VideoOS.Rules.Interfaces;
using VideoOS.Utilities.Validation;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Builders;

public class ConfigurationItemRuleTypeBuilder : IConfigurationPropertiesBuilder<RulePartModel>
{
	private class RecurringTimeProperty
	{
		public string DisplayName { get; }

		public string TranslationId { get; }

		public Action<RulePartModel, List<Property>> ActionName { get; }

		public RecurringTimeProperty(Action<RulePartModel, List<Property>> actionName, string displayName, string translationId)
		{
			DisplayName = displayName;
			ActionName = actionName;
			TranslationId = translationId;
		}
	}

	private static readonly IDictionary<string, string> _startRuleTypeEnumValues = new Dictionary<string, string>
	{
		{
			"TimeInterval",
			ConfigurationApiResources.RuleTypeInterval
		},
		{
			"Event",
			ConfigurationApiResources.RuleTypeEvent
		},
		{
			"RecurringTime",
			ConfigurationApiResources.RuleTypeRecurringTime
		}
	};

	private readonly IDictionary<string, string> _stopRuleTypeEnumValues = new Dictionary<string, string>
	{
		{
			"TimeInterval",
			ConfigurationApiResources.RuleTypeInterval
		},
		{
			"Event",
			ConfigurationApiResources.RuleTypeEvent
		},
		{
			"Timer",
			ConfigurationApiResources.RuleTypeTimer
		},
		{ "None", "None" }
	};

	private static readonly IDictionary<FrequencyType, RecurringTimeProperty> _frequencyTypeEnumValues = new Dictionary<FrequencyType, RecurringTimeProperty>
	{
		{
			FrequencyType.Daily,
			new RecurringTimeProperty(GetDailyProperties, ConfigurationApiResources.FrequencyTypeEnumDaily, "Daily")
		},
		{
			FrequencyType.Weekly,
			new RecurringTimeProperty(GetWeeklyProperties, ConfigurationApiResources.FrequencyTypeEnumWeekly, "Weekly")
		},
		{
			FrequencyType.Monthly,
			new RecurringTimeProperty(GetMonthlyAbsoluteProperties, ConfigurationApiResources.FrequencyTypeEnumMonthly, "Monthly")
		},
		{
			FrequencyType.MonthlyRelative,
			new RecurringTimeProperty(GetMonthlyRelativeProperties, ConfigurationApiResources.FrequencyTypeEnumMonthlyRelative, "MonthlyRelative")
		}
	};

	public static readonly IDictionary<string, string> _offsetTypeEnumValues = new Dictionary<string, string>
	{
		{
			"First",
			ConfigurationApiResources.OffsetTypeEnumFirst
		},
		{
			"Second",
			ConfigurationApiResources.OffsetTypeEnumSecond
		},
		{
			"Third",
			ConfigurationApiResources.OffsetTypeEnumThird
		},
		{
			"Fourth",
			ConfigurationApiResources.OffsetTypeEnumFourth
		},
		{
			"Last",
			ConfigurationApiResources.OffsetTypeEnumLast
		}
	};

	public static readonly IDictionary<string, string> _relativeDayTypeEnumValues = new Dictionary<string, string>
	{
		{
			"Sunday",
			ConfigurationApiResources.PropertyDayTypeEnumSunday
		},
		{
			"Monday",
			ConfigurationApiResources.PropertyDayTypeEnumMonday
		},
		{
			"Tuesday",
			ConfigurationApiResources.PropertyDayTypeEnumTuesday
		},
		{
			"Wednesday",
			ConfigurationApiResources.PropertyDayTypeEnumWednesday
		},
		{
			"Thursday",
			ConfigurationApiResources.PropertyDayTypeEnumThursday
		},
		{
			"Friday",
			ConfigurationApiResources.PropertyDayTypeEnumFriday
		},
		{
			"Saturday",
			ConfigurationApiResources.PropertyDayTypeEnumSaturday
		},
		{
			"Day",
			ConfigurationApiResources.RelativeDayTypeEnumDay
		},
		{
			"WeekDay",
			ConfigurationApiResources.RelativeDayTypeEnumWeekday
		},
		{
			"WeekendDay",
			ConfigurationApiResources.RelativeDayTypeEnumWeekendDay
		}
	};

	private static readonly IDictionary<ScheduleTypes.SubdayType, RecurringTimeProperty> _subDayTypeEnumValues = new Dictionary<ScheduleTypes.SubdayType, RecurringTimeProperty>
	{
		{
			ScheduleTypes.SubdayType.Once,
			new RecurringTimeProperty(GetDailyOnceProperty, ConfigurationApiResources.SubDayTypeEnumOnce, "Once")
		},
		{
			ScheduleTypes.SubdayType.Recurring,
			new RecurringTimeProperty(GetDailyRecurringProperties, ConfigurationApiResources.SubDayTypeEnumRecurring, "Recurring")
		}
	};

	private readonly IEventTypeProxy _eventTypeProxy;

	private readonly EventTypeProvider _eventTypeProvider;

	private bool _isStart;

	public ConfigurationItemRuleTypeBuilder(IEventTypeProxy eventTypeProxy)
	{
		_eventTypeProxy = eventTypeProxy;
		_eventTypeProvider = new EventTypeProvider(eventTypeProxy);
	}

	public IEnumerable<Property> Build(IGeminiRequestContext requestContext, RulePartModel rulePart)
	{
		rulePart.NullCheck("", "Build", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\Rules\\Builders\\ConfigurationItem\\ConfigurationItemRuleTypeBuilder.cs", 78);
		_isStart = rulePart.IsStart;
		List<Property> list = new List<Property>();
		GetRuleTypeProperties(rulePart, list);
		if (rulePart.TriggerType == TriggerType.Event)
		{
			list.AddRange(BuildEventProperties(requestContext, rulePart));
		}
		if (!_isStart && rulePart.TriggerType == TriggerType.Timer)
		{
			GetTimerProperty(rulePart, list);
		}
		if (rulePart.IsStart && rulePart.TriggerType == TriggerType.RecurringTime)
		{
			FrequencyType key = rulePart.RecurringTime?.Frequency.FrequencyType ?? FrequencyType.None;
			GetFrequencyTypeProperties(rulePart, list);
			if (_frequencyTypeEnumValues.ContainsKey(key))
			{
				_frequencyTypeEnumValues[rulePart.RecurringTime.Frequency.FrequencyType].ActionName(rulePart, list);
			}
			ScheduleTypes.SubdayType key2 = rulePart.RecurringTime?.SubDay.SubDayType ?? ScheduleTypes.SubdayType.Undefined;
			GetSubDayTypeProperties(rulePart, list);
			if (_subDayTypeEnumValues.ContainsKey(key2))
			{
				_subDayTypeEnumValues[rulePart.RecurringTime.SubDay.SubDayType].ActionName(rulePart, list);
			}
		}
		return list.ToArray();
	}

	private static void GetTimerProperty(RulePartModel rulePart, List<Property> result)
	{
		result.Add(GeneralPropertyDefinitions.GetIntProperty("TimerTime", ConfigurationApiResources.PropertyRuleTimerTime, rulePart.Delay.ToString(CultureInfo.InvariantCulture), "PropertyRuleTimerTime", isSettable: true));
	}

	private void GetRuleTypeProperties(RulePartModel rulePart, List<Property> result)
	{
		result.Add(CreateRuleTypeProperty(_isStart ? "StartRuleType" : "StopRuleType", rulePart.TriggerType, _isStart ? ConfigurationApiResources.PropertyRuleStartRuleType : ConfigurationApiResources.PropertyRuleStopRuleType, _isStart ? "PropertyRuleStartRuleType" : "PropertyRuleStopRuleType"));
	}

	public static void GetRecurringTimeProperties(List<Property> result, string propertyName, string value, string displayName, string translationId, IDictionary<string, string> members)
	{
		result.Add(GeneralPropertyDefinitions.GetEnumProperty(propertyName, value, displayName, translationId, string.Empty, members, serverValidation: true));
	}

	public static void GetFrequencyTypeProperties(RulePartModel rulePart, List<Property> result)
	{
		FrequencyType frequencyType = rulePart.RecurringTime?.Frequency.FrequencyType ?? FrequencyType.None;
		GetRecurringTimeProperties(result, "Schedule.Frequency", (frequencyType != FrequencyType.None) ? frequencyType.ToString() : "", ConfigurationApiResources.PropertyRuleFrequency, "PropertyRuleFrequency", _frequencyTypeEnumValues.ToDictionary((KeyValuePair<FrequencyType, RecurringTimeProperty> x) => x.Value.TranslationId, (KeyValuePair<FrequencyType, RecurringTimeProperty> x) => x.Value.DisplayName));
	}

	public static void GetSubDayTypeProperties(RulePartModel rulePart, List<Property> result)
	{
		ScheduleTypes.SubdayType subdayType = rulePart.RecurringTime?.SubDay.SubDayType ?? ScheduleTypes.SubdayType.Undefined;
		GetRecurringTimeProperties(result, "Schedule.DailyFrequency", (subdayType != ScheduleTypes.SubdayType.Undefined) ? subdayType.ToString() : "", ConfigurationApiResources.PropertyRuleDailyFrequency, "PropertyRuleDailyFrequency", _subDayTypeEnumValues.ToDictionary((KeyValuePair<ScheduleTypes.SubdayType, RecurringTimeProperty> x) => x.Value.TranslationId, (KeyValuePair<ScheduleTypes.SubdayType, RecurringTimeProperty> x) => x.Value.DisplayName));
	}

	public static void GetDailyProperties(RulePartModel rulePart, List<Property> result)
	{
		string value = rulePart.RecurringTime.Frequency.Frequency.ToString(CultureInfo.InvariantCulture);
		result.Add(GeneralPropertyDefinitions.GetIntProperty(Constants.Properties.Frequency.Daily.Every, ConfigurationApiResources.PropertyRuleFrequencyDailyEvery, value, "PropertyRuleFrequencyDailyEvery", isSettable: true));
	}

	public static void GetWeeklyProperties(RulePartModel rulePart, List<Property> result)
	{
		string value = rulePart.RecurringTime.Frequency.Frequency.ToString(CultureInfo.InvariantCulture);
		result.Add(GeneralPropertyDefinitions.GetIntProperty(Constants.Properties.Frequency.Weekly.Every, ConfigurationApiResources.PropertyRuleFrequencyWeeklyEvery, value, "PropertyRuleFrequencyWeeklyEvery", isSettable: true));
		Dictionary<string, RuleParameter> parameters = rulePart.RecurringTime.Frequency.Parameters;
		string[] array = (parameters.ContainsKey("DayOfWeekIsOneOf") ? parameters["DayOfWeekIsOneOf"].Values : null);
		string values = ((array != null) ? string.Join(";", array) : string.Empty);
		result.Add(ConfigurationItemConditionsHelper.GetDaysOfWeekConditionProperty(Constants.Properties.Frequency.Weekly.Days, ConfigurationApiResources.PropertyRuleFrequencyWeeklyDays, "PropertyRuleFrequencyWeeklyDays", values));
	}

	public static void GetMonthlyAbsoluteProperties(RulePartModel rulePart, List<Property> result)
	{
		Dictionary<string, RuleParameter> parameters = rulePart.RecurringTime.Frequency.Parameters;
		string value = (parameters.ContainsKey("AbsoluteDay") ? parameters["AbsoluteDay"].Value : "0");
		result.Add(GeneralPropertyDefinitions.GetIntProperty(Constants.Properties.Frequency.Monthly.Day, ConfigurationApiResources.PropertyRuleFrequencyMonthlyDay, value, "PropertyRuleFrequencyMonthlyDay", isSettable: true));
		string value2 = rulePart.RecurringTime.Frequency.Frequency.ToString(CultureInfo.InvariantCulture);
		result.Add(GeneralPropertyDefinitions.GetIntProperty(Constants.Properties.Frequency.Monthly.Every, ConfigurationApiResources.PropertyRuleFrequencyMonthlyEvery, value2, "PropertyRuleFrequencyMonthlyEvery", isSettable: true));
	}

	public static void GetMonthlyRelativeProperties(RulePartModel rulePart, List<Property> result)
	{
		Dictionary<string, RuleParameter> parameters = rulePart.RecurringTime.Frequency.Parameters;
		string text = (parameters.ContainsKey("Offset") ? parameters["Offset"].Value : "");
		string value = (string.IsNullOrEmpty(text) ? text : Enum.Parse(typeof(ScheduleTypes.MonthlyRelativeOffset), text, ignoreCase: true).ToString());
		string text2 = (parameters.ContainsKey("RelativeDay") ? parameters["RelativeDay"].Value : "");
		string value2 = (string.IsNullOrEmpty(text2) ? text2 : Enum.Parse(typeof(ScheduleTypes.MonthlyRelativeDay), text2, ignoreCase: true).ToString());
		result.Add(GeneralPropertyDefinitions.GetEnumProperty(Constants.Properties.Frequency.MonthlyRelative.Offset, value, ConfigurationApiResources.PropertyRuleFrequencyMonthlyRelativeOffset, "PropertyRuleFrequencyMonthlyRelativeOffset", string.Empty, _offsetTypeEnumValues, serverValidation: true));
		result.Add(GeneralPropertyDefinitions.GetEnumProperty(Constants.Properties.Frequency.MonthlyRelative.Day, value2, ConfigurationApiResources.PropertyRuleFrequencyMonthlyRelativeDay, "PropertyRuleFrequencyMonthlyRelativeDay", string.Empty, _relativeDayTypeEnumValues, serverValidation: true));
		string value3 = rulePart.RecurringTime.Frequency.Frequency.ToString(CultureInfo.InvariantCulture);
		result.Add(GeneralPropertyDefinitions.GetIntProperty(Constants.Properties.Frequency.MonthlyRelative.Every, ConfigurationApiResources.PropertyRuleFrequencyMonthlyRelativeEvery, value3, "PropertyRuleFrequencyMonthlyRelativeEvery", isSettable: true));
	}

	public static void GetDailyOnceProperty(RulePartModel rulePart, List<Property> result)
	{
		TimeSpan timeSpan = TimeSpan.FromSeconds(rulePart.RecurringTime.SubDay.StartTimeOfDay);
		result.Add(GeneralPropertyDefinitions.GetTimeProperty(Constants.Properties.DailyFrequency.Once.StartTime, ConfigurationApiResources.PropertyRuleDailyFrequencyStartTime, timeSpan.ToString("hh\\:mm\\:ss", CultureInfo.InvariantCulture), "PropertyRuleDailyFrequencyStartTime", isSettable: true));
	}

	public static void GetDailyRecurringProperties(RulePartModel rulePart, List<Property> result)
	{
		TimeSpan timeSpan = TimeSpan.FromSeconds(rulePart.RecurringTime.SubDay.StartTimeOfDay);
		TimeSpan timeSpan2 = TimeSpan.FromSeconds(rulePart.RecurringTime.SubDay.EndTimeOfDay);
		double totalHours = TimeSpan.FromSeconds(rulePart.RecurringTime.SubDay.Frequency).TotalHours;
		result.Add(GeneralPropertyDefinitions.GetIntProperty(Constants.Properties.DailyFrequency.Recurring.Every, ConfigurationApiResources.PropertyRuleDailyFrequencyRecurringEvery, totalHours.ToString(CultureInfo.InvariantCulture), "PropertyRuleDailyFrequencyRecurringEvery", isSettable: true));
		result.Add(GeneralPropertyDefinitions.GetTimeProperty(Constants.Properties.DailyFrequency.Recurring.StartTime, ConfigurationApiResources.PropertyRuleDailyFrequencyStartTime, timeSpan.ToString("hh\\:mm\\:ss", CultureInfo.InvariantCulture), "PropertyRuleDailyFrequencyStartTime", isSettable: true));
		result.Add(GeneralPropertyDefinitions.GetTimeProperty(Constants.Properties.DailyFrequency.Recurring.EndTime, ConfigurationApiResources.PropertyRuleDailyFrequencyEndTime, timeSpan2.ToString("hh\\:mm\\:ss", CultureInfo.InvariantCulture), "PropertyRuleDailyFrequencyEndTime", isSettable: true));
	}

	private Property CreateRuleTypeProperty(string propertyName, TriggerType ruleType, string displayName, string translationId)
	{
		return GeneralPropertyDefinitions.GetEnumProperty(propertyName, ruleType.ToString(), displayName, translationId, string.Empty, _isStart ? _startRuleTypeEnumValues : _stopRuleTypeEnumValues, serverValidation: true);
	}

	private IEnumerable<Property> BuildEventProperties(IGeminiRequestContext requestContext, RulePartModel rulePart)
	{
		Dictionary<string, string> eventGroups = _eventTypeProxy.GetEventGroups(requestContext);
		return new List<Property>
		{
			GeneralPropertyDefinitions.GetEnumProperty(_isStart ? "StartEventGroup" : "StopEventGroup", GetEventGroupName(rulePart.EventModel), _isStart ? ConfigurationApiResources.PropertyRuleStartEventGroup : ConfigurationApiResources.PropertyRuleStopEventGroup, _isStart ? "PropertyRuleStartEventGroup" : "PropertyRuleStopEventGroup", string.Empty, eventGroups, serverValidation: true),
			GeneralPropertyDefinitions.GetEnumProperty(_isStart ? "StartEventType" : "StopEventType", rulePart.EventModel?.EventType.ToString() ?? "", _isStart ? ConfigurationApiResources.PropertyRuleStartEventType : ConfigurationApiResources.PropertyRuleStopEventType, _isStart ? "PropertyRuleStartEventType" : "PropertyRuleStopEventType", string.Empty, _eventTypeProvider.GetEventTypes(requestContext, rulePart.EventModel?.EventGroupType ?? EventGroupType.None, rulePart.EventModel?.PluginEventGroupId ?? Guid.Empty)),
			GeneralPropertyDefinitions.GetPathListProperty(_isStart ? "StartEventSources" : "StopEventSources", GetRuleSourcePaths(rulePart.EventModel?.EventSources), _isStart ? ConfigurationApiResources.PropertyRuleStartEventSources : ConfigurationApiResources.PropertyRuleStopEventSources, _isStart ? "PropertyRuleStartEventSources" : "PropertyRuleStopEventSources", _eventTypeProxy.GetEventSourceItemTypes(requestContext, rulePart.EventModel?.EventType ?? Guid.Empty))
		};
	}

	private static string GetRuleSourcePaths(IEnumerable<EventSource> eventSources)
	{
		if (eventSources == null)
		{
			return string.Empty;
		}
		return string.Join(";", eventSources.Select((EventSource s) => ((s.ItemType == "System") ? ConfigurationItemPath.CreateSystemPath() : ConfigurationItemPath.CreateItemPath(s.ItemType, s.Id.ToString())).Text));
	}

	private static string GetEventGroupName(EventModel eventModel)
	{
		if (eventModel == null)
		{
			return string.Empty;
		}
		if (eventModel.EventGroupType == EventGroupType.PluginEvent || eventModel.EventGroupType == EventGroupType.AnalyticsEvent)
		{
			return eventModel.PluginEventGroupId.ToString();
		}
		return eventModel.EventGroupType.ToString();
	}
}
