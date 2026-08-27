using System;
using System.Runtime.Serialization;

namespace VideoOS.Platform.License;

[Serializable]
public class UnknownCompressionTypeException : MIPException
{
	public UnknownCompressionTypeException()
	{
	}

	public UnknownCompressionTypeException(string message)
		: base(message)
	{
	}

	public UnknownCompressionTypeException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected UnknownCompressionTypeException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
