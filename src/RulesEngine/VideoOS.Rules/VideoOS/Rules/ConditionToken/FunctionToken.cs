namespace VideoOS.Rules.ConditionToken;

public abstract class FunctionToken : IToken
{
	protected const int _baseFunctionPrecedence = 1000;

	public TokenType Type => TokenType.Function;

	public int Precedence => 1000;

	public FunctionToken()
	{
	}
}
