using System.Diagnostics;

namespace VideoOS.Rules.ConditionToken;

[DebuggerDisplay("IntegerValueToken: {Value}")]
public class IntegerValueToken : ValueToken
{
	public int Value { get; set; }

	public IntegerValueToken()
	{
		Value = 0;
	}

	public IntegerValueToken(int value)
	{
		Value = value;
	}
}
