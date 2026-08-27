using System;
using System.Runtime.Serialization;

namespace VideoOS.Vms.BackupRestore;

[Serializable]
public class UnknownRestoreException : Exception
{
	public UnknownRestoreException()
	{
	}

	public UnknownRestoreException(string message)
		: base(message)
	{
	}

	public UnknownRestoreException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected UnknownRestoreException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
