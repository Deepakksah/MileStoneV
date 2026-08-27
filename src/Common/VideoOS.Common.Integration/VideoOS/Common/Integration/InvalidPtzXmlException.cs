using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration;

[Serializable]
public class InvalidPtzXmlException : Exception
{
	public InvalidPtzXmlException()
	{
	}

	public InvalidPtzXmlException(string message)
		: base(message)
	{
	}

	public InvalidPtzXmlException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected InvalidPtzXmlException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
