using System;
using System.Runtime.Serialization;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class VmoException : VmoServerException
{
	public VmoException()
	{
	}

	public VmoException(string message)
		: base(message)
	{
	}

	public VmoException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected VmoException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
