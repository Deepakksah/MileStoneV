using System.Collections.Generic;
using System.Linq;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Validation;

public class RuleModelValidator : IValidator<RuleModel>
{
	private delegate ValidationError[] ValidatorFunction(IGeminiRequestContext requestContext, RuleModel model);

	private readonly ValidatorFunction[] _validatorFunctions;

	private readonly IValidator<ConditionModel> _conditionsValidator;

	private readonly IValidator<RulePartModel> _eventValidator;

	private readonly IValidator<RuleModel> _actionsValidator;

	private readonly IValidator<RuleModel> _recurringTimeValidator;

	public RuleModelValidator(IValidator<ConditionModel> conditionsValidator, IValidator<RulePartModel> eventValidator, IValidator<RuleModel> actionsValidator, IValidator<RuleModel> recurringTimeValidator)
	{
		_conditionsValidator = conditionsValidator;
		_eventValidator = eventValidator;
		_actionsValidator = actionsValidator;
		_recurringTimeValidator = recurringTimeValidator;
		_validatorFunctions = new ValidatorFunction[7] { ValidateName, ValidateStartTriggerType, ValidateStartConditions, ValidateStartEvent, ValidateStopEvent, ValidateRecurringTime, ValidateActions };
	}

	public ValidationError[] Validate(IGeminiRequestContext requestContext, RuleModel model)
	{
		ValidatorFunction[] validatorFunctions = _validatorFunctions;
		for (int i = 0; i < validatorFunctions.Length; i++)
		{
			ValidationError[] array = validatorFunctions[i](requestContext, model);
			if (array != null && array.Any())
			{
				return array;
			}
		}
		return new ValidationError[0];
	}

	private ValidationError[] ValidateName(IGeminiRequestContext requestContext, RuleModel model)
	{
		if (string.IsNullOrEmpty(model.Name))
		{
			return new ValidationError[1]
			{
				new ValidationError("Name cannot be null or empty")
				{
					PropertyName = "Name"
				}
			};
		}
		return null;
	}

	private ValidationError[] ValidateStartTriggerType(IGeminiRequestContext requestContext, RuleModel model)
	{
		if (model.RuleStart.TriggerType == TriggerType.None || model.RuleStart.TriggerType == TriggerType.Timer)
		{
			return new ValidationError[1]
			{
				new ValidationError("Start trigger type <{0}> is not valid!", model.RuleStart.TriggerType)
				{
					PropertyName = "StartRuleType"
				}
			};
		}
		if (model.RuleStop.TriggerType == TriggerType.Timer && model.RuleStart.TriggerType != TriggerType.Event)
		{
			return new ValidationError[1] { InvalidStopTriggerForStartTrigger(model) };
		}
		if (model.RuleStart.TriggerType == TriggerType.RecurringTime && model.RuleStop.TriggerType != TriggerType.None)
		{
			return new ValidationError[1] { InvalidStopTriggerForStartTrigger(model) };
		}
		if (model.RuleStop.TriggerType != TriggerType.None && model.RuleStop.TriggerType != TriggerType.Timer && model.RuleStop.TriggerType != model.RuleStart.TriggerType)
		{
			return new ValidationError[1] { InvalidStopTriggerForStartTrigger(model) };
		}
		if (model.RuleStop.TriggerType == TriggerType.Timer && model.RuleStop.Delay <= 0)
		{
			return new ValidationError[1]
			{
				new ValidationError("Time: <{0}> for stop trigger <{1}> must be greater than zero!", model.RuleStop.Delay, model.RuleStop.TriggerType)
				{
					PropertyName = "TimerTime"
				}
			};
		}
		return null;
	}

	private static ValidationError InvalidStopTriggerForStartTrigger(RuleModel model)
	{
		return new ValidationError("Stop trigger type <{0}> is not valid for start trigger type <{1}>!", model.RuleStop.TriggerType, model.RuleStart.TriggerType)
		{
			PropertyName = "StopRuleType"
		};
	}

	private ValidationError[] ValidateStartConditions(IGeminiRequestContext requestContext, RuleModel model)
	{
		if (model.RuleStart.Conditions.Keys.Any((ConditionType s) => s == ConditionType.None))
		{
			return new ValidationError[1]
			{
				new ValidationError("Unsupported condition detected!")
			};
		}
		if (model.RuleStart.TriggerType == TriggerType.TimeInterval && !model.RuleStart.Conditions.Any())
		{
			return new ValidationError[1]
			{
				new ValidationError("Please provide at least one time condition for the time rule.")
			};
		}
		List<ValidationError> list = new List<ValidationError>();
		foreach (ConditionModel value in model.RuleStart.Conditions.Values)
		{
			list.AddRange(_conditionsValidator.Validate(requestContext, value));
		}
		return list.ToArray();
	}

	private ValidationError[] ValidateStartEvent(IGeminiRequestContext requestContext, RuleModel model)
	{
		return _eventValidator.Validate(requestContext, model.RuleStart);
	}

	private ValidationError[] ValidateStopEvent(IGeminiRequestContext requestContext, RuleModel model)
	{
		return _eventValidator.Validate(requestContext, model.RuleStop);
	}

	private ValidationError[] ValidateActions(IGeminiRequestContext requestContext, RuleModel model)
	{
		return _actionsValidator.Validate(requestContext, model);
	}

	private ValidationError[] ValidateRecurringTime(IGeminiRequestContext requestContext, RuleModel model)
	{
		return _recurringTimeValidator.Validate(requestContext, model);
	}
}
