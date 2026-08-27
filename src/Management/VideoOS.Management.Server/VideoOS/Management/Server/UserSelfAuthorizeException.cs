using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class UserSelfAuthorizeException : Exception
{
	public UserSelfAuthorizeException()
		: this(VmoResources.VMO60165)
	{
	}

	public UserSelfAuthorizeException(string message)
		: base(message)
	{
	}

	public UserSelfAuthorizeException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected UserSelfAuthorizeException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
