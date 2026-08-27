namespace VideoOS.ServerConfigurator.CertificateRegistration;

public static class StatusCodeExtensions
{
	public static bool IsSuccess(this StatusCode statusCode)
	{
		return statusCode == StatusCode.Success;
	}

	public static bool IsWarning(this StatusCode statusCode)
	{
		return statusCode == StatusCode.ServiceRegistrationNotFound;
	}
}
