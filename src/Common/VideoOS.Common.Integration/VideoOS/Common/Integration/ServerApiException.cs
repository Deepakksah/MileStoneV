using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration;

[Serializable]
public class ServerApiException : ServerServiceException
{
	public ServerApiException()
	{
	}

	public ServerApiException(string message)
		: base(message)
	{
	}

	public ServerApiException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected ServerApiException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
