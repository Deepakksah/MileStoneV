using System;
using System.Runtime.Serialization;

namespace VideoOS.Rules.Interfaces;

[Serializable]
public class RuleEngineApplicationException : Exception
{
	public RuleEngineApplicationException()
	{
	}

	public RuleEngineApplicationException(string message)
		: base(message)
	{
	}

	public RuleEngineApplicationException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected RuleEngineApplicationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
