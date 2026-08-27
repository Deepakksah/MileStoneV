using System.Diagnostics;

namespace VideoOS.Rules.ConditionToken;

[DebuggerDisplay("RightParenthesisToken")]
public class RightParenthesisToken : ParenthesisToken
{
	private static RightParenthesisToken _instance = new RightParenthesisToken();

	public static RightParenthesisToken Instance => _instance;

	private RightParenthesisToken()
	{
	}
}
