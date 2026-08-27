using System;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class RemoveDefaultRecordingStorageException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60029;

	protected override int ErrorCodeDefault => 60029;

	public RemoveDefaultRecordingStorageException()
		: base(VmoResources.VMO60029)
	{
	}

	[SqlError(60029)]
	public RemoveDefaultRecordingStorageException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}
}
