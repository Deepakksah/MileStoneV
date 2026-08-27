using System;
using System.Globalization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class ArchiveStorageCreationException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60049;

	protected override int ErrorCodeDefault => 60049;

	public ArchiveStorageCreationException(string name, string recordingStorageName)
		: base(FormatMessage(name, recordingStorageName))
	{
		base.MessageParameters.Add(0, new MessageParameter(0, "ArchiveName", name));
		base.MessageParameters.Add(1, new MessageParameter(1, "RecordingStorageName", recordingStorageName));
	}

	[SqlError(60049)]
	public ArchiveStorageCreationException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.ExtractString("name"), databaseError.ExtractString("recordingStorageName"))
	{
	}

	private static string FormatMessage(string name, string recordingStorageName)
	{
		return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60049, name, recordingStorageName);
	}
}
