using System;
using System.Collections;
using System.Diagnostics;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Rules.ConditionToken;

[DebuggerDisplay("GetVariableFunctionToken")]
public class GetVariableFunctionToken : FunctionToken, IEvaluate
{
	private static GetVariableFunctionToken _instance = new GetVariableFunctionToken();

	public static GetVariableFunctionToken Instance => _instance;

	private GetVariableFunctionToken()
	{
	}

	public void Evaluate(TriggerEvent triggerEvent, Stack resultStack)
	{
		StringValueToken stringValueToken = (StringValueToken)resultStack.Pop();
		StringValueToken stringValueToken2 = (StringValueToken)resultStack.Pop();
		string variableValue = ((IRuleEngine)((ObjectValueToken)resultStack.Pop()).Value).RuleVariableManager.GetVariableValue(new Guid(stringValueToken2.Value), new Guid(stringValueToken.Value));
		ValueToken valueToken = null;
		valueToken = (string.IsNullOrEmpty(variableValue) ? new NullValueToken() : ((!int.TryParse(variableValue, out var result)) ? ((ValueToken)new StringValueToken(variableValue)) : ((ValueToken)new IntegerValueToken(result))));
		resultStack.Push(valueToken);
	}
}
