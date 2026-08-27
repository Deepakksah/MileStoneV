using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Exceptions;

[Serializable]
public class MediaApplicationException : CorporateApplicationException
{
	public MediaApplicationException()
	{
	}

	public MediaApplicationException(string message)
		: base(message)
	{
	}

	public MediaApplicationException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected MediaApplicationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
