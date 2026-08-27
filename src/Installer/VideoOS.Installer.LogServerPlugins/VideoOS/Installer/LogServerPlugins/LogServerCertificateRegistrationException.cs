using System;
using System.Runtime.Serialization;

namespace VideoOS.Installer.LogServerPlugins;

[Serializable]
public class LogServerCertificateRegistrationException : Exception
{
	public LogServerCertificateRegistrationException()
	{
	}

	public LogServerCertificateRegistrationException(string message)
		: base(message)
	{
	}

	public LogServerCertificateRegistrationException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected LogServerCertificateRegistrationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
