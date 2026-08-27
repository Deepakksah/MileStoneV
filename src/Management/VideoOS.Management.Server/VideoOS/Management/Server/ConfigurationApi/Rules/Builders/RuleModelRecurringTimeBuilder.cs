using System;
using System.Collections.Generic;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Builders;

public static class RuleModelRecurringTimeBuilder
{
	private static readonly Dictionary<string, string> _typeNameToPropertyPrefix = new Dictionary<string, string>
	{
		{
			FrequencyType.Daily.ToString(),
			Constants.Properties.Frequency.Daily.Prefix
		},
		{
			FrequencyType.Weekly.ToString(),
			Constants.Properties.Frequency.Weekly.Prefix
		},
		{
			FrequencyType.Monthly.ToString(),
			Constants.Properties.Frequency.Monthly.Prefix
		},
		{
			FrequencyType.MonthlyRelative.ToString(),
			Constants.Properties.Frequency.MonthlyRelative.Prefix
		},
		{
			ScheduleTypes.SubdayType.Once.ToString(),
			Constants.Properties.DailyFrequency.Once.Prefix
		},
		{
			ScheduleTypes.SubdayType.Recurring.ToString(),
			Constants.Properties.DailyFrequency.Recurring.Prefix
		}
	};

	public static RecurringTimeModel Build(ConfigurationItem item)
	{
		RecurringTimeModel recurringTimeModel = new RecurringTimeModel();
		if (item.TryGetPropertyValue("Schedule.Frequency", out FrequencyType result) && result != FrequencyType.None)
		{
			SetFrequency(item, recurringTimeModel, result);
		}
		if (item.TryGetPropertyValue("Schedule.DailyFrequency", out ScheduleTypes.SubdayType result2) && result2 != ScheduleTypes.SubdayType.Undefined)
		{
			SetSubDay(item, recurringTimeModel, result2);
		}
		return recurringTimeModel;
	}

	private static void SetFrequency(ConfigurationItem item, RecurringTimeModel model, FrequencyType type)
	{
		model.Frequency.FrequencyType = type;
		model.Frequency.Frequency = GetFrequencyValue(item, type.ToString());
		switch (type)
		{
		case FrequencyType.Weekly:
			SetWeeklyParameters(item, model);
			break;
		case FrequencyType.Monthly:
			SetMonthlyParameters(item, model);
			break;
		case FrequencyType.MonthlyRelative:
			SetMonthlyRelativeParameters(item, model);
			break;
		}
	}

	private static void SetWeeklyParameters(ConfigurationItem item, RecurringTimeModel model)
	{
		if (item.TryGetPropertyValue(Constants.Properties.Frequency.Weekly.Days, out var result))
		{
			model.Frequency.Parameters.Add("DayOfWeekIsOneOf", new RuleParameter
			{
				Name = "DayOfWeekIsOneOf",
				Values = result.Split(';')
			});
		}
	}

	private static void SetMonthlyParameters(ConfigurationItem item, RecurringTimeModel model)
	{
		if (item.TryGetPropertyValue(Constants.Properties.Frequency.Monthly.Day, out var result))
		{
			model.Frequency.Parameters.Add("AbsoluteDay", new RuleParameter
			{
				Name = "AbsoluteDay",
				Value = result
			});
		}
	}

	private static void SetMonthlyRelativeParameters(ConfigurationItem item, RecurringTimeModel model)
	{
		if (item.TryGetPropertyValue(Constants.Properties.Frequency.MonthlyRelative.Day, out var result))
		{
			model.Frequency.Parameters.Add("RelativeDay", new RuleParameter
			{
				Name = "RelativeDay",
				Value = result
			});
		}
		if (item.TryGetPropertyValue(Constants.Properties.Frequency.MonthlyRelative.Offset, out var result2))
		{
			model.Frequency.Parameters.Add("Offset", new RuleParameter
			{
				Name = "Offset",
				Value = result2
			});
		}
	}

	private static void SetSubDay(ConfigurationItem item, RecurringTimeModel model, ScheduleTypes.SubdayType type)
	{
		model.SubDay.SubDayType = type;
		model.SubDay.StartTimeOfDay = GetSubDayTimeValue(item, type.ToString(), "StartTime");
		if (type == ScheduleTypes.SubdayType.Recurring)
		{
			model.SubDay.EndTimeOfDay = GetSubDayTimeValue(item, type.ToString(), "EndTime");
			model.SubDay.Frequency = Convert.ToInt32(TimeSpan.FromHours(GetFrequencyValue(item, type.ToString())).TotalSeconds);
		}
	}

	private static int GetFrequencyValue(ConfigurationItem item, string type)
	{
		string propertyKey = GetPropertyKey(type, "Every");
		if (item.TryGetProperty(propertyKey, out int result))
		{
			return result;
		}
		return 0;
	}

	private static int GetSubDayTimeValue(ConfigurationItem item, string type, string postfix)
	{
		string propertyKey = GetPropertyKey(type, postfix);
		if (item.TryGetProperty(propertyKey, out TimeSpan result))
		{
			return Convert.ToInt32(result.TotalSeconds);
		}
		return 0;
	}

	private static string GetPropertyKey(string type, string postfix)
	{
		return _typeNameToPropertyPrefix[type] + "." + postfix;
	}
}
