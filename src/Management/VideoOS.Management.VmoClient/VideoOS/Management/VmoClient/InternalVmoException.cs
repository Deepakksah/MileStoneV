using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class InternalVmoException : VmoException
{
	public InternalVmoException()
	{
	}

	public InternalVmoException(string message)
		: base(message)
	{
	}

	public InternalVmoException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected InternalVmoException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
