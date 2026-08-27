using System;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class RemoveNonExistingArchiveStorageException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60052;

	protected override int ErrorCodeDefault => 60052;

	public RemoveNonExistingArchiveStorageException()
		: base(VmoResources.VMO60052)
	{
	}

	[SqlError(60052)]
	public RemoveNonExistingArchiveStorageException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}
}
