using System;

namespace VideoOS.EventServer.Shared.WebServiceCommandFaultException.Map;

[Serializable]
public class InsufficientRightsCommandFaultException : MapCommandFaultException
{
	public InsufficientRightsCommandFaultException()
		: base("Insufficient Rights", MapCommandFaultExceptionErrorReason.InsufficientRights)
	{
	}

	public InsufficientRightsCommandFaultException(string errorDescription)
		: base(errorDescription, MapCommandFaultExceptionErrorReason.InsufficientRights)
	{
	}

	public InsufficientRightsCommandFaultException(string errorDescription, Exception innerException)
		: base(errorDescription, MapCommandFaultExceptionErrorReason.InsufficientRights, innerException)
	{
	}
}
