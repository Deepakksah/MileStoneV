using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class RoleRenameException : VmoException
{
	public RoleRenameException()
	{
	}

	public RoleRenameException(string message)
		: base(message)
	{
	}

	public RoleRenameException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected RoleRenameException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
