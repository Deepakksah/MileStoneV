using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.Server;

[Serializable]
public sealed class RestrictedMediaFailedException : Exception
{
	public RestrictedMediaFailedException()
	{
	}

	public RestrictedMediaFailedException(string message)
		: base(message)
	{
	}

	public RestrictedMediaFailedException(string message, Exception inner)
		: base(message, inner)
	{
	}

	private RestrictedMediaFailedException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
