using System.Diagnostics;

namespace VideoOS.Rules.ConditionToken;

[DebuggerDisplay("DoubleValueToken: {Value}")]
public class DoubleValueToken : ValueToken
{
	public double Value { get; set; }

	public DoubleValueToken()
	{
		Value = 0.0;
	}

	public DoubleValueToken(double value)
	{
		Value = value;
	}
}
