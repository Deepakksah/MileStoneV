using System;
using System.Runtime.Serialization;

namespace VideoOS.Vms.BackupRestore;

[Serializable]
public class DeleteShareAccessDeniedException : DeleteShareException
{
	public DeleteShareAccessDeniedException()
	{
	}

	public DeleteShareAccessDeniedException(string message)
		: base(message)
	{
	}

	public DeleteShareAccessDeniedException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected DeleteShareAccessDeniedException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
