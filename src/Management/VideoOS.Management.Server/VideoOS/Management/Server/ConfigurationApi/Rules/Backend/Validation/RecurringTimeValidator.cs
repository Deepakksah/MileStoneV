using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;
using VideoOS.Management.Server.ConfigurationApi.Rules.Builders;
using VideoOS.Rules.Interfaces;
using VideoOS.Utilities.Validation;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Validation;

public class RecurringTimeValidator : IValidator<RuleModel>
{
	private class FrequencyTest
	{
		public int MinValue { get; }

		public int MaxValue { get; }

		public string Property { get; }

		public FrequencyTest(int minValue, int maxValue, string property)
		{
			MinValue = minValue;
			MaxValue = maxValue;
			Property = property;
		}
	}

	private static readonly int _maxSecondsInDay = Convert.ToInt32(TimeSpan.FromHours(24.0).TotalSeconds);

	private static Dictionary<FrequencyType, FrequencyTest> _frequencyTests = new Dictionary<FrequencyType, FrequencyTest>
	{
		{
			FrequencyType.Daily,
			new FrequencyTest(1, 100, Constants.Properties.Frequency.Daily.Every)
		},
		{
			FrequencyType.Weekly,
			new FrequencyTest(1, 100, Constants.Properties.Frequency.Weekly.Every)
		},
		{
			FrequencyType.Monthly,
			new FrequencyTest(1, 99, Constants.Properties.Frequency.Monthly.Every)
		},
		{
			FrequencyType.MonthlyRelative,
			new FrequencyTest(1, 99, Constants.Properties.Frequency.MonthlyRelative.Every)
		}
	};

	private static Dictionary<FrequencyType, Func<FrequencyModel, ValidationError[]>> _parameterTests = new Dictionary<FrequencyType, Func<FrequencyModel, ValidationError[]>>
	{
		{
			FrequencyType.Daily,
			NoParameterTest
		},
		{
			FrequencyType.Weekly,
			WeeklyParameterTest
		},
		{
			FrequencyType.Monthly,
			MonthlyParameterTest
		},
		{
			FrequencyType.MonthlyRelative,
			MonthlyRelativeParameterTest
		}
	};

