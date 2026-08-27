using System;
using System.Runtime.Serialization;

namespace VideoOS.Vms.BackupRestore;

[Serializable]
public class CreateShareUnknownDirectoryException : CreateShareException
{
	public CreateShareUnknownDirectoryException()
	{
	}

	public CreateShareUnknownDirectoryException(string message)
		: base(message)
	{
	}

	public CreateShareUnknownDirectoryException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected CreateShareUnknownDirectoryException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
