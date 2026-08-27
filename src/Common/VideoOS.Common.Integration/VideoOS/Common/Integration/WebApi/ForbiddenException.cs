using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.WebApi;

[Serializable]
public class ForbiddenException : ResponseException
{
	public ForbiddenException()
	{
	}

	public ForbiddenException(string message)
		: base(message)
	{
	}

	public ForbiddenException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected ForbiddenException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
