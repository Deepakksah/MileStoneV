using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Validation;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Builders;

public class ConfigurationItemConditionsBuilder : IConfigurationPropertiesBuilder<RulePartModel>
{
	private class ConditionProperty
	{
		private TriggerType[] _invalidForTriggerTypes { get; }

		public string DisplayName { get; }

		public string TranslationId { get; }

		public ConditionType ConditionType { get; }

		public Func<string, IDictionary<ConditionType, ConditionModel>, Property> FunctionName { get; }

		public ConditionProperty(ConditionType conditionType, Func<string, IDictionary<ConditionType, ConditionModel>, Property> functionName, string displayName, string translationId, TriggerType[] invalidForTriggerTypes = null)
		{
			DisplayName = displayName;
			TranslationId = translationId;
			ConditionType = conditionType;
			FunctionName = functionName;
			_invalidForTriggerTypes = invalidForTriggerTypes;
		}

		public bool IsTriggerTypeValid(TriggerType type)
		{
			if (_invalidForTriggerTypes != null)
			{
				return !_invalidForTriggerTypes.Contains(type);
			}
			return true;
		}
	}

	private static readonly Dictionary<string, ConditionProperty> _properties = new Dictionary<string, ConditionProperty>
	{
		{
			"Always",
			new ConditionProperty(ConditionType.Always, GetConditionTickProperty, ConfigurationApiResources.PropertyRuleAlways, "PropertyRuleAlways", new TriggerType[1] { TriggerType.RecurringTime })
		},
		{
			"WithinTimeProfile",
			new ConditionProperty(ConditionType.WithinTimeProfile, GetConditionTickProperty, ConfigurationApiResources.PropertyRuleWithinSelectedTimeProfile, "PropertyRuleWithinSelectedTimeProfile")
		},
		{
			"WithinTimeProfile.TimeProfile",
			new ConditionProperty(ConditionType.WithinTimeProfile, GetTimeProfilePathProperty, ConfigurationApiResources.PropertyRuleWithinTimeProfile, "PropertyRuleWithinTimeProfile")
		},
		{
			"OutsideTimeProfile",
			new ConditionProperty(ConditionType.OutsideTimeProfile, GetConditionTickProperty, ConfigurationApiResources.PropertyRuleOutsideSelectedTimeProfile, "PropertyRuleOutsideSelectedTimeProfile")
		},
		{
			"OutsideTimeProfile.TimeProfile",
			new ConditionProperty(ConditionType.OutsideTimeProfile, GetTimeProfilePathProperty, ConfigurationApiResources.PropertyRuleOutsideTimeProfile, "PropertyRuleOutsideTimeProfile")
		},
		{
			"TimeOfDayBetween",
			new ConditionProperty(ConditionType.TimeBetween, GetConditionTickProperty, ConfigurationApiResources.PropertyRuleTimeBetween, "PropertyRuleTimeBetween", new TriggerType[1] { TriggerType.RecurringTime })
		},
		{
			"TimeOfDayBetween.StartTime",
			new ConditionProperty(ConditionType.TimeBetween, GetTimeBetweenTimeProperty, ConfigurationApiResources.PropertyRuleTimeBetweenStartTime, "PropertyRuleTimeBetweenStartTime", new TriggerType[1] { TriggerType.RecurringTime })
		},
		{
			"TimeOfDayBetween.EndTime",
			new ConditionProperty(ConditionType.TimeBetween, GetTimeBetweenTimeProperty, ConfigurationApiResources.PropertyRuleTimeBetweenEndTime, "PropertyRuleTimeBetweenEndTime", new TriggerType[1] { TriggerType.RecurringTime })
		},
		{
			"DaysOfWeek",
			new ConditionProperty(ConditionType.DaysOfWeek, GetConditionTickProperty, ConfigurationApiResources.PropertyRuleDaysOfWeek, "PropertyRuleDaysOfWeek", new TriggerType[1] { TriggerType.RecurringTime })
		},
		{
			"DaysOfWeek.Days",
			new ConditionProperty(ConditionType.DaysOfWeek, GetDaysOfWeekProperty, ConfigurationApiResources.PropertyRuleDaysOfWeekSelectedDays, "PropertyRuleDaysOfWeekSelectedDays", new TriggerType[1] { TriggerType.RecurringTime })
		},
		{
			"WithinWindow",
			new ConditionProperty(ConditionType.WithinWindow, GetConditionTickProperty, ConfigurationApiResources.PropertyRuleWithinWindow, "PropertyRuleWithinWindow", new TriggerType[2]
			{
				TriggerType.RecurringTime,
				TriggerType.TimeInterval
			})
		},
		{
			"WithinWindow.Window",
			new ConditionProperty(ConditionType.WithinWindow, GetWindowProperty, ConfigurationApiResources.PropertyRuleWithinWindowSelectedWindow, "PropertyRuleWithinWindowSelectedWindow", new TriggerType[2]
			{
				TriggerType.RecurringTime,
				TriggerType.TimeInterval
			})
		},
		{
			"FailoverActive",
			new ConditionProperty(ConditionType.FailoverActive, GetConditionTickProperty, ConfigurationApiResources.PropertyRuleFailoverActive, "PropertyRuleFailoverActive")
		},
		{
			"FailoverInactive",
			new ConditionProperty(ConditionType.FailoverInactive, GetConditionTickProperty, ConfigurationApiResources.PropertyRuleFailoverInactive, "PropertyRuleFailoverInactive")
		}
	};

