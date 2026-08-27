using System;

namespace VideoOS.IdentityServer.DomainEventInfrastructure;

public class TransientDomainException : Exception
{
	public TransientDomainException(string message)
		: base(message)
	{
	}

	public TransientDomainException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}