	public ValidationError[] Validate(IGeminiRequestContext requestContext, RuleModel model)
	{
		model.NullCheck("", "Validate", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\Rules\\Backend\\Validation\\Common\\RecurringTimeValidator.cs", 37);
		model.RuleStart.NullCheck("", "Validate", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\Rules\\Backend\\Validation\\Common\\RecurringTimeValidator.cs", 38);
		if (model.RuleStart.TriggerType != TriggerType.RecurringTime)
		{
			return new ValidationError[0];
		}
		RecurringTimeModel recurringTime = model.RuleStart.RecurringTime;
		recurringTime.NullCheck("", "Validate", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\Rules\\Backend\\Validation\\Common\\RecurringTimeValidator.cs", 44);
		recurringTime.Frequency.NullCheck("", "Validate", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\Rules\\Backend\\Validation\\Common\\RecurringTimeValidator.cs", 45);
		recurringTime.SubDay.NullCheck("", "Validate", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\Rules\\Backend\\Validation\\Common\\RecurringTimeValidator.cs", 46);
		RulePartModel ruleStop = model.RuleStop;
		if (ruleStop != null && ruleStop.TriggerType != TriggerType.None)
		{
			return new ValidationError[1]
			{
				new ValidationError("Recurring Time rule cannot have a stop rule.")
				{
					PropertyName = "StopRuleType"
				}
			};
		}
		ValidationError[] array = ValidateFrequencyModel(recurringTime.Frequency);
		if (array.Length != 0)
		{
			return array;
		}
		array = ValidateSubdayModel(recurringTime.SubDay);
		if (array.Length != 0)
		{
			return array;
		}
		return ValidateTimeConditions(model.RuleStart);
	}

	private static ValidationError[] ValidateTimeConditions(RulePartModel ruleStart)
	{
		ICollection<ConditionModel> obj = ruleStart.Conditions?.Values;
		obj.NullCheck("", "ValidateTimeConditions", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\Rules\\Backend\\Validation\\Common\\RecurringTimeValidator.cs", 76);
		if (obj.Any((ConditionModel x) => x.ConditionType != ConditionType.WithinTimeProfile && x.ConditionType != ConditionType.OutsideTimeProfile))
		{
			return new ValidationError[1]
			{
				new ValidationError("Only WithinTimeProfile and OutsideTimeProfile conditions are allowed for Recurring Time trigger.")
			};
		}
		return new ValidationError[0];
	}

	private static ValidationError[] ValidateSubdayModel(SubDayModel model)
	{
		return model.SubDayType switch
		{
			ScheduleTypes.SubdayType.Once => ValidateOnceSubday(model), 
			ScheduleTypes.SubdayType.Recurring => ValidateRecurringSubday(model), 
			ScheduleTypes.SubdayType.Undefined => new ValidationError[1]
			{
				new ValidationError("Subday type not specified.")
				{
					PropertyName = "Schedule.DailyFrequency",
					ValidProperty = GetSubdayModelProperties(model).First((Property p) => p.Key == "Schedule.DailyFrequency")
				}
			}, 
			_ => new ValidationError[0], 
		};
	}

	private static ValidationError[] ValidateOnceSubday(SubDayModel model)
	{
		if (model.StartTimeOfDay < 0 || model.StartTimeOfDay >= _maxSecondsInDay)
		{
			return new ValidationError[1]
			{
				new ValidationError("The time of day for Once must be between 00:00 and 23:59")
				{
					PropertyName = Constants.Properties.DailyFrequency.Once.StartTime,
					ValidProperty = GetDailyOnceSubdayProperties(model).First((Property p) => p.Key == Constants.Properties.DailyFrequency.Once.StartTime)
				}
			};
		}
		return new ValidationError[0];
	}

	private static ValidationError[] ValidateRecurringSubday(SubDayModel model)
	{
		List<ValidationError> list = new List<ValidationError>();
		if (model.StartTimeOfDay < 0 || model.StartTimeOfDay >= _maxSecondsInDay)
		{
			list.Add(new ValidationError("Start time must be between 00:00 and 23:59")
			{
				PropertyName = Constants.Properties.DailyFrequency.Recurring.StartTime,
				ValidProperty = GetDailyRecurringSubdayProperties(model).First((Property p) => p.Key == Constants.Properties.DailyFrequency.Recurring.StartTime)
			});
		}
		if (model.EndTimeOfDay < 0 || model.EndTimeOfDay >= _maxSecondsInDay || model.EndTimeOfDay < model.StartTimeOfDay)
		{
			list.Add(new ValidationError("End time must be between 00:00 and 23:59, and greater than start time.")
			{
				PropertyName = Constants.Properties.DailyFrequency.Recurring.EndTime,
				ValidProperty = GetDailyRecurringSubdayProperties(model).First((Property p) => p.Key == Constants.Properties.DailyFrequency.Recurring.EndTime)
			});
		}
		if (model.Frequency < 3600 || model.Frequency > _maxSecondsInDay || model.Frequency % 3600 != 0)
		{
			list.Add(new ValidationError("Daily frequency must be from 1 to 24 hours")
			{
				PropertyName = Constants.Properties.DailyFrequency.Recurring.Every,
				ValidProperty = GetDailyRecurringSubdayProperties(model).First((Property p) => p.Key == Constants.Properties.DailyFrequency.Recurring.Every)
			});
		}
		return list.ToArray();
	}

	private static ValidationError[] ValidateFrequencyModel(FrequencyModel model)
	{
		if (model.FrequencyType == FrequencyType.None)
		{
			return new ValidationError[1]
			{
				new ValidationError("Invalid frequency type specified.")
				{
					PropertyName = "Schedule.Frequency",
					ValidProperty = GetFrequencyModelProperties(model).First((Property p) => p.Key == "Schedule.Frequency")
				}
			};
		}
		if (HasFrequencyError(model, _frequencyTests[model.FrequencyType], out var error))
		{
			return new ValidationError[1] { error };
		}
		return _parameterTests[model.FrequencyType](model);
	}

	private static bool HasFrequencyError(FrequencyModel model, FrequencyTest test, out ValidationError error)
	{
		error = null;
		if (model.Frequency < test.MinValue || model.Frequency > test.MaxValue)
		{
			error = new ValidationError("Frequency must be between {0} and {1}", test.MinValue, test.MaxValue)
			{
				PropertyName = test.Property,
				ValidProperty = GetFrequencyProperties(model).First((Property p) => p.Key == test.Property)
			};
			return true;
		}
		return false;
	}

	private static ValidationError[] NoParameterTest(FrequencyModel model)
	{
		return new ValidationError[0];
	}

	private static ValidationError[] WeeklyParameterTest(FrequencyModel model)
	{
		if (!model.Parameters.TryGetValue("DayOfWeekIsOneOf", out var value) || value.IsEmpty)
		{
			return new ValidationError[1]
			{
				new ValidationError("Parameter Days is required for weekly frequency")
				{
					PropertyName = Constants.Properties.Frequency.Weekly.Days,
					ValidProperty = GetWeeklyProperties(model).First((Property p) => p.Key == Constants.Properties.Frequency.Weekly.Days)
				}
			};
		}
		List<ValidationError> list = new List<ValidationError>();
		string[] values = value.Values;
		foreach (string text in values)
		{
			if (!Enum.TryParse<DayOfWeek>(text, ignoreCase: true, out var _))
			{
				list.Add(new ValidationError("{0} is not a valid day", text)
				{
					PropertyName = Constants.Properties.Frequency.Weekly.Days
				});
			}
		}
		if (list.Count > 0)
		{
			list.First().ValidProperty = GetWeeklyProperties(model).First((Property p) => p.Key == Constants.Properties.Frequency.Weekly.Days);
		}
		return list.ToArray();
	}

	private static ValidationError[] MonthlyParameterTest(FrequencyModel model)
	{
		if (!model.Parameters.TryGetValue("AbsoluteDay", out var value) || value.IsEmpty)
		{
			return new ValidationError[1]
			{
				new ValidationError("Absolute day is a required parameter for a monthly schedule.")
				{
					PropertyName = Constants.Properties.Frequency.Monthly.Day,
					ValidProperty = GetMonthlyAbsoluteProperties(model).First((Property p) => p.Key == Constants.Properties.Frequency.Monthly.Day)
				}
			};
		}
		if (!int.TryParse(value.Value, out var result) || result < 1 || result > 31)
		{
			return new ValidationError[1]
			{
				new ValidationError("Absolute day must be a valid integer between 1 and 31")
				{
					PropertyName = Constants.Properties.Frequency.Monthly.Day
				}
			};
		}
		return new ValidationError[0];
	}

	private static ValidationError[] MonthlyRelativeParameterTest(FrequencyModel model)
	{
		if (!model.Parameters.TryGetValue("Offset", out var value) || value.IsEmpty)
		{
			return new ValidationError[1]
			{
				new ValidationError("Offset must be chosen for monthly relative recurring rule.")
				{
					PropertyName = Constants.Properties.Frequency.MonthlyRelative.Offset,
					ValidProperty = GetMonthlyRelativeProperties(model).First((Property p) => p.Key == Constants.Properties.Frequency.MonthlyRelative.Offset)
				}
			};
		}
		if (!model.Parameters.TryGetValue("RelativeDay", out var value2) || value2.IsEmpty)
		{
			return new ValidationError[1]
			{
				new ValidationError("Day must be chosen for monthly relative recurring rule.")
				{
					PropertyName = Constants.Properties.Frequency.MonthlyRelative.Day,
					ValidProperty = GetMonthlyRelativeProperties(model).First((Property p) => p.Key == Constants.Properties.Frequency.MonthlyRelative.Day)
				}
			};
		}
		if (!Enum.TryParse<ScheduleTypes.MonthlyRelativeOffset>(value.Value, ignoreCase: true, out var _))
		{
			return new ValidationError[1]
			{
				new ValidationError("Invalid offset. Valid values are First, Second, Third, Fourth, and Last.")
				{
					PropertyName = Constants.Properties.Frequency.MonthlyRelative.Offset
				}
			};
		}
		if (!Enum.TryParse<ScheduleTypes.MonthlyRelativeDay>(value2.Value, ignoreCase: true, out var _))
		{
			return new ValidationError[1]
			{
				new ValidationError("Invalid day value. Valid values are day names, Day, Weekday or Weekendday.")
				{
					PropertyName = Constants.Properties.Frequency.MonthlyRelative.Day
				}
			};
		}
		return new ValidationError[0];
	}

	private static List<Property> GetFrequencyModelProperties(FrequencyModel model)
	{
		List<Property> result = new List<Property>();
		ConfigurationItemRuleTypeBuilder.GetFrequencyTypeProperties(new RulePartModel
		{
			RecurringTime = new RecurringTimeModel
			{
				Frequency = model
			}
		}, result);
		return result;
	}

	private static List<Property> GetFrequencyProperties(FrequencyModel model)
	{
		return model.FrequencyType switch
		{
			FrequencyType.Daily => GetDailyProperties(model), 
			FrequencyType.Weekly => GetWeeklyProperties(model), 
			FrequencyType.Monthly => GetMonthlyAbsoluteProperties(model), 
			FrequencyType.MonthlyRelative => GetMonthlyRelativeProperties(model), 
			_ => new List<Property>(), 
		};
	}

	private static List<Property> GetDailyProperties(FrequencyModel model)
	{
		List<Property> result = new List<Property>();
		ConfigurationItemRuleTypeBuilder.GetDailyProperties(new RulePartModel
		{
			RecurringTime = new RecurringTimeModel
			{
				Frequency = model
			}
		}, result);
		return result;
	}

	private static List<Property> GetWeeklyProperties(FrequencyModel model)
	{
		List<Property> result = new List<Property>();
		ConfigurationItemRuleTypeBuilder.GetWeeklyProperties(new RulePartModel
		{
			RecurringTime = new RecurringTimeModel
			{
				Frequency = model
			}
		}, result);
		return result;
	}

	private static List<Property> GetMonthlyAbsoluteProperties(FrequencyModel model)
	{
		List<Property> result = new List<Property>();
		ConfigurationItemRuleTypeBuilder.GetMonthlyAbsoluteProperties(new RulePartModel
		{
			RecurringTime = new RecurringTimeModel
			{
				Frequency = model
			}
		}, result);
		return result;
	}

	private static List<Property> GetMonthlyRelativeProperties(FrequencyModel model)
	{
		List<Property> result = new List<Property>();
		ConfigurationItemRuleTypeBuilder.GetMonthlyRelativeProperties(new RulePartModel
		{
			RecurringTime = new RecurringTimeModel
			{
				Frequency = model
			}
		}, result);
		return result;
	}

	private static List<Property> GetSubdayModelProperties(SubDayModel model)
	{
		List<Property> result = new List<Property>();
		ConfigurationItemRuleTypeBuilder.GetSubDayTypeProperties(new RulePartModel
		{
			RecurringTime = new RecurringTimeModel
			{
				SubDay = model
			}
		}, result);
		return result;
	}

	private static List<Property> GetDailyRecurringSubdayProperties(SubDayModel model)
	{
		List<Property> result = new List<Property>();
		ConfigurationItemRuleTypeBuilder.GetDailyRecurringProperties(new RulePartModel
		{
			RecurringTime = new RecurringTimeModel
			{
				SubDay = model
			}
		}, result);
		return result;
	}

	private static List<Property> GetDailyOnceSubdayProperties(SubDayModel model)
	{
		List<Property> result = new List<Property>();
		ConfigurationItemRuleTypeBuilder.GetDailyOnceProperty(new RulePartModel
		{
			RecurringTime = new RecurringTimeModel
			{
				SubDay = model
			}
		}, result);
		return result;
	}
}
