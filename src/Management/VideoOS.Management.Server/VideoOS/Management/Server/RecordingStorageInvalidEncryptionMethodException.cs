using System;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class RecordingStorageInvalidEncryptionMethodException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60092;

	protected override int ErrorCodeDefault => 60092;

	public RecordingStorageInvalidEncryptionMethodException()
		: base(VmoResources.VMO60092)
	{
	}

	[SqlError(60092)]
	public RecordingStorageInvalidEncryptionMethodException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}
}
