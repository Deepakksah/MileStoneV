using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Builders;

public class RuleModelConditionsBuilder : IRuleModelConditionsBuilder
{
	public IDictionary<ConditionType, ConditionModel> Build(ConfigurationItem item)
	{
		Dictionary<ConditionType, ConditionModel> dictionary = new Dictionary<ConditionType, ConditionModel>();
		if (item.IsPropertySelected("Always"))
		{
			dictionary[ConditionType.Always] = GetEmptyParametersCondition(ConditionType.Always);
		}
		if (item.IsPropertySelected("FailoverActive"))
		{
			dictionary[ConditionType.FailoverActive] = GetEmptyParametersCondition(ConditionType.FailoverActive);
		}
		if (item.IsPropertySelected("FailoverInactive"))
		{
			dictionary[ConditionType.FailoverInactive] = GetEmptyParametersCondition(ConditionType.FailoverInactive);
		}
		if (item.IsPropertySelected("WithinTimeProfile"))
		{
			dictionary[ConditionType.WithinTimeProfile] = GetTimeProfileCondition(item, withinTimeProfile: true);
		}
		if (item.IsPropertySelected("OutsideTimeProfile"))
		{
			dictionary[ConditionType.OutsideTimeProfile] = GetTimeProfileCondition(item, withinTimeProfile: false);
		}
		if (item.IsPropertySelected("TimeOfDayBetween"))
		{
			dictionary[ConditionType.TimeBetween] = GetTimeBetweenCondition(item);
		}
		if (item.IsPropertySelected("DaysOfWeek"))
		{
			dictionary[ConditionType.DaysOfWeek] = GetDaysOfWeekCondition(item);
		}
		if (item.IsPropertySelected("WithinWindow"))
		{
			dictionary[ConditionType.WithinWindow] = GetWithinWindowCondition(item);
		}
		return dictionary;
	}

	private static ConditionModel GetTimeBetweenCondition(ConfigurationItem item)
	{
		string timeBetweenTimePropertyKey = GetTimeBetweenTimePropertyKey("StartTime");
		string timeBetweenTimePropertyKey2 = GetTimeBetweenTimePropertyKey("EndTime");
		ValidateData("TimeOfDayBetween", new string[2] { timeBetweenTimePropertyKey, timeBetweenTimePropertyKey2 }, item);
		TimeSpan propertyValue = item.GetPropertyValue<TimeSpan>(timeBetweenTimePropertyKey);
		TimeSpan propertyValue2 = item.GetPropertyValue<TimeSpan>(timeBetweenTimePropertyKey2);
		return new ConditionModel
		{
			ConditionType = ConditionType.TimeBetween,
			Parameters = new Dictionary<string, RuleParameter>
			{
				{
					"StartTime",
					new RuleParameter
					{
						Name = "StartTime",
						Value = propertyValue.ToString("hh\\:mm\\:ss", CultureInfo.InvariantCulture)
					}
				},
				{
					"EndTime",
					new RuleParameter
					{
						Name = "EndTime",
						Value = propertyValue2.ToString("hh\\:mm\\:ss", CultureInfo.InvariantCulture)
					}
				}
			}
		};
	}

	private static ConditionModel GetDaysOfWeekCondition(ConfigurationItem item)
	{
		string daysOfWeekDaysPropertyKey = GetDaysOfWeekDaysPropertyKey();
		ValidateData("DaysOfWeek", new string[1] { daysOfWeekDaysPropertyKey }, item);
		string propertyValue = item.GetPropertyValue(daysOfWeekDaysPropertyKey);
		ConditionModel conditionModel = new ConditionModel();
		conditionModel.ConditionType = ConditionType.DaysOfWeek;
		conditionModel.Parameters = new Dictionary<string, RuleParameter> { 
		{
			"DayOfWeekIsOneOf",
			new RuleParameter
			{
				Name = "DayOfWeekIsOneOf",
				Values = propertyValue.Split(';')
			}
		} };
		return conditionModel;
	}

	private static ConditionModel GetWithinWindowCondition(ConfigurationItem item)
	{
		string withinWindowTextPropertyKey = GetWithinWindowTextPropertyKey();
		ValidateData("WithinWindow", new string[1] { withinWindowTextPropertyKey }, item);
		string propertyValue = item.GetPropertyValue(withinWindowTextPropertyKey);
		return new ConditionModel
		{
			ConditionType = ConditionType.WithinWindow,
			Parameters = new Dictionary<string, RuleParameter> { 
			{
				"EventIsWithinWindow",
				new RuleParameter
				{
					Name = "EventIsWithinWindow",
					Value = propertyValue
				}
			} }
		};
	}

	private static ConditionModel GetEmptyParametersCondition(ConditionType conditionType)
	{
		return new ConditionModel
		{
			ConditionType = conditionType,
			Parameters = new Dictionary<string, RuleParameter>()
		};
	}

	private static ConditionModel GetTimeProfileCondition(ConfigurationItem item, bool withinTimeProfile)
	{
		string timeProfilePropertyKey = GetTimeProfilePropertyKey(withinTimeProfile);
		string timeProfileIdPropertyKey = GetTimeProfileIdPropertyKey(timeProfilePropertyKey);
		ValidateData(timeProfilePropertyKey, new string[1] { timeProfileIdPropertyKey }, item);
		if (!item.TryGetProperty(timeProfileIdPropertyKey, out var result))
		{
			throw new ArgumentException("Could not read property {0}", timeProfileIdPropertyKey);
		}
		result = result ?? string.Empty;
		return new ConditionModel
		{
			ConditionType = (withinTimeProfile ? ConditionType.WithinTimeProfile : ConditionType.OutsideTimeProfile),
			Parameters = new Dictionary<string, RuleParameter> { 
			{
				GetTimeProfileParameterName(withinTimeProfile),
				new RuleParameter
				{
					Name = GetTimeProfileParameterName(withinTimeProfile),
					Value = ConfigurationItemPath.CreatePath(result).Id
				}
			} }
		};
	}

	private static string GetTimeProfilePropertyKey(bool withinTimeProfile)
	{
		if (!withinTimeProfile)
		{
			return "OutsideTimeProfile";
		}
		return "WithinTimeProfile";
	}

	private static string GetTimeProfileParameterName(bool withinTimeProfile)
	{
		if (!withinTimeProfile)
		{
			return "OutsideTimeProfile";
		}
		return "WithinTimeProfile";
	}

	private static string GetTimeProfileIdPropertyKey(string timeProfilePropertyKey)
	{
		return timeProfilePropertyKey + ".TimeProfile";
	}

	private static string GetTimeBetweenTimePropertyKey(string postfix)
	{
		return "TimeOfDayBetween." + postfix;
	}

	private static string GetDaysOfWeekDaysPropertyKey()
	{
		return "DaysOfWeek.Days";
	}

	private static string GetWithinWindowTextPropertyKey()
	{
		return "WithinWindow.Window";
	}

	private static void ValidateData(string tickPropertyKey, string[] valuePropertyKeys, ConfigurationItem item)
	{
		if (valuePropertyKeys.Select(item.IsPropertyValuePresent).All((bool v) => v))
		{
			return;
		}
		if (valuePropertyKeys.Length > 1)
		{
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Value for both {0} and {1} must be set", valuePropertyKeys[0], valuePropertyKeys[1]));
		}
		throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Value for {0} must be set if {1} is selected", valuePropertyKeys[0], tickPropertyKey));
	}
}
