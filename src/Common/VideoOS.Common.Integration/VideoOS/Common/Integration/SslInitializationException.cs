using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration;

[Serializable]
public class SslInitializationException : Exception
{
	public SslInitializationException()
	{
	}

	public SslInitializationException(string message)
		: base(message)
	{
	}

	public SslInitializationException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected SslInitializationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
