using System;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class SetNonExistingRecordingStorageAsDefaultException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60100;

	protected override int ErrorCodeDefault => 60100;

	public SetNonExistingRecordingStorageAsDefaultException()
		: base(VmoResources.VMO60100)
	{
	}

	[SqlError(60100)]
	public SetNonExistingRecordingStorageAsDefaultException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}
}
