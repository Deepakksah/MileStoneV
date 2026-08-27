using System;
using System.Runtime.Serialization;

namespace VideoOS.Licensing;

[Serializable]
public class FormatInvalidException : Exception
{
	public FormatInvalidException()
	{
	}

	public FormatInvalidException(string message)
		: base(message)
	{
	}

	public FormatInvalidException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected FormatInvalidException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
