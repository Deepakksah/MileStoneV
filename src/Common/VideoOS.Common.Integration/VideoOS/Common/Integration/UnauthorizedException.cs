using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration;

[Serializable]
public class UnauthorizedException : ServerException
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
