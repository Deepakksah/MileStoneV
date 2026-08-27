using System;

namespace VideoOS.GisMapCommunication.Exceptions;

public class ServiceNotRegisteredException : Exception
{
	public ServiceNotRegisteredException()
	{
	}

	public ServiceNotRegisteredException(string message)
		: base(message)
	{
	}

	public ServiceNotRegisteredException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}
