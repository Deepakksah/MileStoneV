using System;

namespace VideoOS.EventServer.Shared.WebServiceCommandFaultException.Map;

[Serializable]
public class TimestampCommandFaultException : MapCommandFaultException
{
	public TimestampCommandFaultException()
		: base("Timestamp", MapCommandFaultExceptionErrorReason.Timestamp)
	{
	}

	public TimestampCommandFaultException(string errorDescription)
		: base(errorDescription, MapCommandFaultExceptionErrorReason.Timestamp)
	{
	}

	public TimestampCommandFaultException(string errorDescription, Exception innerException)
		: base(errorDescription, MapCommandFaultExceptionErrorReason.Timestamp, innerException)
	{
	}
}
