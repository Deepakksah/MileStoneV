using System;

namespace VideoOS.EventServer.Shared.WebServiceCommandFaultException;

[Serializable]
public class InvalidAuthenticationCommandFaultException : CommonCommandFaultException
{
	public InvalidAuthenticationCommandFaultException()
		: base("Invalid authentication", CommonCommandFaultExceptionErrorReason.InvalidAuthentication)
	{
	}

	public InvalidAuthenticationCommandFaultException(string errorDescription)
		: base(errorDescription, CommonCommandFaultExceptionErrorReason.InvalidAuthentication)
	{
	}

	public InvalidAuthenticationCommandFaultException(string errorDescription, Exception innerException)
		: base(errorDescription, CommonCommandFaultExceptionErrorReason.InvalidAuthentication, innerException)
	{
	}
}
