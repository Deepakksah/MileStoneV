using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class RoleUpdateException : VmoException
{
	public RoleUpdateException()
	{
	}

	public RoleUpdateException(string message)
		: base(message)
	{
	}

	public RoleUpdateException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected RoleUpdateException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
