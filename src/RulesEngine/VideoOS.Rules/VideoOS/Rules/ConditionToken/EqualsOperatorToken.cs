using System.Collections;
using System.Diagnostics;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Rules.ConditionToken;

[DebuggerDisplay("EqualsOperatorToken")]
public class EqualsOperatorToken : OperatorToken, IEvaluate
{
	private const int _precedence = 13;

	private static EqualsOperatorToken _instance = new EqualsOperatorToken();

	public static EqualsOperatorToken Instance => _instance;

	public override int Precedence => 13;

	private EqualsOperatorToken()
	{
	}

	public void Evaluate(TriggerEvent triggerEvent, Stack resultStack)
	{
		IToken token = (IToken)resultStack.Pop();
		IToken token2 = (IToken)resultStack.Pop();
		bool value = false;
		if (token.GetType() == token2.GetType())
		{
			if (token is BooleanValueToken)
			{
				BooleanValueToken obj = (BooleanValueToken)token;
				BooleanValueToken booleanValueToken = (BooleanValueToken)token2;
				value = obj.Value == booleanValueToken.Value;
			}
			else if (token is StringValueToken)
			{
				StringValueToken obj2 = (StringValueToken)token;
				StringValueToken stringValueToken = (StringValueToken)token2;
				value = obj2.Value == stringValueToken.Value;
			}
			else if (token is IntegerValueToken)
			{
				IntegerValueToken obj3 = (IntegerValueToken)token;
				IntegerValueToken integerValueToken = (IntegerValueToken)token2;
				value = obj3.Value == integerValueToken.Value;
			}
			else if (token is DoubleValueToken)
			{
				DoubleValueToken obj4 = (DoubleValueToken)token;
				DoubleValueToken doubleValueToken = (DoubleValueToken)token2;
				value = obj4.Value == doubleValueToken.Value;
			}
			else
			{
				if (!(token is NullValueToken))
				{
					throw new InvalidRuleException("Unexpected token popped from stack during evaluation of rule condition");
				}
				value = true;
			}
		}
		BooleanValueToken booleanValueToken2 = new BooleanValueToken();
		booleanValueToken2.Value = value;
		resultStack.Push(booleanValueToken2);
	}
}
