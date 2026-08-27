using System;

namespace VideoOS.EventServer.Shared.WebServiceCommandFaultException.Map;

[Serializable]
public class ReferenceExistsCommandFaultException : MapCommandFaultException
{
	public ReferenceExistsCommandFaultException()
		: base("Reference Exists", MapCommandFaultExceptionErrorReason.ReferenceExists)
	{
	}

	public ReferenceExistsCommandFaultException(string errorDescription)
		: base(errorDescription, MapCommandFaultExceptionErrorReason.ReferenceExists)
	{
	}

	public ReferenceExistsCommandFaultException(string errorDescription, Exception innerException)
		: base(errorDescription, MapCommandFaultExceptionErrorReason.ReferenceExists, innerException)
	{
	}
}
