using System.Diagnostics;

namespace VideoOS.Rules.ConditionToken;

[DebuggerDisplay("ObjectValueToken: {Value}")]
public class ObjectValueToken : ValueToken
{
	public object Value { get; set; }

	public ObjectValueToken()
	{
		Value = null;
	}

	public ObjectValueToken(object value)
	{
		Value = value;
	}
}
