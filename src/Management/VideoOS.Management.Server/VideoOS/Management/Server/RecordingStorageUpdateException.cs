using System;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class RecordingStorageUpdateException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60027;

	protected override int ErrorCodeDefault => 60027;

	public RecordingStorageUpdateException()
		: base(VmoResources.VMO60027)
	{
	}

	[SqlError(60027)]
	public RecordingStorageUpdateException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}
}
