using System;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class RecordingStorageConfigureEncryptionException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60093;

	protected override int ErrorCodeDefault => 60093;

	public RecordingStorageConfigureEncryptionException()
		: base(VmoResources.VMO60093)
	{
	}

	[SqlError(60093)]
	public RecordingStorageConfigureEncryptionException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}
}
