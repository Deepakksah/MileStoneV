using System;

namespace VideoOS.EventServer.Shared.WebServiceCommandFaultException;

[Serializable]
public class InvalidTimeoutCommandFaultException : CommonCommandFaultException
{
	public InvalidTimeoutCommandFaultException()
		: base("Invalid timeout", CommonCommandFaultExceptionErrorReason.InvalidTimeout)
	{
	}

	public InvalidTimeoutCommandFaultException(string errorDescription)
		: base(errorDescription, CommonCommandFaultExceptionErrorReason.InvalidTimeout)
	{
	}

	public InvalidTimeoutCommandFaultException(string errorDescription, Exception innerException)
		: base(errorDescription, CommonCommandFaultExceptionErrorReason.InvalidTimeout, innerException)
	{
	}
}
