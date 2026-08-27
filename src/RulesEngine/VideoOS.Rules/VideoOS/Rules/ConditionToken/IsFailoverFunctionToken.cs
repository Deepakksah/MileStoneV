using System.Collections;
using System.Diagnostics;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Rules.ConditionToken;

[DebuggerDisplay("IsFailoverFunctionToken")]
public class IsFailoverFunctionToken : FunctionToken, IEvaluate
{
	private static IsFailoverFunctionToken _instance = new IsFailoverFunctionToken();

	public static IsFailoverFunctionToken Instance => _instance;

	private IsFailoverFunctionToken()
	{
	}

	public void Evaluate(TriggerEvent triggerEvent, Stack resultStack)
	{
		BooleanValueToken obj = new BooleanValueToken(((IRuleEngine)((ObjectValueToken)resultStack.Pop()).Value).IsFailover);
		resultStack.Push(obj);
	}
}
