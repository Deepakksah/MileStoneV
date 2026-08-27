using System;
using System.Runtime.Serialization;

namespace VideoOS.Installer.RecorderTaskPlugin;

[Serializable]
public class RecorderCertificateRegistrationException : Exception
{
	public RecorderCertificateRegistrationException()
	{
	}

	public RecorderCertificateRegistrationException(string message)
		: base(message)
	{
	}

	public RecorderCertificateRegistrationException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected RecorderCertificateRegistrationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
