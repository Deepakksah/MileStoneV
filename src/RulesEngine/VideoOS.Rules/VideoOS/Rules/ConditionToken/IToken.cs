namespace VideoOS.Rules.ConditionToken;

public interface IToken
{
	TokenType Type { get; }

	int Precedence { get; }
}
