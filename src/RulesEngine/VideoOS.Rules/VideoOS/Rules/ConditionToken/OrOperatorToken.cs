using System.Collections;
using System.Diagnostics;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Rules.ConditionToken;

[DebuggerDisplay("OrOperatorToken")]
public class OrOperatorToken : OperatorToken, IEvaluate
{
	private const int _precedence = 11;

	private static OrOperatorToken _instance = new OrOperatorToken();

	public static OrOperatorToken Instance => _instance;

	public override int Precedence => 11;

	private OrOperatorToken()
	{
	}

	public void Evaluate(TriggerEvent triggerEvent, Stack resultStack)
	{
		BooleanValueToken booleanValueToken = (BooleanValueToken)resultStack.Pop();
		BooleanValueToken booleanValueToken2 = (BooleanValueToken)resultStack.Pop();
		BooleanValueToken booleanValueToken3 = new BooleanValueToken();
		booleanValueToken3.Value = booleanValueToken.Value || booleanValueToken2.Value;
		resultStack.Push(booleanValueToken3);
	}
}
