using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.WebApi;

[Serializable]
public class UrlReservationMissingException : Exception
{
	public UrlReservationMissingException()
	{
	}

	public UrlReservationMissingException(string message)
		: base(message)
	{
	}

	public UrlReservationMissingException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected UrlReservationMissingException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
