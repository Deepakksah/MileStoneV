using System;
using System.Runtime.Serialization;

namespace VideoOS.Server.Admin;

[Serializable]
public class ServerAdminException : Exception
{
	public ServerAdminException()
	{
	}

	public ServerAdminException(string message)
		: base(message)
	{
	}

	public ServerAdminException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected ServerAdminException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
