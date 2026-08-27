namespace VideoOS.Rules.ConditionToken;

public abstract class UnaryOperatorToken : IToken
{
	protected const int _baseUnaryOperatorPrecedence = 100;

	public TokenType Type => TokenType.Operator;

	public abstract int Precedence { get; }

	public UnaryOperatorToken()
	{
	}
}
