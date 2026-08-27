using System.Diagnostics;

namespace VideoOS.Rules.ConditionToken;

[DebuggerDisplay("BooleanValueToken: {Value}")]
public class BooleanValueToken : ValueToken
{
	public bool Value { get; set; }

	public BooleanValueToken()
	{
		Value = false;
	}

	public BooleanValueToken(bool value)
	{
		Value = value;
	}
}
