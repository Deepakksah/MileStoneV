using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.Server;

[Serializable]
public class ConnectionStringNotFoundException : InvalidOperationException
{
	public ConnectionStringNotFoundException()
		: base("Connection string was not found in registry.")
	{
	}

	public ConnectionStringNotFoundException(string message)
		: base(message)
	{
	}

	public ConnectionStringNotFoundException(string message, Exception exception)
		: base(message, exception)
	{
	}

	protected ConnectionStringNotFoundException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
