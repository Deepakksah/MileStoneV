using VideoOS.ServerConfigurator.CertificateRegistration;

namespace VideoOS.ServerConfigurator.RegistrationContainer;

public static class StatusCodeExtensions
{
	public static Status FromContainerStatus(this StatusContainer status)
	{
		return Status.ErrorStatus(status?.StatusCode ?? StatusCode.UnspecifiedErrorRegistration);
	}

	public static StatusContainer ToContainerStatus(this Status status)
	{
		return StatusContainer.ErrorStatus(status?.StatusCode ?? StatusCode.UnspecifiedErrorRegistration);
	}
}
