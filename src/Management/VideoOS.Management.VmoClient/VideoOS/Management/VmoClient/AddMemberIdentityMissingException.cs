using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class AddMemberIdentityMissingException : VmoException
{
	public AddMemberIdentityMissingException()
	{
	}

	public AddMemberIdentityMissingException(string message)
		: base(message)
	{
	}

	public AddMemberIdentityMissingException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected AddMemberIdentityMissingException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
