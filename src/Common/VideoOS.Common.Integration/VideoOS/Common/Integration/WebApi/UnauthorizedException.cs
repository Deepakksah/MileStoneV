using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.WebApi;

[Serializable]
public class UnauthorizedException : ResponseException
{
	public UnauthorizedException()
	{
	}

	public UnauthorizedException(string message)
		: base(message)
	{
	}

	public UnauthorizedException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected UnauthorizedException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
