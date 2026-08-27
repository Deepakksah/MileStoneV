using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class ArchiveStorageMovePathInvalidException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 60141;

	public ArchiveStorageMovePathInvalidException()
		: base(VmoResources.VMO60141)
	{
	}

	public ArchiveStorageMovePathInvalidException(string message)
		: base(message)
	{
	}

	public ArchiveStorageMovePathInvalidException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected ArchiveStorageMovePathInvalidException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
