using System;

namespace VideoOS.EventServer.Shared.WebServiceCommandFaultException.AccessControl;

[Serializable]
public class AccessControlCommandFaultException : CommonCommandFaultException
{
	public AccessControlCommandFaultException()
	{
	}

	public AccessControlCommandFaultException(string errorDescription)
		: base(errorDescription)
	{
	}

	public AccessControlCommandFaultException(string errorDescription, Exception innerException)
		: base(errorDescription, innerException)
	{
	}

	public AccessControlCommandFaultException(string errorDescription, string errorReason)
		: base(errorDescription, errorReason)
	{
	}

	public AccessControlCommandFaultException(string errorDescription, string errorReason, Exception innerException)
		: base(errorDescription, errorReason, innerException)
	{
	}
}
