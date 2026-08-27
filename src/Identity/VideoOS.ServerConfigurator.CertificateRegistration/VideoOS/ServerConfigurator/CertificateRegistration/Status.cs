using System;

namespace VideoOS.ServerConfigurator.CertificateRegistration;

[Serializable]
public class Status
{
	public StatusCode StatusCode { get; }

	private Status(StatusCode statusCode)
	{
		StatusCode = statusCode;
	}

	public static Status SuccessStatus()
	{
		return new Status(StatusCode.Success);
	}

	public static Status ErrorStatus(StatusCode statusCode)
	{
		return new Status(statusCode);
	}
}
