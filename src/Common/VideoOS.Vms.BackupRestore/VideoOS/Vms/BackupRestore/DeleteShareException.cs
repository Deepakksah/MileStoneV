using System;
using System.Runtime.Serialization;

namespace VideoOS.Vms.BackupRestore;

[Serializable]
public class DeleteShareException : Exception
{
	public DeleteShareException()
	{
	}

	public DeleteShareException(string message)
		: base(message)
	{
	}

	public DeleteShareException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected DeleteShareException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
