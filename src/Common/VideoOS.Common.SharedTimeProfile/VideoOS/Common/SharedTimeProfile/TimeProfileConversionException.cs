using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.SharedTimeProfile;

[Serializable]
public class TimeProfileConversionException : Exception
{
	public TimeProfileConversionException()
	{
	}

	public TimeProfileConversionException(string message)
		: base(message)
	{
	}

	public TimeProfileConversionException(string message, Exception exception)
		: base(message, exception)
	{
	}

	protected TimeProfileConversionException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
