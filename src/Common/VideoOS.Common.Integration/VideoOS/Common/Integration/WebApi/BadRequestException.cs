using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.WebApi;

[Serializable]
public class BadRequestException : ResponseException
{
	public BadRequestException()
	{
	}

	public BadRequestException(string message)
		: base(message)
	{
	}

	public BadRequestException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected BadRequestException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
