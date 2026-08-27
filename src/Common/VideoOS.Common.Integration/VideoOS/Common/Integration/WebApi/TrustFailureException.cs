using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.WebApi;

[Serializable]
public class TrustFailureException : WebApiException
{
	public TrustFailureException()
	{
	}

	public TrustFailureException(string message)
		: base(message)
	{
	}

	public TrustFailureException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected TrustFailureException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
