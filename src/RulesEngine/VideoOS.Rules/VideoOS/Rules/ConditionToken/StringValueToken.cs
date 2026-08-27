using System.Diagnostics;

namespace VideoOS.Rules.ConditionToken;

[DebuggerDisplay("StringValueToken: {Value}")]
public class StringValueToken : ValueToken
{
	public string Value { get; set; }

	public StringValueToken()
	{
		Value = string.Empty;
	}

	public StringValueToken(string value)
	{
		Value = value;
	}
}
