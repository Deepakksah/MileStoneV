using System;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class RecordingStorageEncryptionConfigurationPendingException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60095;

	protected override int ErrorCodeDefault => 60095;

	public RecordingStorageEncryptionConfigurationPendingException()
		: base(VmoResources.VMO60095)
	{
	}

	[SqlError(60095)]
	public RecordingStorageEncryptionConfigurationPendingException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}
}
