using System;

namespace VideoOS.Rules.Interfaces;

[Serializable]
public class RuleConfigurationException : RuleException
{
	public RuleConfigurationException(string message)
		: base(message)
	{
	}

	public RuleConfigurationException(string message, Exception inner)
		: base(message, inner)
	{
	}
}
