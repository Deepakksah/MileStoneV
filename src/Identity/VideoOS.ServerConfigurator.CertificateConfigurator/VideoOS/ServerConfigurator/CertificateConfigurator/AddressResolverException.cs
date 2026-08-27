using System;
using System.Runtime.Serialization;

namespace VideoOS.ServerConfigurator.CertificateConfigurator;

[Serializable]
public class AddressResolverException : Exception
{
	public AddressResolverException()
	{
	}

	public AddressResolverException(string message)
		: base(message)
	{
	}

	public AddressResolverException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected AddressResolverException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
