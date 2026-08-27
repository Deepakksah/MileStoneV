using System;

namespace VideoOS.EventServer.Shared.WebServiceCommandFaultException.AccessControl;

[Serializable]
public class PersonalizedConfigurationLoadErrorCommandFaultException : AccessControlCommandFaultException
{
	public PersonalizedConfigurationLoadErrorCommandFaultException()
		: base("Error Loading Personalized Configuration", AccessControlCommandFaultExceptionErrorReason.PersonalizedConfigurationLoadError)
	{
	}

	public PersonalizedConfigurationLoadErrorCommandFaultException(string errorDescription)
		: base(errorDescription, AccessControlCommandFaultExceptionErrorReason.PersonalizedConfigurationLoadError)
	{
	}

	public PersonalizedConfigurationLoadErrorCommandFaultException(string errorDescription, Exception innerException)
		: base(errorDescription, AccessControlCommandFaultExceptionErrorReason.PersonalizedConfigurationLoadError, innerException)
	{
	}
}
