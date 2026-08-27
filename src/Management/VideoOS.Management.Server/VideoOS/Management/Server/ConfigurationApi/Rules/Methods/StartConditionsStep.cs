using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Methods;

public class StartConditionsStep : IWizardStep
{
	public struct ConditionParameter(string name, string displayName, string translationId, string defaultValue)
	{
		public string ParameterName = name;

		public string DisplayName = displayName;

		public string DefaultValue = defaultValue;

		public string TranslationId = translationId;
	}

	private class DisplayName
	{
		public string Value { get; }

		public string TranslationId { get; }

		public DisplayName(string displayName, string translationId)
		{
			Value = displayName;
			TranslationId = translationId;
		}
	}

	private static readonly Dictionary<string, DisplayName> _conditionsTypeProperties = new Dictionary<string, DisplayName>
	{
		{
			"Always",
			new DisplayName(ConfigurationApiResources.PropertyRuleAlways, "PropertyRuleAlways")
		},
		{
			"WithinTimeProfile",
			new DisplayName(ConfigurationApiResources.PropertyRuleWithinSelectedTimeProfile, "PropertyRuleWithinSelectedTimeProfile")
		},
		{
			"OutsideTimeProfile",
			new DisplayName(ConfigurationApiResources.PropertyRuleOutsideSelectedTimeProfile, "PropertyRuleOutsideSelectedTimeProfile")
		},
		{
			"TimeOfDayBetween",
			new DisplayName(ConfigurationApiResources.PropertyRuleTimeBetween, "PropertyRuleTimeBetween")
		},
		{
			"DaysOfWeek",
			new DisplayName(ConfigurationApiResources.PropertyRuleDaysOfWeek, "PropertyRuleDaysOfWeek")
		},
		{
			"WithinWindow",
			new DisplayName(ConfigurationApiResources.PropertyRuleWithinWindow, "PropertyRuleWithinWindow")
		},
		{
			"FailoverActive",
			new DisplayName(ConfigurationApiResources.PropertyRuleFailoverActive, "PropertyRuleFailoverActive")
		},
		{
			"FailoverInactive",
			new DisplayName(ConfigurationApiResources.PropertyRuleFailoverInactive, "PropertyRuleFailoverInactive")
		}
	};

	private static readonly Dictionary<string, DisplayName> _recurringRuleConditionsTypeProperties = new Dictionary<string, DisplayName>
	{
		{
			"WithinTimeProfile",
			new DisplayName(ConfigurationApiResources.PropertyRuleWithinSelectedTimeProfile, "PropertyRuleWithinSelectedTimeProfile")
		},
		{
			"OutsideTimeProfile",
			new DisplayName(ConfigurationApiResources.PropertyRuleOutsideSelectedTimeProfile, "PropertyRuleOutsideSelectedTimeProfile")
		}
	};

	private static readonly Dictionary<string, Func<string, string, string, string, Property>> _conditionFunctionMap = new Dictionary<string, Func<string, string, string, string, Property>>
	{
		{ "WithinTimeProfile", GetTimeProfileProperty },
		{ "OutsideTimeProfile", GetTimeProfileProperty },
		{ "TimeOfDayBetween", GetTimeProperty },
		{
			"DaysOfWeek",
			ConfigurationItemConditionsHelper.GetDaysOfWeekConditionProperty
		},
		{ "WithinWindow", GetWindowProperty }
	};

	private static readonly Dictionary<string, IEnumerable<ConditionParameter>> _conditionParameterProperties = new Dictionary<string, IEnumerable<ConditionParameter>>
	{
		{
			"WithinTimeProfile",
			new List<ConditionParameter>
			{
				new ConditionParameter("WithinTimeProfile.TimeProfile", ConfigurationApiResources.PropertyRuleWithinTimeProfile, "PropertyRuleWithinTimeProfile", string.Empty)
			}
		},
		{
			"OutsideTimeProfile",
			new List<ConditionParameter>
			{
				new ConditionParameter("OutsideTimeProfile.TimeProfile", ConfigurationApiResources.PropertyRuleOutsideTimeProfile, "PropertyRuleOutsideTimeProfile", string.Empty)
			}
		},
		{
			"TimeOfDayBetween",
			new List<ConditionParameter>
			{
				new ConditionParameter("TimeOfDayBetween.StartTime", ConfigurationApiResources.PropertyRuleTimeBetweenStartTime, "PropertyRuleTimeBetweenStartTime", "08:00:00"),
				new ConditionParameter("TimeOfDayBetween.EndTime", ConfigurationApiResources.PropertyRuleTimeBetweenEndTime, "PropertyRuleTimeBetweenEndTime", "08:00:00")
			}
		},
		{
			"DaysOfWeek",
			new List<ConditionParameter>
			{
				new ConditionParameter("DaysOfWeek.Days", ConfigurationApiResources.PropertyRuleDaysOfWeekSelectedDays, "PropertyRuleDaysOfWeekSelectedDays", string.Empty)
			}
		},
		{
			"WithinWindow",
			new List<ConditionParameter>
			{
				new ConditionParameter("WithinWindow.Window", ConfigurationApiResources.PropertyRuleWithinWindowSelectedWindow, "PropertyRuleWithinWindowSelectedWindow", string.Empty)
			}
		}
	};

