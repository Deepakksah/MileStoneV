using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Definitions;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Validation;
using VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Validation;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Builders;

public class ConfigurationItemActionPropertiesBuilder : IConfigurationPropertiesBuilder<RuleModel>
{
	private class ParameterBuilderData
	{
		public string Name { get; set; }

		public List<string> Values { get; set; }

		public List<string> AllowedTypes { get; set; }

		public Dictionary<string, string> AllowedKeyValuePairs { get; set; }

		public RuleParameterDefinition Definition { get; set; }
	}

	private static readonly Guid FromTriggerId = new Guid("00000000-0000-0000-0000-000000000000");

	private static readonly Guid FromMetadataId = new Guid("11111111-1111-1111-1111-111111111111");

	private readonly IValueValidatorFactory _validatorFactory;

	private readonly ActionDefinitionsProvider _actionValidationModelProvider;

	public ConfigurationItemActionPropertiesBuilder(IValueValidatorFactory validatorFactory, IMIPActionProxy mipActionProxy, IMIPKindProxy mipKindProxy, IRecorderPluginProxy recorderPluginProxy, ISiteProxy siteProxy, IGeminiApplicationCoreProxy geminiApplicationCoreProxy)
	{
		_validatorFactory = validatorFactory;
		_actionValidationModelProvider = new ActionDefinitionsProvider(mipActionProxy, mipKindProxy, recorderPluginProxy, siteProxy, geminiApplicationCoreProxy);
	}

