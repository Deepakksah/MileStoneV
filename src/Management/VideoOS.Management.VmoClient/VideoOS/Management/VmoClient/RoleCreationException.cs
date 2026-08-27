using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class RoleCreationException : VmoException
{
	public RoleCreationException()
	{
	}

	public RoleCreationException(string message)
		: base(message)
	{
	}

	public RoleCreationException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected RoleCreationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
