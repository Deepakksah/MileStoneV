using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class NotSupportedVmoException : VmoException
{
	public NotSupportedVmoException()
	{
	}

	public NotSupportedVmoException(string message)
		: base(message)
	{
	}

	public NotSupportedVmoException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected NotSupportedVmoException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