	public IEnumerable<Property> Build(IGeminiRequestContext requestContext, RulePartModel rulePart)
	{
		rulePart.NullCheck("", "Build", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\Rules\\Builders\\ConfigurationItem\\ConfigurationItemConditionsBuilder.cs", 49);
		List<Property> list = new List<Property>();
		IDictionary<ConditionType, ConditionModel> conditions = rulePart.Conditions ?? new Dictionary<ConditionType, ConditionModel>();
		foreach (KeyValuePair<string, ConditionProperty> property in _properties)
		{
			if (property.Value.IsTriggerTypeValid(rulePart.TriggerType))
			{
				list.Add(BuildSingle(property.Key, conditions));
			}
		}
		return list.ToArray();
	}

	public static Property BuildSingle(string key, IDictionary<ConditionType, ConditionModel> conditions)
	{
		return _properties[key].FunctionName(key, conditions);
	}

	private static Property GetConditionTickProperty(string propertyName, IDictionary<ConditionType, ConditionModel> conditions)
	{
		string displayName = _properties[propertyName].DisplayName;
		ConditionType conditionType = _properties[propertyName].ConditionType;
		string value = (conditions.Values.FirstOrDefault((ConditionModel x) => x.ConditionType == conditionType) != null).ToString();
		return GeneralPropertyDefinitions.GetTickProperty(propertyName, displayName, value, _properties[propertyName].TranslationId, isSettable: true);
	}

	private static Property GetTimeProfilePathProperty(string propertyName, IDictionary<ConditionType, ConditionModel> conditions)
	{
		string displayName = _properties[propertyName].DisplayName;
		ConditionType conditionType = _properties[propertyName].ConditionType;
		ConditionModel conditionModel = conditions.Values.FirstOrDefault((ConditionModel x) => x.ConditionType == conditionType);
		string value = ((conditionModel != null) ? GetTimeProfilePath(conditionModel) : string.Empty);
		return GeneralPropertyDefinitions.GetPathProperty(propertyName, displayName, value, new List<string> { "TimeProfile" }, _properties[propertyName].TranslationId, isSettable: true);
	}

	private static Property GetTimeBetweenTimeProperty(string propertyName, IDictionary<ConditionType, ConditionModel> conditions)
	{
		string displayName = _properties[propertyName].DisplayName;
		ConditionType conditionType = _properties[propertyName].ConditionType;
		ConditionModel conditionModel = conditions.Values.FirstOrDefault((ConditionModel x) => x.ConditionType == conditionType);
		bool isStart = propertyName.Contains("StartTime");
		string value = ((conditionModel != null) ? GetTimeBetweenTime(conditionModel, isStart) : "08:00:00");
		return GeneralPropertyDefinitions.GetTimeProperty(propertyName, displayName, value, _properties[propertyName].TranslationId, isSettable: true);
	}

	private static string GetTimeProfilePath(ConditionModel condition)
	{
		string key = ((condition.ConditionType == ConditionType.WithinTimeProfile) ? "WithinTimeProfile" : "OutsideTimeProfile");
		string text = "";
		if (condition.Parameters.TryGetValue(key, out var value))
		{
			text = value.Value;
		}
		if (!string.IsNullOrEmpty(text))
		{
			return ConfigurationItemPath.CreateItemPath("TimeProfile", text).Text;
		}
		return "";
	}

	private static string GetTimeBetweenTime(ConditionModel condition, bool isStart)
	{
		string key = (isStart ? "StartTime" : "EndTime");
		return condition.Parameters[key].Value;
	}

	private static Property GetDaysOfWeekProperty(string propertyName, IDictionary<ConditionType, ConditionModel> conditions)
	{
		string displayName = _properties[propertyName].DisplayName;
		ConditionModel conditionModel = conditions.Values.FirstOrDefault((ConditionModel x) => x.ConditionType == ConditionType.DaysOfWeek);
		string values = ((conditionModel?.Parameters["DayOfWeekIsOneOf"]?.Values != null) ? string.Join(";", conditionModel.Parameters["DayOfWeekIsOneOf"].Values) : string.Empty);
		return ConfigurationItemConditionsHelper.GetDaysOfWeekConditionProperty(propertyName, displayName, _properties[propertyName].TranslationId, values);
	}

	private static Property GetWindowProperty(string propertyName, IDictionary<ConditionType, ConditionModel> conditions)
	{
		string displayName = _properties[propertyName].DisplayName;
		ConditionType conditionType = _properties[propertyName].ConditionType;
		ConditionModel conditionModel = conditions.Values.FirstOrDefault((ConditionModel x) => x.ConditionType == conditionType);
		string value = "";
		if (conditionModel != null && conditionModel.Parameters.TryGetValue("EventIsWithinWindow", out var value2))
		{
			value = value2.Value;
		}
		return GeneralPropertyDefinitions.GetStringProperty(propertyName, value, displayName, _properties[propertyName].TranslationId, isSettable: true);
	}
}
