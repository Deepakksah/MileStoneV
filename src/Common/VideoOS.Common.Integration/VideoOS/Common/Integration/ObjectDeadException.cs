using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration;

[Serializable]
public class ObjectDeadException : Exception
{
	public ObjectDeadException()
	{
	}

	public ObjectDeadException(string message)
		: base(message)
	{
	}

	public ObjectDeadException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected ObjectDeadException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
