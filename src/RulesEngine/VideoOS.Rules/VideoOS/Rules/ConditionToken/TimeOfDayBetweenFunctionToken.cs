using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Rules.ConditionToken;

[DebuggerDisplay("TimeOfDayBetweenFunctionToken")]
public class TimeOfDayBetweenFunctionToken : FunctionToken, IEvaluate
{
	private static TimeOfDayBetweenFunctionToken _instance = new TimeOfDayBetweenFunctionToken();

	public static TimeOfDayBetweenFunctionToken Instance => _instance;

	private TimeOfDayBetweenFunctionToken()
	{
	}

	public void Evaluate(TriggerEvent triggerEvent, Stack resultStack)
	{
		StringValueToken obj = (StringValueToken)resultStack.Pop();
		TimeSpan timeSpan = TimeSpan.Parse(((StringValueToken)resultStack.Pop()).Value, CultureInfo.InvariantCulture);
		TimeSpan timeSpan2 = TimeSpan.Parse(obj.Value, CultureInfo.InvariantCulture);
		BooleanValueToken booleanValueToken = new BooleanValueToken();
		DateTime dateTime = triggerEvent.Time.ToLocalTime();
		if (timeSpan < timeSpan2)
		{
			booleanValueToken.Value = dateTime.TimeOfDay >= timeSpan && dateTime.TimeOfDay < timeSpan2;
		}
		else
		{
			booleanValueToken.Value = dateTime.TimeOfDay >= timeSpan || dateTime.TimeOfDay < timeSpan2;
		}
		resultStack.Push(booleanValueToken);
	}
}
