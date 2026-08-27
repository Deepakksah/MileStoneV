using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Exceptions;

[Serializable]
public class ValidateScheduleDataException : CorporateApplicationException
{
	public ValidateScheduleDataException()
	{
	}

	public ValidateScheduleDataException(string message)
		: base(message)
	{
	}

	public ValidateScheduleDataException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected ValidateScheduleDataException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
