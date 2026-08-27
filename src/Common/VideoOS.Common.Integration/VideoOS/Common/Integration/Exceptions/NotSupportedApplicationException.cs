using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Exceptions;

[Serializable]
public class NotSupportedApplicationException : CorporateApplicationException
{
	public NotSupportedApplicationException()
	{
	}

	public NotSupportedApplicationException(string message)
		: base(message)
	{
	}

	public NotSupportedApplicationException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected NotSupportedApplicationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
