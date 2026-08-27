using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class UserNotAuthorizedForLogOnException : Exception
{
	public UserNotAuthorizedForLogOnException()
		: this(VmoResources.VMO60164)
	{
	}

	public UserNotAuthorizedForLogOnException(string message)
		: base(message)
	{
	}

	public UserNotAuthorizedForLogOnException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected UserNotAuthorizedForLogOnException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
