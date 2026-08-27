using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.Server.ConfigurationApi;

[Serializable]
public class InternalServerException : Exception
{
	public InternalServerException()
	{
	}

	public InternalServerException(string message)
		: base(message)
	{
	}

	public InternalServerException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected InternalServerException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