	public bool IsComplete(IGeminiRequestContext requestContext, ConfigurationItem item)
	{
		if (!item.TryGetPropertyValue("StartRuleType", out TriggerType result))
		{
			return false;
		}
		Dictionary<string, DisplayName> allowedProperties = GetAllowedProperties(result);
		if (!allowedProperties.Select((KeyValuePair<string, DisplayName> p) => item.IsPropertyPresent(p.Key)).All((bool v) => v))
		{
			return false;
		}
		if (result == TriggerType.TimeInterval && !allowedProperties.Select((KeyValuePair<string, DisplayName> p) => item.IsPropertySelected(p.Key)).Any((bool v) => v))
		{
			return false;
		}
		Dictionary<string, IEnumerable<ConditionParameter>> dictionary = _conditionParameterProperties.Where((KeyValuePair<string, IEnumerable<ConditionParameter>> p) => allowedProperties.ContainsKey(p.Key)).ToDictionary((KeyValuePair<string, IEnumerable<ConditionParameter>> p) => p.Key, (KeyValuePair<string, IEnumerable<ConditionParameter>> p) => p.Value);
		foreach (KeyValuePair<string, IEnumerable<ConditionParameter>> item2 in dictionary)
		{
			if (item.IsPropertySelected(item2.Key) && dictionary[item2.Key].Any((ConditionParameter v) => !item.IsPropertyValuePresent(v.ParameterName)))
			{
				return false;
			}
		}
		return true;
	}

	private static Dictionary<string, DisplayName> GetAllowedProperties(TriggerType ruleType)
	{
		return ruleType switch
		{
			TriggerType.RecurringTime => _recurringRuleConditionsTypeProperties, 
			TriggerType.Event => _conditionsTypeProperties, 
			_ => _conditionsTypeProperties.Where((KeyValuePair<string, DisplayName> kv) => kv.Key != "WithinWindow").ToDictionary((KeyValuePair<string, DisplayName> kv) => kv.Key, (KeyValuePair<string, DisplayName> kv) => kv.Value), 
		};
	}

	public List<Property> GetCurrentProperties(IGeminiRequestContext requestContext, ConfigurationItem item, out string lastPropertyKey)
	{
		List<Property> list = new List<Property>();
		lastPropertyKey = null;
		if (item.TryGetPropertyValue("StartRuleType", out TriggerType result))
		{
			foreach (KeyValuePair<string, DisplayName> allowedProperty in GetAllowedProperties(result))
			{
				GetCurrentConditionParameterProperties(allowedProperty.Key, item, list);
			}
		}
		return list;
	}

	private static void GetCurrentConditionParameterProperties(string condition, ConfigurationItem item, List<Property> properties)
	{
		if (item.TryGetProperty(condition, out bool result))
		{
			properties.Add(GetConditionTypeProperty(condition, result));
		}
		if (!item.IsPropertySelected(condition) || !_conditionParameterProperties.ContainsKey(condition))
		{
			return;
		}
		foreach (ConditionParameter item2 in _conditionParameterProperties[condition])
		{
			if (item.TryGetProperty(item2.ParameterName, out var result2))
			{
				properties.Add(_conditionFunctionMap[condition](item2.ParameterName, item2.DisplayName, item2.TranslationId, result2));
			}
		}
	}

	public List<Property> GetNextProperties(IGeminiRequestContext requestContext, ConfigurationItem item, string lastPropertyKey)
	{
		List<Property> list = new List<Property>();
		item.TryGetPropertyValue("StartRuleType", out TriggerType result);
		foreach (KeyValuePair<string, DisplayName> allowedProperty in GetAllowedProperties(result))
		{
			GetNextConditionParameterProperties(allowedProperty.Key, item, list);
		}
		return list;
	}

	private static void GetNextConditionParameterProperties(string condition, ConfigurationItem item, List<Property> properties)
	{
		if (!item.IsPropertyPresent(condition))
		{
			properties.Add(GetConditionTypeProperty(condition, value: false));
		}
		if (!item.IsPropertySelected(condition) || !_conditionParameterProperties.ContainsKey(condition))
		{
			return;
		}
		foreach (ConditionParameter item2 in _conditionParameterProperties[condition])
		{
			if (!item.IsPropertyPresent(item2.ParameterName))
			{
				properties.Add(_conditionFunctionMap[condition](item2.ParameterName, item2.DisplayName, item2.TranslationId, item2.DefaultValue));
			}
		}
	}

	private static Property GetConditionTypeProperty(string name, bool value)
	{
		return GeneralPropertyDefinitions.GetTickProperty(name, _conditionsTypeProperties[name].Value, TypeHelper.ToString(value), _conditionsTypeProperties[name].TranslationId, isSettable: true);
	}

	private static Property GetTimeProfileProperty(string name, string displayName, string translationId, string value)
	{
		List<string> itemTypes = new List<string> { "TimeProfile" };
		return GeneralPropertyDefinitions.GetPathProperty(name, displayName, value, itemTypes, translationId, isSettable: true);
	}

	private static Property GetTimeProperty(string name, string displayName, string translationId, string value)
	{
		return GeneralPropertyDefinitions.GetTimeProperty(name, displayName, value, translationId, isSettable: true);
	}

	private static Property GetWindowProperty(string name, string displayName, string translationId, string value)
	{
		return GeneralPropertyDefinitions.GetStringProperty(name, value, displayName, translationId, isSettable: true);
	}
}
