using System;

namespace VideoOS.EventServer.Shared.WebServiceCommandFaultException.Map;

[Serializable]
public class InvalidBoundsCommandFaultException : MapCommandFaultException
{
	public InvalidBoundsCommandFaultException()
		: base("Invalid Bounds", MapCommandFaultExceptionErrorReason.InvalidBounds)
	{
	}

	public InvalidBoundsCommandFaultException(string errorDescription)
		: base(errorDescription, MapCommandFaultExceptionErrorReason.InvalidBounds)
	{
	}

	public InvalidBoundsCommandFaultException(string errorDescription, Exception innerException)
		: base(errorDescription, MapCommandFaultExceptionErrorReason.InvalidBounds, innerException)
	{
	}
}
