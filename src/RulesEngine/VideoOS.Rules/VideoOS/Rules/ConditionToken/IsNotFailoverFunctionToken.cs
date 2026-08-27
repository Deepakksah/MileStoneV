using System.Collections;
using System.Diagnostics;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Rules.ConditionToken;

[DebuggerDisplay("IsNotFailoverFunctionToken")]
public class IsNotFailoverFunctionToken : FunctionToken, IEvaluate
{
	private static IsNotFailoverFunctionToken _instance = new IsNotFailoverFunctionToken();

	public static IsNotFailoverFunctionToken Instance => _instance;

	private IsNotFailoverFunctionToken()
	{
	}

	public void Evaluate(TriggerEvent triggerEvent, Stack resultStack)
	{
		BooleanValueToken obj = new BooleanValueToken(!((IRuleEngine)((ObjectValueToken)resultStack.Pop()).Value).IsFailover);
		resultStack.Push(obj);
	}
}
