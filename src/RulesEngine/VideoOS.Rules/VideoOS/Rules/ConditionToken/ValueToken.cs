using System.Collections;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Rules.ConditionToken;

public abstract class ValueToken : IToken, IEvaluate
{
	public TokenType Type => TokenType.Value;

	public int Precedence => 0;

	public ValueToken()
	{
	}

	public void Evaluate(TriggerEvent triggerEvent, Stack resultStack)
	{
		resultStack.Push(this);
	}
}
