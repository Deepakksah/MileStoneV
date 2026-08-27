using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class VmoTimeoutException : VmoException
{
	public VmoTimeoutException()
	{
	}

	public VmoTimeoutException(string message)
		: base(message)
	{
	}

	public VmoTimeoutException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected VmoTimeoutException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
