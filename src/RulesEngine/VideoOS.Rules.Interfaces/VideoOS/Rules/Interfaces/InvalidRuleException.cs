using System;

namespace VideoOS.Rules.Interfaces;

[Serializable]
public class InvalidRuleException : RuleException
{
	public InvalidRuleException(string message)
		: base(message)
	{
	}
}
