using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.Server;

[Serializable]
public class AuthorizationServerException : Exception
{
	public AuthorizationServerException()
	{
	}

	public AuthorizationServerException(string message)
		: base(message)
	{
	}

	public AuthorizationServerException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected AuthorizationServerException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
