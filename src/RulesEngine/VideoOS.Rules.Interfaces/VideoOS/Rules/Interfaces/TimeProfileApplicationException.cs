using System;
using System.Runtime.Serialization;

namespace VideoOS.Rules.Interfaces;

[Serializable]
public class TimeProfileApplicationException : Exception
{
	public TimeProfileApplicationException()
	{
	}

	public TimeProfileApplicationException(string message)
		: base(message)
	{
	}

	public TimeProfileApplicationException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected TimeProfileApplicationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
