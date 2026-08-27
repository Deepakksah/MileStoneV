using VideoOS.EventServer.Shared.WebServiceCommandFaultException;

namespace VideoOS.EventServer.Communication;

public abstract class CommonFaultExceptionHandler
{
	protected virtual CommonCommandFaultException FindCorrectException(string description, string reason)
	{
		if (reason == CommonCommandFaultExceptionErrorReason.InvalidAuthentication)
		{
			return new InvalidAuthenticationCommandFaultException(description);
		}
		return new CommonCommandFaultException(description, reason);
	}
}
