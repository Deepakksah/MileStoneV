using System;
using System.Collections;
using System.Diagnostics;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Rules.ConditionToken;

[DebuggerDisplay("NotWithinTimeProfileFunctionToken")]
public class NotWithinTimeProfileFunctionToken : FunctionToken, IEvaluate
{
	private static NotWithinTimeProfileFunctionToken _instance = new NotWithinTimeProfileFunctionToken();

	public static NotWithinTimeProfileFunctionToken Instance => _instance;

	private NotWithinTimeProfileFunctionToken()
	{
	}

	public void Evaluate(TriggerEvent triggerEvent, Stack resultStack)
	{
		ObjectValueToken obj = (ObjectValueToken)resultStack.Pop();
		StringValueToken stringValueToken = (StringValueToken)resultStack.Pop();
		BooleanValueToken booleanValueToken = new BooleanValueToken();
		booleanValueToken.Value = false;
		IRuleEngine ruleEngine = (IRuleEngine)obj.Value;
		booleanValueToken.Value = !ruleEngine.TimeProfileConfiguration.EvaluateTimeProfile(new Guid(stringValueToken.Value), triggerEvent.Time);
		resultStack.Push(booleanValueToken);
	}
}
