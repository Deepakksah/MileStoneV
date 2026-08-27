using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class RemoveNonExistingRoleException : VmoException
{
	public RemoveNonExistingRoleException()
	{
	}

	public RemoveNonExistingRoleException(string message)
		: base(message)
	{
	}

	public RemoveNonExistingRoleException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected RemoveNonExistingRoleException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
