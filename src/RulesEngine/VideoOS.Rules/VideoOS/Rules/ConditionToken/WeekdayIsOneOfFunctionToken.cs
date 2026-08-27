using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Rules.ConditionToken;

[DebuggerDisplay("WeekdayIsOneOfFunctionToken")]
public class WeekdayIsOneOfFunctionToken : FunctionToken, IEvaluate
{
	private static WeekdayIsOneOfFunctionToken _instance = new WeekdayIsOneOfFunctionToken();

	public static WeekdayIsOneOfFunctionToken Instance => _instance;

	private WeekdayIsOneOfFunctionToken()
	{
	}

	public void Evaluate(TriggerEvent triggerEvent, Stack resultStack)
	{
		ValueToken valueToken = (ValueToken)resultStack.Pop();
		BooleanValueToken booleanValueToken = new BooleanValueToken(value: false);
		if (valueToken is IntegerArrayToken)
		{
			IntegerArrayToken integerArrayToken = (IntegerArrayToken)valueToken;
			for (int i = 0; i < integerArrayToken.Value.Length; i++)
			{
				DayOfWeek dayOfWeek = ParseDayOfWeek(integerArrayToken.Value[i]);
				booleanValueToken.Value |= triggerEvent.Time.ToLocalTime().DayOfWeek == dayOfWeek;
			}
		}
		else
		{
			StringArrayToken stringArrayToken = (StringArrayToken)valueToken;
			for (int j = 0; j < stringArrayToken.Value.Length; j++)
			{
				DayOfWeek dayOfWeek = ParseDayOfWeek(stringArrayToken.Value[j]);
				booleanValueToken.Value |= triggerEvent.Time.ToLocalTime().DayOfWeek == dayOfWeek;
			}
		}
		resultStack.Push(booleanValueToken);
	}

	private static DayOfWeek ParseDayOfWeek(string day)
	{
		return day.ToLower(CultureInfo.InvariantCulture) switch
		{
			"sunday" => DayOfWeek.Sunday, 
			"monday" => DayOfWeek.Monday, 
			"tuesday" => DayOfWeek.Tuesday, 
			"wednesday" => DayOfWeek.Wednesday, 
			"thursday" => DayOfWeek.Thursday, 
			"friday" => DayOfWeek.Friday, 
			"saturday" => DayOfWeek.Saturday, 
			_ => throw new ArgumentException("Unable to convert string parameter to a weekday"), 
		};
	}

	private static DayOfWeek ParseDayOfWeek(int day)
	{
		return day switch
		{
			0 => DayOfWeek.Sunday, 
			1 => DayOfWeek.Monday, 
			2 => DayOfWeek.Tuesday, 
			3 => DayOfWeek.Wednesday, 
			4 => DayOfWeek.Thursday, 
			5 => DayOfWeek.Friday, 
			6 => DayOfWeek.Saturday, 
			_ => throw new ArgumentException("Unable to convert string parameter to a weekday"), 
		};
	}
}
