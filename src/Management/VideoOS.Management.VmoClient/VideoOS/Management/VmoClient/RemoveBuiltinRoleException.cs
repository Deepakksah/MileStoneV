using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class RemoveBuiltinRoleException : VmoException
{
	public RemoveBuiltinRoleException()
	{
	}

	public RemoveBuiltinRoleException(string message)
		: base(message)
	{
	}

	public RemoveBuiltinRoleException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected RemoveBuiltinRoleException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
