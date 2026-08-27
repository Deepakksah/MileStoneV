namespace VideoOS.EventServer.Shared.WebServiceCommandFaultException;

public abstract class CommonCommandFaultExceptionErrorReason
{
	private static string _unknown = "Unknown";

	private static string _invalidAuthentication = "InvalidAuthentication";

	private static string _invalidTimeout = "InvalidTimeout";

	private static string _reloadRequired = "ReloadRequired";

	public static string Unknown => _unknown;

	public static string InvalidAuthentication => _invalidAuthentication;

	public static string InvalidTimeout => _invalidTimeout;

	public static string ReloadRequired => _reloadRequired;
}
