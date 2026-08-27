using System;
using System.Runtime.Serialization;

namespace VideoOS.Vms.BackupRestore;

[Serializable]
public class CreateShareAccessDeniedException : CreateShareException
{
	public CreateShareAccessDeniedException()
	{
	}

	public CreateShareAccessDeniedException(string message)
		: base(message)
	{
	}

	public CreateShareAccessDeniedException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected CreateShareAccessDeniedException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
