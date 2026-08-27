using System;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class RecordingStorageEncryptionConfigurationIncorrectPasswordException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60097;

	protected override int ErrorCodeDefault => 60097;

	public RecordingStorageEncryptionConfigurationIncorrectPasswordException()
		: base(VmoResources.VMO60097)
	{
	}

	[SqlError(60097)]
	public RecordingStorageEncryptionConfigurationIncorrectPasswordException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}
}
