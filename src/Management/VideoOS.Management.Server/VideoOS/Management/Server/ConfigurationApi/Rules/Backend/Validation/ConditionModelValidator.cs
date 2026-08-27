using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Definitions;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;
using VideoOS.Management.Server.ConfigurationApi.Rules.Builders;
using VideoOS.Utilities.Validation;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Validation;

public class ConditionModelValidator : IValidator<ConditionModel>
{
	private static Dictionary<ConditionType, string> _parameterKeys = new Dictionary<ConditionType, string>
	{
		{
			ConditionType.WithinTimeProfile,
			"WithinTimeProfile.TimeProfile"
		},
		{
			ConditionType.OutsideTimeProfile,
			"OutsideTimeProfile.TimeProfile"
		},
		{
			ConditionType.DaysOfWeek,
			"DaysOfWeek.Days"
		},
		{
			ConditionType.WithinWindow,
			"WithinWindow.Window"
		}
	};

	private readonly IParametersValidator _parameterValidator;

	private readonly ConditionDefinitionsProvider _validationModelProvider;

	public ConditionModelValidator(IParametersValidator parametersValidator)
	{
		_parameterValidator = parametersValidator;
		_validationModelProvider = new ConditionDefinitionsProvider();
	}

	public ValidationError[] Validate(IGeminiRequestContext requestContext, ConditionModel model)
	{
		model.NullCheck("", "Validate", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\Rules\\Backend\\Validation\\Common\\ConditionModelValidator.cs", 36);
		IConditionDefinition conditionDefinition = _validationModelProvider.Get(model.ConditionType);
		if (conditionDefinition == null)
		{
			return new ValidationError[0];
		}
		ValidationError[] source = _parameterValidator.Validate(requestContext, model.Parameters, conditionDefinition.Parameters);
		if (source.Any())
		{
			return source.Select((ValidationError s) => ValidationError.WithPrefix("Condition: " + model.ConditionType, s, GetPropertyName(model.ConditionType, s.PropertyName), GetEmptyParameterProperty(model, s))).ToArray();
		}
		List<ValidationError> list = new List<ValidationError>();
		list.AddRange(ValidateTimeBetweenCondition(model));
		return list.ToArray();
	}

	private static IEnumerable<ValidationError> ValidateTimeBetweenCondition(ConditionModel model)
	{
		if (model.ConditionType != ConditionType.TimeBetween)
		{
			return new ValidationError[0];
		}
		RuleParameter value;
		RuleParameter ruleParameter = (model.Parameters.TryGetValue("StartTime", out value) ? value : null);
		RuleParameter value2;
		RuleParameter ruleParameter2 = (model.Parameters.TryGetValue("EndTime", out value2) ? value2 : null);
		if (ruleParameter == null || ruleParameter2 == null)
		{
			return new ValidationError[0];
		}
		if (!string.Equals(ruleParameter.Value, ruleParameter2.Value, StringComparison.OrdinalIgnoreCase))
		{
			return new ValidationError[0];
		}
		return new ValidationError[1] { ValidationError.WithPrefix("Condition: " + model.ConditionType, new ValidationError("Start and End time must not be identical."), GetPropertyName(model.ConditionType, "StartTime")) };
	}

	private static string GetPropertyName(ConditionType type, string parameterName)
	{
		if (type == ConditionType.TimeBetween)
		{
			return "TimeOfDayBetween." + parameterName;
		}
		if (_parameterKeys.TryGetValue(type, out var value))
		{
			return value;
		}
		return null;
	}

	private static Property GetEmptyParameterProperty(ConditionModel model, ValidationError error)
	{
		if (model == null || model.Parameters == null)
		{
			return null;
		}
		string propertyName = GetPropertyName(model.ConditionType, error.PropertyName);
		Dictionary<ConditionType, ConditionModel> dictionary = new Dictionary<ConditionType, ConditionModel>();
		foreach (RuleParameter value in model.Parameters.Values)
		{
			if (value != null)
			{
				value.Value = "";
			}
		}
		dictionary.Add(model.ConditionType, model);
		return ConfigurationItemConditionsBuilder.BuildSingle(propertyName, dictionary);
	}
}
