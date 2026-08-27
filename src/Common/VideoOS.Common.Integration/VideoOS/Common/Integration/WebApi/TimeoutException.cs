using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.WebApi;

[Serializable]
public class TimeoutException : WebApiException
{
	public TimeoutException()
	{
	}

	public TimeoutException(string message)
		: base(message)
	{
	}

	public TimeoutException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected TimeoutException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
