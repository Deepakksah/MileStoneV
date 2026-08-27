using System;

namespace VideoOS.EventServer.Shared.WebServiceCommandFaultException.AccessControl;

[Serializable]
public class UseAccessControlErrorCommandFaultException : AccessControlCommandFaultException
{
	public UseAccessControlErrorCommandFaultException()
		: base("User is not allowed to use Access Control", AccessControlCommandFaultExceptionErrorReason.UseAccessControlError)
	{
	}

	public UseAccessControlErrorCommandFaultException(string errorDescription)
		: base(errorDescription, AccessControlCommandFaultExceptionErrorReason.UseAccessControlError)
	{
	}

	public UseAccessControlErrorCommandFaultException(string errorDescription, Exception innerException)
		: base(errorDescription, AccessControlCommandFaultExceptionErrorReason.UseAccessControlError, innerException)
	{
	}
}
