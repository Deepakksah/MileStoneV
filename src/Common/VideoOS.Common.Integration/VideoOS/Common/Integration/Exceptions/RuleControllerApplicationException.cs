using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Exceptions;

[Serializable]
public class RuleControllerApplicationException : CorporateApplicationException
{
	public RuleControllerApplicationException()
	{
	}

	public RuleControllerApplicationException(string message)
		: base(message)
	{
	}

	public RuleControllerApplicationException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected RuleControllerApplicationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
