using System.Collections;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Rules.ConditionToken;

public class NotUnaryOperatorToken : UnaryOperatorToken, IEvaluate
{
	private const int _precedence = 101;

	private static NotUnaryOperatorToken _instance = new NotUnaryOperatorToken();

	public static NotUnaryOperatorToken Instance => _instance;

	public override int Precedence => 101;

	private NotUnaryOperatorToken()
	{
	}

	public void Evaluate(TriggerEvent triggerEvent, Stack resultStack)
	{
		BooleanValueToken booleanValueToken = (BooleanValueToken)resultStack.Pop();
		BooleanValueToken booleanValueToken2 = new BooleanValueToken();
		booleanValueToken2.Value = !booleanValueToken.Value;
		resultStack.Push(booleanValueToken2);
	}
}
