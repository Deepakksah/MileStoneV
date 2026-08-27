using System;
using VideoOS.ServerConfigurator.CertificateRegistration;

namespace VideoOS.ServerConfigurator.RegistrationContainer;

[Serializable]
public class StatusContainer
{
	public StatusCode StatusCode { get; }

	private StatusContainer(StatusCode statusCode)
	{
		StatusCode = statusCode;
	}

	public static StatusContainer SuccessStatus()
	{
		return new StatusContainer(StatusCode.Success);
	}

	public static StatusContainer ErrorStatus(StatusCode statusCode)
	{
		return new StatusContainer(statusCode);
	}
}
