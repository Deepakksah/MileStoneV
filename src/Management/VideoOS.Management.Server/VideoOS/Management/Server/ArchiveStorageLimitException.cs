using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class ArchiveStorageLimitException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 64002;

	protected override int ErrorCodeDefault => 64002;

	public ArchiveStorageLimitException()
		: base(VmoResources.VMO64002)
	{
	}

	[SqlError(64002)]
	public ArchiveStorageLimitException(DatabaseException ex, DatabaseError databaseError)
		: this()
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
