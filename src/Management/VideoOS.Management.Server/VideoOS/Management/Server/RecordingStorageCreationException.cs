using System;
using System.Globalization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class RecordingStorageCreationException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60025;

	protected override int ErrorCodeDefault => 60025;

	public RecordingStorageCreationException(string storageName, string recordingServerName)
		: base(FormatMessage(storageName, recordingServerName))
	{
		base.MessageParameters.Add(0, new MessageParameter(0, "StorageName", storageName));
		base.MessageParameters.Add(1, new MessageParameter(1, "RecordingServerName", recordingServerName));
	}

	[SqlError(60025)]
	public RecordingStorageCreationException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.ExtractString("name"), databaseError.ExtractString("recordingServerName"))
	{
	}

	private static string FormatMessage(string storageName, string recordingServerName)
	{
		return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60025, storageName, recordingServerName);
	}
}
