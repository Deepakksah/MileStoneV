using System;

namespace VideoOS.EventServer.Shared.WebServiceCommandFaultException;

[Serializable]
public class ReloadRequiredCommandFaultException : CommonCommandFaultException
{
	public ReloadRequiredCommandFaultException()
		: base("Reload Required", CommonCommandFaultExceptionErrorReason.ReloadRequired)
	{
	}

	public ReloadRequiredCommandFaultException(string errorDescription)
		: base(errorDescription, CommonCommandFaultExceptionErrorReason.ReloadRequired)
	{
	}

	public ReloadRequiredCommandFaultException(string errorDescription, Exception innerException)
		: base(errorDescription, CommonCommandFaultExceptionErrorReason.ReloadRequired, innerException)
	{
	}
}
