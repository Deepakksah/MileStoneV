using System;
using System.ServiceModel;
using VideoOS.EventServer.Communication.AccessControl.Client;
using VideoOS.EventServer.Shared.WebServiceCommandFaultException;
using VideoOS.EventServer.Shared.WebServiceCommandFaultException.AccessControl;

namespace VideoOS.EventServer.Communication.AccessControl;

public class AccessControlFaultExceptionHandler : CommonFaultExceptionHandler
{
	private AccessControlFaultExceptionHandler()
	{
	}

	public static CommonCommandFaultException HandleException(FaultException<AccessControlServiceFault> error)
	{
		if (error == null)
		{
			throw new ArgumentException("Argument is null", "error");
		}
		return new AccessControlFaultExceptionHandler().FindCorrectException(error.Detail.ErrorDescription, error.Detail.ErrorReason);
	}

	protected override CommonCommandFaultException FindCorrectException(string description, string reason)
	{
		if (reason == AccessControlCommandFaultExceptionErrorReason.PersonalizedConfigurationLoadError)
		{
			return new PersonalizedConfigurationLoadErrorCommandFaultException(description);
		}
		if (reason == AccessControlCommandFaultExceptionErrorReason.UseAccessControlError)
		{
			return new UseAccessControlErrorCommandFaultException(description);
		}
		return base.FindCorrectException(description, reason);
	}
}
