using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Rules.ConditionToken;

[DebuggerDisplay("DayOfWeekBetweenFunctionToken")]
public class DayOfWeekBetweenFunctionToken : FunctionToken, IEvaluate
{
	private static DayOfWeekBetweenFunctionToken _instance = new DayOfWeekBetweenFunctionToken();

	public static DayOfWeekBetweenFunctionToken Instance => _instance;

	private DayOfWeekBetweenFunctionToken()
	{
	}

	public void Evaluate(TriggerEvent triggerEvent, Stack resultStack)
	{
		ValueToken valueToken = (ValueToken)resultStack.Pop();
		ValueToken valueToken2 = (ValueToken)resultStack.Pop();
		DayOfWeek dayOfWeek = ((!(valueToken2 is IntegerValueToken)) ? ParseDayOfWeek(((StringValueToken)valueToken2).Value) : ParseDayOfWeek(((IntegerValueToken)valueToken2).Value));
		DayOfWeek dayOfWeek2 = ((!(valueToken is IntegerValueToken)) ? ParseDayOfWeek(((StringValueToken)valueToken).Value) : ParseDayOfWeek(((IntegerValueToken)valueToken).Value));
		BooleanValueToken booleanValueToken = new BooleanValueToken();
		DateTime dateTime = triggerEvent.Time.ToLocalTime();
		if (dayOfWeek < dayOfWeek2)
		{
			booleanValueToken.Value = dateTime.DayOfWeek >= dayOfWeek && dateTime.DayOfWeek <= dayOfWeek2;
		}
		else
		{
			booleanValueToken.Value = dateTime.DayOfWeek >= dayOfWeek || dateTime.DayOfWeek <= dayOfWeek2;
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
