using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class RemoveRoleMemberNotMemberException : VmoException
{
	public RemoveRoleMemberNotMemberException()
	{
	}

	public RemoveRoleMemberNotMemberException(string message)
		: base(message)
	{
	}

	public RemoveRoleMemberNotMemberException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected RemoveRoleMemberNotMemberException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
