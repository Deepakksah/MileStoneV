using System;

namespace VideoOS.EventServer.Shared.WebServiceCommandFaultException.Map;

[Serializable]
public class InvalidIdCommandFaultException : MapCommandFaultException
{
	public InvalidIdCommandFaultException()
		: base("Invalid Id", MapCommandFaultExceptionErrorReason.InvalidId)
	{
	}

	public InvalidIdCommandFaultException(string errorDescription)
		: base(errorDescription, MapCommandFaultExceptionErrorReason.InvalidId)
	{
	}

	public InvalidIdCommandFaultException(string errorDescription, Exception innerException)
		: base(errorDescription, MapCommandFaultExceptionErrorReason.InvalidId, innerException)
	{
	}
}
