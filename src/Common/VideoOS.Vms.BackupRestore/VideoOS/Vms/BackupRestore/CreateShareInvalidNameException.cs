using System;
using System.Runtime.Serialization;

namespace VideoOS.Vms.BackupRestore;

[Serializable]
public class CreateShareInvalidNameException : CreateShareException
{
	public CreateShareInvalidNameException()
	{
	}

	public CreateShareInvalidNameException(string message)
		: base(message)
	{
	}

	public CreateShareInvalidNameException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected CreateShareInvalidNameException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
