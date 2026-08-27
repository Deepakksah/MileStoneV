using System;
using System.Runtime.Serialization;

namespace VideoOS.Vms.BackupRestore;

[Serializable]
public class ShareNotAvailableException : Exception
{
	public ShareNotAvailableException()
	{
	}

	public ShareNotAvailableException(string message)
		: base(message)
	{
	}

	public ShareNotAvailableException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected ShareNotAvailableException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
