using System;

namespace VideoOS.EventServer.Shared.WebServiceCommandFaultException.Map;

[Serializable]
public class InvalidServerIdCommandFaultException : MapCommandFaultException
{
	public InvalidServerIdCommandFaultException()
		: base("Invalid Server Id", MapCommandFaultExceptionErrorReason.InvalidServerId)
	{
	}

	public InvalidServerIdCommandFaultException(string errorDescription)
		: base(errorDescription, MapCommandFaultExceptionErrorReason.InvalidServerId)
	{
	}

	public InvalidServerIdCommandFaultException(string errorDescription, Exception innerException)
		: base(errorDescription, MapCommandFaultExceptionErrorReason.InvalidServerId, innerException)
	{
	}
}
