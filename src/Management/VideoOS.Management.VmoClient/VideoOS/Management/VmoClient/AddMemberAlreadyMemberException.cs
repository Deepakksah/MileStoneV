using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class AddMemberAlreadyMemberException : VmoException
{
	public AddMemberAlreadyMemberException()
	{
	}

	public AddMemberAlreadyMemberException(string message)
		: base(message)
	{
	}

	public AddMemberAlreadyMemberException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected AddMemberAlreadyMemberException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
