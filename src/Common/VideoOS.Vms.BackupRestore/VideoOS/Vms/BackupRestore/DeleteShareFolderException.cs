using System;
using System.Runtime.Serialization;

namespace VideoOS.Vms.BackupRestore;

[Serializable]
public class DeleteShareFolderException : Exception
{
	public DeleteShareFolderException()
	{
	}

	public DeleteShareFolderException(string message)
		: base(message)
	{
	}

	public DeleteShareFolderException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected DeleteShareFolderException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
