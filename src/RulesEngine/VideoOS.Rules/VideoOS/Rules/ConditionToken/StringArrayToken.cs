using System;
using System.Diagnostics;

namespace VideoOS.Rules.ConditionToken;

[DebuggerDisplay("StringArrayToken: Count: {Value.Length}")]
public class StringArrayToken : ValueToken
{
	public string[] Value { get; set; }

	public StringArrayToken()
	{
		Value = Array.Empty<string>();
	}
}
