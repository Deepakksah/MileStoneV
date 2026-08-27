using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration;

[Serializable]
public class ServerSecurityException : ServerServiceException
{
	public ServerSecurityException()
	{
	}

	public ServerSecurityException(string message)
		: base(message)
	{
	}

	protected ServerSecurityException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
