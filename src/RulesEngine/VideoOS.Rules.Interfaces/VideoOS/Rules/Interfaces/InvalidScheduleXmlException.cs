using System;
using System.Runtime.Serialization;

namespace VideoOS.Rules.Interfaces;

[Serializable]
public class InvalidScheduleXmlException : Exception
{
	public InvalidScheduleXmlException()
	{
	}

	public InvalidScheduleXmlException(string message)
		: base(message)
	{
	}

	public InvalidScheduleXmlException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected InvalidScheduleXmlException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
