using System;
using System.Runtime.Serialization;

namespace VideoOS.Licensing;

[Serializable]
public class ProductCodeInvalidException : Exception
{
	public ProductCodeInvalidException()
	{
	}

	public ProductCodeInvalidException(string message)
		: base(message)
	{
	}

	public ProductCodeInvalidException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected ProductCodeInvalidException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
