using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class ArchiveStorageLimitException : VmoException
{
	public ArchiveStorageLimitException()
	{
	}

	public ArchiveStorageLimitException(string message)
		: base(message)
	{
	}

	public ArchiveStorageLimitException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected ArchiveStorageLimitException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
