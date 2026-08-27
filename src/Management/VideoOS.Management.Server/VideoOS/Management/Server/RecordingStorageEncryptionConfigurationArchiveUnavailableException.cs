using System;
using System.Globalization;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class RecordingStorageEncryptionConfigurationArchiveUnavailableException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 61108;

	public RecordingStorageEncryptionConfigurationArchiveUnavailableException()
	{
	}

	public RecordingStorageEncryptionConfigurationArchiveUnavailableException(string archiveName)
		: base(FormatMessage(archiveName))
	{
		base.MessageParameters.Add(0, new MessageParameter(0, "ArchiveName", archiveName));
	}

	public RecordingStorageEncryptionConfigurationArchiveUnavailableException(string archiveName, Exception inner)
		: base(archiveName, inner)
	{
	}

	protected RecordingStorageEncryptionConfigurationArchiveUnavailableException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	private static string FormatMessage(string archiveName)
	{
		return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO61108, archiveName);
	}
}
