using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.WebApi;

[Serializable]
public class SendFailureException : WebApiException
{
	public SendFailureException()
	{
	}

	public SendFailureException(string message)
		: base(message)
	{
	}

	public SendFailureException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected SendFailureException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
