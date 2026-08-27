using System;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class RemoveNonExistingRecordingStorageException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60028;

	protected override int ErrorCodeDefault => 60028;

	public RemoveNonExistingRecordingStorageException()
		: base(VmoResources.VMO60028)
	{
	}

	[SqlError(60028)]
	public RemoveNonExistingRecordingStorageException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}
}
