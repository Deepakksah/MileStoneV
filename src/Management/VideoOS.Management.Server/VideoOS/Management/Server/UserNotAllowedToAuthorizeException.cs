using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class UserNotAllowedToAuthorizeException : Exception
{
	public UserNotAllowedToAuthorizeException()
		: this(VmoResources.VMO60166)
	{
	}

	public UserNotAllowedToAuthorizeException(string message)
		: base(message)
	{
	}

	public UserNotAllowedToAuthorizeException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected UserNotAllowedToAuthorizeException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
