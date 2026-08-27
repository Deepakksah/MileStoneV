namespace VideoOS.Rules.ConditionToken;

public class ParenthesisToken : IToken
{
	protected const int _baseParenthesisPrecedence = 100000;

	public TokenType Type => TokenType.Parenthesis;

	public int Precedence => 100000;

	public ParenthesisToken()
	{
	}
}
