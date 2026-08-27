using System;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class RecordingStorageEncryptionAlreadyEnabledException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60094;

	protected override int ErrorCodeDefault => 60094;

	public RecordingStorageEncryptionAlreadyEnabledException()
		: base(VmoResources.VMO60094)
	{
	}

	[SqlError(60094)]
	public RecordingStorageEncryptionAlreadyEnabledException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}
}
