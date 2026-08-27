using System;
using System.Runtime.Serialization;

namespace VideoOS.Server.Failover.Configurator;

[Serializable]
public class StartCertificateServerException : Exception
{
	public StartCertificateServerException()
	{
	}

	public StartCertificateServerException(string message)
		: base(message)
	{
	}

	public StartCertificateServerException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected StartCertificateServerException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
