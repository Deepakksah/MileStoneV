using System.Collections;
using System.Diagnostics;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Rules.ConditionToken;

[DebuggerDisplay("AndOperatorToken")]
public class AndOperatorToken : OperatorToken, IEvaluate
{
	private const int _precedence = 12;

	private static AndOperatorToken _instance = new AndOperatorToken();

	public static AndOperatorToken Instance => _instance;

	public override int Precedence => 12;

	private AndOperatorToken()
	{
	}

	public void Evaluate(TriggerEvent triggerEvent, Stack resultStack)
	{
		BooleanValueToken booleanValueToken = (BooleanValueToken)resultStack.Pop();
		BooleanValueToken booleanValueToken2 = (BooleanValueToken)resultStack.Pop();
		BooleanValueToken booleanValueToken3 = new BooleanValueToken();
		booleanValueToken3.Value = booleanValueToken.Value && booleanValueToken2.Value;
		resultStack.Push(booleanValueToken3);
	}
}
