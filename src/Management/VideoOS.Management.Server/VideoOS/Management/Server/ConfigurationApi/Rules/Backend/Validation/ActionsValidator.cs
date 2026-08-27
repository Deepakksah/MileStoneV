using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Definitions;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;
using VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Validation;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Validation;

public class ActionsValidator : IValidator<RuleModel>
{
	private readonly IParametersValidator _parameterValidator;

	private readonly ActionDefinitionsProvider _validationModelProvider;

	public ActionsValidator(IParametersValidator parametersValidator, IMIPActionProxy mipActionProxy, IMIPKindProxy mipKindProxy, IRecorderPluginProxy recorderPluginProxy, ISiteProxy siteProxy, IGeminiApplicationCoreProxy geminiApplicationCoreProxy)
	{
		_parameterValidator = parametersValidator;
		_validationModelProvider = new ActionDefinitionsProvider(mipActionProxy, mipKindProxy, recorderPluginProxy, siteProxy, geminiApplicationCoreProxy);
	}

	public ValidationError[] Validate(IGeminiRequestContext requestContext, RuleModel model)
	{
		model.NullCheck("", "Validate", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\Rules\\Backend\\Validation\\Common\\ActionsValidator.cs", 33);
		if (model.RuleStart.Actions == null || !model.RuleStart.Actions.Any())
		{
			return new ValidationError[1]
			{
				new ValidationError("Rule must have at least one start action set")
				{
					PropertyName = "StartActions",
					ValidProperty = GetActionProperty(requestContext, isStart: true, model)
				}
			};
		}
		ValidationError[] array = ValidateActionTypes(requestContext, model);
		if (array.Any())
		{
			return array;
		}
		ValidationError[] array2 = ValidateCounterAction(requestContext, model.RuleStart, model.RuleStop, fromStart: true);
		if (array2.Any())
		{
			return array2;
		}
		ValidationError[] array3 = ValidateCounterAction(requestContext, model.RuleStop, model.RuleStart, fromStart: false);
		if (array3.Any())
		{
			return array3;
		}
		List<ValidationError> list = new List<ValidationError>();
		list.AddRange(ValidateActionParameters(requestContext, model.RuleStart.Actions, "Start"));
		list.AddRange(ValidateActionParameters(requestContext, model.RuleStop?.Actions ?? new ActionModel[0], "Stop"));
		return list.ToArray();
	}

	private bool IsValidActionType(IGeminiRequestContext requestContext, string key, TriggerType ruleType, bool isStart, out string errorMessage)
	{
		errorMessage = string.Format(CultureInfo.InvariantCulture, "Action type(s) for {0} are not valid", isStart ? "start rule" : "stop rule");
		string[] array = key.Split(':');
		if (array.Any() && Enum.TryParse<ActionType>(array[0], out var result))
		{
			switch (result)
			{
			case ActionType.None:
				return false;
			case ActionType.MIPAction:
			case ActionType.MIPAction2:
			{
				if (array.Length != 2 || !Guid.TryParse(array[1], out var _))
				{
					return false;
				}
				break;
			}
			}
			if (!_validationModelProvider.IsActionValidForRuleType(requestContext, key, ruleType))
			{
				errorMessage = string.Format(CultureInfo.InvariantCulture, "Action type {0} for {1} is not valid for rule type {2}", result.ToString(), isStart ? "start rule" : "stop rule", ruleType.ToString());
				return false;
			}
			if (!_validationModelProvider.AreRequiredFeaturesLicensed(requestContext, key))
			{
				string arg = string.Join(", ", _validationModelProvider.GetActionRequiredFeatures(requestContext, key));
				errorMessage = string.Format(CultureInfo.InvariantCulture, "Action type {0} for {1} requires licensed feature(s): {2}", result.ToString(), isStart ? "start rule" : "stop rule", arg);
				return false;
			}
			return true;
		}
		return false;
	}

	private ValidationError[] ValidateActionTypes(IGeminiRequestContext requestContext, RuleModel model)
	{
		List<ValidationError> list = new List<ValidationError>();
		string errorMessage = "";
		if (model.RuleStart.Actions.Any((ActionModel a) => !IsValidActionType(requestContext, a.ActionType, model.RuleStart.TriggerType, isStart: true, out errorMessage)))
		{
			list.Add(new ValidationError(errorMessage)
			{
				PropertyName = "StartActions",
				ValidProperty = GetActionProperty(requestContext, isStart: true, model, model.RuleStart.Actions)
			});
		}
		if (model.RuleStop?.Actions != null && model.RuleStop.Actions.Any((ActionModel a) => !IsValidActionType(requestContext, a.ActionType, model.RuleStart.TriggerType, isStart: false, out errorMessage)))
		{
			list.Add(new ValidationError(errorMessage)
			{
				PropertyName = "StopActions",
				ValidProperty = GetActionProperty(requestContext, isStart: false, model, model.RuleStop.Actions)
			});
		}
		return list.ToArray();
	}

	private ValidationError[] ValidateCounterAction(IGeminiRequestContext requestContext, RulePartModel fromModel, RulePartModel versusModel, bool fromStart)
	{
		if (fromModel == null || fromModel.TriggerType == TriggerType.None)
		{
			return new ValidationError[0];
		}
		string fromModelName = (fromStart ? "RuleStart" : "RuleStop");
		string versusModelName = ((!fromStart) ? "RuleStart" : "RuleStop");
		string versusModelEmptyMessage = ((fromStart && (versusModel == null || versusModel.TriggerType == TriggerType.None)) ? string.Format(CultureInfo.InvariantCulture, " and it to be non-empty (set the {0} property)", "StopRuleType") : "");
		return (from action in fromModel.Actions
			select new
			{
				action = action,
				validationModel = _validationModelProvider.Get(requestContext, action.ActionType)
			} into s
			where s.validationModel != null && !TestForValidCounterAction(versusModel, s.validationModel)
			select new ValidationError("{0} action in {1} requires {2} action set in {3}{4}", s.action.ActionType, fromModelName, s.validationModel.CounteractionActionType, versusModelName, versusModelEmptyMessage)
			{
				PropertyName = (fromStart ? "StartActions" : "StopActions")
			}).ToArray();
	}

	private static bool TestForValidCounterAction(RulePartModel versusModel, IActionDefinition validationModel)
	{
		if (!(validationModel.CounteractionActionType == ActionType.None.ToString()))
		{
			if (versusModel != null && versusModel.TriggerType != TriggerType.None)
			{
				return versusModel.Actions.Any((ActionModel a) => a.ActionType == validationModel.CounteractionActionType);
			}
			return false;
		}
		return true;
	}

	private ValidationError[] ValidateActionParameters(IGeminiRequestContext requestContext, IEnumerable<ActionModel> actions, string actionPrefix)
	{
		List<ValidationError> list = new List<ValidationError>();
		foreach (ActionModel action in actions)
		{
			IActionDefinition validationModel = _validationModelProvider.Get(requestContext, action.ActionType);
			if (validationModel == null)
			{
				continue;
			}
			ValidationError[] source = _parameterValidator.Validate(requestContext, action.Parameters, validationModel.Parameters);
			if (!source.Any())
			{
				continue;
			}
			list.AddRange(source.Select(delegate(ValidationError s)
			{
				RuleParameterDefinition definition = validationModel.Parameters.Values.FirstOrDefault((RuleParameterDefinition x) => x.PropertyName == s.PropertyName || x.Name == s.PropertyName);
				return ValidationError.WithPrefix("Action: " + action.ActionType, s, GetActionParameterPropertyName(actionPrefix, action.ActionType, s.PropertyName, definition), GetValidPropertyForActionParameter(requestContext, actionPrefix, action, s));
			}));
		}
		return list.ToArray();
	}

	private Property GetValidPropertyForActionParameter(IGeminiRequestContext requestContext, string actionPrefix, ActionModel action, ValidationError error)
	{
		RuleParameterDefinition ruleParameterDefinition = _validationModelProvider.Get(requestContext, action.ActionType)?.Parameters.Values.FirstOrDefault((RuleParameterDefinition x) => (x.PropertyName ?? x.Name) == error.PropertyName);
		if (ruleParameterDefinition == null || !ruleParameterDefinition.IsVisible)
		{
			return null;
		}
		if (error.ValidProperty != null)
		{
			return error.ValidProperty;
		}
		if (!action.Parameters.Keys.Contains(error.PropertyName))
		{
			string actionParameterPropertyName = GetActionParameterPropertyName(actionPrefix, action.ActionType, error.PropertyName, ruleParameterDefinition);
			string propertyValue = ruleParameterDefinition.DefaultValue ?? "";
			return ConfigurationItemActionHelper.GetProperty(ruleParameterDefinition, actionParameterPropertyName, propertyValue);
		}
		return null;
	}

	private Property GetActionProperty(IGeminiRequestContext requestContext, bool isStart, RuleModel model, IEnumerable<ActionModel> actions = null)
	{
		string text = "";
		if (actions != null)
		{
			bool flag = true;
			foreach (ActionModel action in actions)
			{
				if (action.ActionType != ActionType.None.ToString() && ConfigurationItemActionHelper.IsCorrectPosition(isStart, _validationModelProvider.Get(requestContext, action.ActionType).Position))
				{
					if (flag)
					{
						flag = false;
					}
					else
					{
						text += ";";
					}
					text += action.ActionType.ToString();
				}
			}
		}
		string name = (isStart ? "StartActions" : "StopActions");
		return GeneralPropertyDefinitions.GetListProperty(text, name, isStart ? ConfigurationApiResources.PropertyRuleStartActions : ConfigurationApiResources.PropertyRuleStopActions, isStart ? "PropertyRuleStartActions" : "PropertyRuleStopActions", isSettable: true, _validationModelProvider.GetActionsForDisplay(requestContext, isStart, isStart ? model.RuleStart.TriggerType : model.RuleStop.TriggerType));
	}

	private static string GetActionParameterPropertyName(string actionPrefix, string actionType, string parameterName, RuleParameterDefinition definition)
	{
		parameterName = ((!string.IsNullOrEmpty(definition.PropertyName)) ? definition.PropertyName : parameterName);
		return string.Format(CultureInfo.InvariantCulture, "{0}.{1}.{2}", actionPrefix, actionType, parameterName);
	}
}
