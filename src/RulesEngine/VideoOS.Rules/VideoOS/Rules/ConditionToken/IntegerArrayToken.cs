using System;
using System.Diagnostics;

namespace VideoOS.Rules.ConditionToken;

[DebuggerDisplay("IntegerArrayToken: Count: {Value.Length}")]
public class IntegerArrayToken : ValueToken
{
	public int[] Value { get; set; }

	public IntegerArrayToken()
	{
		Value = Array.Empty<int>();
	}
}
