using System;
using System.Globalization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class ArchiveStorageRenameException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60050;

	protected override int ErrorCodeDefault => 60050;

	public ArchiveStorageRenameException(string name, string recordingStorageName)
		: base(FormatMessage(name, recordingStorageName))
	{
		base.MessageParameters.Add(0, new MessageParameter(0, "Name", name));
		base.MessageParameters.Add(1, new MessageParameter(1, "RecordingStorageName", recordingStorageName));
	}

	[SqlError(60050)]
	public ArchiveStorageRenameException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.ExtractString("name"), databaseError.ExtractString("recordingStorageName"))
	{
	}

	private static string FormatMessage(string name, string recordingServerName)
	{
		return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60050, name, recordingServerName);
	}
}
