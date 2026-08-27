namespace VideoOS.EventServer.Shared.WebServiceCommandFaultException.AccessControl;

public sealed class AccessControlCommandFaultExceptionErrorReason : CommonCommandFaultExceptionErrorReason
{
	private static string _personalizedConfigurationLoadError = "PersonalizedConfigurationLoadError";

	private static string _useAccessControlError = "UseAccessControlError";

	public static string PersonalizedConfigurationLoadError => _personalizedConfigurationLoadError;

	public static string UseAccessControlError => _useAccessControlError;

	private AccessControlCommandFaultExceptionErrorReason()
	{
	}
}
