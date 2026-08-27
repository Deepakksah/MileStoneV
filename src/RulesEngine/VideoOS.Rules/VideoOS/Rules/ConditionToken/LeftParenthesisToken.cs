using System.Diagnostics;

namespace VideoOS.Rules.ConditionToken;

[DebuggerDisplay("LeftParenthesisToken")]
public class LeftParenthesisToken : ParenthesisToken
{
	private static LeftParenthesisToken _instance = new LeftParenthesisToken();

	public static LeftParenthesisToken Instance => _instance;

	private LeftParenthesisToken()
	{
	}
}
