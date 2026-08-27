using System;
using System.Collections;
using System.Diagnostics;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Rules.ConditionToken;

[DebuggerDisplay("WithinTimeProfileFunctionToken")]
public class WithinTimeProfileFunctionToken : FunctionToken, IEvaluate
{
	private static WithinTimeProfileFunctionToken _instance = new WithinTimeProfileFunctionToken();

	public static WithinTimeProfileFunctionToken Instance => _instance;

	private WithinTimeProfileFunctionToken()
	{
	}

	public void Evaluate(TriggerEvent triggerEvent, Stack resultStack)
	{
		ObjectValueToken obj = (ObjectValueToken)resultStack.Pop();
		StringValueToken stringValueToken = (StringValueToken)resultStack.Pop();
		BooleanValueToken booleanValueToken = new BooleanValueToken();
		booleanValueToken.Value = false;
		IRuleEngine ruleEngine = (IRuleEngine)obj.Value;
		booleanValueToken.Value = ruleEngine.TimeProfileConfiguration.EvaluateTimeProfile(new Guid(stringValueToken.Value), triggerEvent.Time);
		resultStack.Push(booleanValueToken);
	}
}
