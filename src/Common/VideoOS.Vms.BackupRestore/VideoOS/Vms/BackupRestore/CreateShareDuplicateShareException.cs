using System;
using System.Runtime.Serialization;

namespace VideoOS.Vms.BackupRestore;

[Serializable]
public class CreateShareDuplicateShareException : CreateShareException
{
	public CreateShareDuplicateShareException()
	{
	}

	public CreateShareDuplicateShareException(string message)
		: base(message)
	{
	}

	public CreateShareDuplicateShareException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected CreateShareDuplicateShareException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
