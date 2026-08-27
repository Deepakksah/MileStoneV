using System;

namespace VideoOS.Common.Crypto;

public class UnknownCompressionTypeException : Exception
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
}
