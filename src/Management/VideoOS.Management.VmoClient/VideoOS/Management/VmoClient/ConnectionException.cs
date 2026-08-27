using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class ConnectionException : VmoException
{
	public ConnectionException()
	{
	}

	public ConnectionException(string message)
		: base(message)
	{
	}

	public ConnectionException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected ConnectionException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
