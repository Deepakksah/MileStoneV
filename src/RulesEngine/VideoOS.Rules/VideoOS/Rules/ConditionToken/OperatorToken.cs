namespace VideoOS.Rules.ConditionToken;

public abstract class OperatorToken : IToken
{
	protected const int _baseOperatorPrecedence = 10;

	public TokenType Type => TokenType.Operator;

	public abstract int Precedence { get; }

	public OperatorToken()
	{
	}
}
