using System;

namespace VideoOS.Rules.Interfaces;

[Serializable]
public class RuleException : Exception
{
	public RuleException()
	{
	}

	public RuleException(string message)
		: base(message)
	{
	}

	public RuleException(string message, Exception inner)
		: base(message, inner)
	{
	}
}
