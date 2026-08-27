using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Exceptions;

[Serializable]
public class DriverNotFoundApplicationException : CorporateApplicationException
{
	public DriverNotFoundApplicationException()
	{
	}

	public DriverNotFoundApplicationException(string message)
		: base(message)
	{
	}

	public DriverNotFoundApplicationException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected DriverNotFoundApplicationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
