using System;

namespace VideoOS.DataCollector.Service.Common.Exceptions;

public class ClientRegistrationIdInvalidException : InvalidOperationException
{
	public ClientRegistrationIdInvalidException()
		: base("Client registration id is not in a valid format")
	{
	}

	public ClientRegistrationIdInvalidException(string message)
		: base(message)
	{
	}

	public ClientRegistrationIdInvalidException(string message, Exception exception)
		: base(message, exception)
	{
	}
}
