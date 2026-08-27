using System;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class RecordingStorageEncryptionDisabledException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60098;

	protected override int ErrorCodeDefault => 60098;

	public RecordingStorageEncryptionDisabledException()
		: base(VmoResources.VMO60098)
	{
	}

	[SqlError(60098)]
	public RecordingStorageEncryptionDisabledException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}
}
