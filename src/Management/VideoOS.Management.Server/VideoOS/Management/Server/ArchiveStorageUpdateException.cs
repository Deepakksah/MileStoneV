using System;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class ArchiveStorageUpdateException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60051;

	protected override int ErrorCodeDefault => 60051;

	public ArchiveStorageUpdateException()
		: base(VmoResources.VMO60051)
	{
	}

	[SqlError(60051)]
	public ArchiveStorageUpdateException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}
}
