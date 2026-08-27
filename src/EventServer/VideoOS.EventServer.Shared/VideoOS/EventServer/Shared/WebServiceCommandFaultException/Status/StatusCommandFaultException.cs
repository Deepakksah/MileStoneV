using System;

namespace VideoOS.EventServer.Shared.WebServiceCommandFaultException.Status;

[Serializable]
public class StatusCommandFaultException : CommonCommandFaultException
{
	public StatusCommandFaultException()
	{
	}

	public StatusCommandFaultException(string errorDescription)
		: base(errorDescription)
	{
	}

	public StatusCommandFaultException(string errorDescription, Exception innerException)
		: base(errorDescription, innerException)
	{
	}

	public StatusCommandFaultException(string errorDescription, string errorReason)
		: base(errorDescription, errorReason)
	{
	}

	public StatusCommandFaultException(string errorDescription, string errorReason, Exception innerException)
		: base(errorDescription, errorReason, innerException)
	{
	}
}