	public IEnumerable<Property> Build(IGeminiRequestContext requestContext, RuleModel rule)
	{
		rule.NullCheck("", "Build", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\Rules\\Builders\\ConfigurationItem\\ConfigurationItemActionPropertiesBuilder.cs", 43);
		List<Property> list = new List<Property>();
		list.AddRange(Build(requestContext, rule.RuleStart.Actions, isStart: true, rule.RuleStart.TriggerType));
		list.AddRange(Build(requestContext, rule.RuleStop.Actions, isStart: false, rule.RuleStop.TriggerType));
		return list;
	}

	private IEnumerable<Property> Build(IGeminiRequestContext requestContext, IEnumerable<ActionModel> actions, bool isStart, TriggerType ruleType)
	{
		List<Property> list = new List<Property>();
		if (actions == null)
		{
			return list;
		}
		string semicolonSeparatedItems = string.Join(";", actions.Select((ActionModel action) => action.ActionType.ToString()));
		IDictionary<string, string> actionsForDisplay = _actionValidationModelProvider.GetActionsForDisplay(requestContext, isStart, ruleType);
		list.Add(GeneralPropertyDefinitions.GetListProperty(semicolonSeparatedItems, isStart ? "StartActions" : "StopActions", isStart ? ConfigurationApiResources.PropertyRuleStartActions : ConfigurationApiResources.PropertyRuleStopActions, isStart ? "PropertyRuleStartActions" : "PropertyRuleStopActions", isSettable: true, actionsForDisplay, serverValidation: true));
		foreach (ActionModel action in actions)
		{
			list.AddRange(GetActionParameters(requestContext, action, isStart));
		}
		return list;
	}

	private IEnumerable<Property> GetActionParameters(IGeminiRequestContext requestContext, ActionModel action, bool isStart)
	{
		IActionDefinition actionDefinition = _actionValidationModelProvider.Get(requestContext, action.ActionType);
		if (actionDefinition == null)
		{
			throw new NotSupportedException(action.ActionType.ToString());
		}
		return MapToBuilderData(requestContext, isStart, action, actionDefinition).Select(CreateProperty).ToArray();
	}

	private static Property CreateProperty(ParameterBuilderData parameter)
	{
		string value = string.Join(";", parameter.Values);
		return new Property
		{
			Key = parameter.Name,
			Value = value,
			ValueType = ConfigurationItemActionHelper.GetPropertyType(parameter.Definition),
			ValueTypeInfos = GetPropertyTypeInfos(parameter),
			ItemReference = ConfigurationItemActionHelper.IsItemReference(parameter.Definition),
			IsSettable = true,
			DisplayName = parameter.Name,
			ServerValidation = false,
			TranslationId = string.Empty,
			ToolTipTranslationId = (parameter.Definition.ToolTipTranslationId ?? string.Empty),
			UIImportance = UIImportance.Summary,
			SortKey = 100
		};
	}

	private static ValueTypeInfo[] GetPropertyTypeInfos(ParameterBuilderData parameter)
	{
		if (parameter.Definition.ValueType.IsEnum)
		{
			return GeneralPropertyDefinitions.GetEnumValuesFromType(parameter.Definition.ValueType);
		}
		string propertyType = ConfigurationItemActionHelper.GetPropertyType(parameter.Definition);
		if (ConfigurationItemActionHelper.RangedValueTypes.Contains(propertyType) && parameter.Definition.MaxValue != null && parameter.Definition.MinValue != null)
		{
			return PropertyTypesHelper.GetMinMaxRange(parameter.Definition.MinValue.ToString(), parameter.Definition.MaxValue.ToString());
		}
		if (ConfigurationItemActionHelper.PathTypes.Contains(propertyType) && parameter.AllowedTypes != null && parameter.AllowedTypes.Any())
		{
			return parameter.AllowedTypes.Select((string s) => new ValueTypeInfo
			{
				Name = "PathItemType",
				Value = s
			}).ToArray();
		}
		if (parameter.AllowedKeyValuePairs != null && parameter.AllowedKeyValuePairs.Any())
		{
			return parameter.AllowedKeyValuePairs.Select((KeyValuePair<string, string> s) => new ValueTypeInfo
			{
				Name = s.Value,
				Value = s.Key
			}).ToArray();
		}
		return null;
	}

	private ParameterBuilderData[] MapToBuilderData(IGeminiRequestContext requestContext, bool isStart, ActionModel action, IActionDefinition actionValidationModel)
	{
		SortedDictionary<string, ParameterBuilderData> sortedDictionary = new SortedDictionary<string, ParameterBuilderData>();
		foreach (RuleParameter parameter in action.Parameters.Values)
		{
			if (!actionValidationModel.Parameters.TryGetValue(parameter.Name, out var value))
			{
				value = actionValidationModel.Parameters.Values.FirstOrDefault((RuleParameterDefinition p) => p.XmlName == parameter.Name);
				if (value == null)
				{
					continue;
				}
			}
			string text = null;
			string value2 = null;
			if (!string.IsNullOrEmpty(value.ParameterToAppend))
			{
				text = value.ParameterToAppend;
				if (parameter.Values.Length != 2)
				{
					throw new ArgumentException("Incorrect format of parameter " + parameter.Name + " value: " + parameter.Value);
				}
				value2 = parameter.Values[1];
				parameter.Value = parameter.Values[0];
			}
			AppendOrUpdateBuilderData(requestContext, sortedDictionary, isStart, action, parameter, value);
			if (text != null && actionValidationModel.Parameters.TryGetValue(text, out var value3))
			{
				RuleParameter parameter2 = new RuleParameter
				{
					ExcludeFromDatabase = true,
					Value = value2,
					Name = text
				};
				AppendOrUpdateBuilderData(requestContext, sortedDictionary, isStart, action, parameter2, value3);
			}
		}
		return sortedDictionary.Select((KeyValuePair<string, ParameterBuilderData> s) => s.Value).ToArray();
	}

	private void AppendOrUpdateBuilderData(IGeminiRequestContext requestContext, SortedDictionary<string, ParameterBuilderData> builderDataCollection, bool isStart, ActionModel action, RuleParameter parameter, RuleParameterDefinition validationModel)
	{
		if (!validationModel.IsVisible)
		{
			return;
		}
		string text = (string.IsNullOrEmpty(validationModel.PropertyName) ? parameter.Name : validationModel.PropertyName);
		if (!string.IsNullOrEmpty(validationModel.XmlName))
		{
			text = validationModel.Name;
		}
		string[] parameterValues = GetParameterValues(requestContext, parameter, validationModel, action.Parameters);
		if (builderDataCollection.TryGetValue(text, out var value))
		{
			value.Values.AddRange(parameterValues);
			value.AllowedTypes.AddRange(validationModel.AllowedItemTypes);
			{
				foreach (KeyValuePair<string, string> allowedKeyValuePair in validationModel.AllowedKeyValuePairs)
				{
					value.AllowedKeyValuePairs.Add(allowedKeyValuePair.Key, allowedKeyValuePair.Value);
				}
				return;
			}
		}
		string name = string.Format(CultureInfo.InvariantCulture, "{0}.{1}.{2}", isStart ? "Start" : "Stop", action.ActionType, text);
		value = new ParameterBuilderData
		{
			Name = name,
			Definition = validationModel,
			AllowedTypes = validationModel.AllowedItemTypes.ToList(),
			AllowedKeyValuePairs = validationModel.AllowedKeyValuePairs.ToDictionary((KeyValuePair<string, string> p) => p.Key, (KeyValuePair<string, string> p) => p.Value),
			Values = parameterValues.ToList()
		};
		builderDataCollection[text] = value;
	}

	private string[] GetParameterValues(IGeminiRequestContext requestContext, RuleParameter parameter, RuleParameterDefinition validationModel, IDictionary<string, RuleParameter> parameters)
	{
		if (parameter.IsEmpty)
		{
			return new string[0];
		}
		string[] values = parameter.Values;
		if (validationModel.ValidatorType == null)
		{
			return values;
		}
		IValueValidator validator = _validatorFactory.GetValidator(validationModel.ValidatorType);
		if (validator == null)
		{
			return values;
		}
		if (validationModel.ValueType == typeof(Guid) || (validationModel.ValueType == typeof(Guid[]) && validationModel.AllowedItemTypes != null && validationModel.AllowedItemTypes.Any()))
		{
			return values.Select((string x) => GetItemPath(requestContext, validator, validationModel, x, parameters)).ToArray();
		}
		return values.Select((string x) => GetParameterValue(requestContext, validator, x, parameters)).ToArray();
	}

	private static string GetParameterValue(IGeminiRequestContext requestContext, IValueValidator validator, string value, IDictionary<string, RuleParameter> parameters)
	{
		if (!validator.Validate(requestContext, value, null, parameters))
		{
			return "";
		}
		return value;
	}

	private static string GetItemPath(IGeminiRequestContext requestContext, IValueValidator validator, RuleParameterDefinition validationModel, string value, IDictionary<string, RuleParameter> parameters)
	{
		if (value == FromTriggerId.ToString() || value == FromMetadataId.ToString())
		{
			if (!validationModel.AllowedItemTypes.Any())
			{
				throw new ArgumentException("Allowed Item Types does not contain data for parameter " + validationModel.Name);
			}
			return ConfigurationItemPath.CreateItemPath(validationModel.AllowedItemTypes[0], value).Text;
		}
		string itemType = validator.GetItemType(requestContext, value, parameters);
		return (string.IsNullOrEmpty(itemType) ? ConfigurationItemPath.CreateItemPath("Unknown", value) : ConfigurationItemPath.CreateItemPath(itemType, value)).Text;
	}
}
