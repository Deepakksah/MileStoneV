using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.WebApi;

[Serializable]
public class InternalServerErrorException : ResponseException
{
	public InternalServerErrorException()
	{
	}

	public InternalServerErrorException(string message)
		: base(message)
	{
	}

	public InternalServerErrorException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected InternalServerErrorException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
