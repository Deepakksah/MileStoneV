using System;
using System.Runtime.Serialization;

namespace VideoOS.Licensing;

[Serializable]
public class SLCInvalidException : Exception
{
	public SLCInvalidException()
	{
	}

	public SLCInvalidException(string message)
		: base(message)
	{
	}

	public SLCInvalidException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected SLCInvalidException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
