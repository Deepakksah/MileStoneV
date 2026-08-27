using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class ArchiveStorageInformationException : VmoException
{
	public ArchiveStorageInformationException()
	{
	}

	public ArchiveStorageInformationException(string message)
		: base(message)
	{
	}

	public ArchiveStorageInformationException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected ArchiveStorageInformationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
