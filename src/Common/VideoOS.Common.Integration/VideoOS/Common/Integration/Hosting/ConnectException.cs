using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Hosting;

[Serializable]
public class ConnectException : Exception
{
	public ConnectException()
	{
	}

	public ConnectException(string message)
		: base(message)
	{
	}

	public ConnectException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected ConnectException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
