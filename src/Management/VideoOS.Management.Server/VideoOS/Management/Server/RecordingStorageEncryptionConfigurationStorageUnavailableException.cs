using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class RecordingStorageEncryptionConfigurationStorageUnavailableException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 61107;

	public RecordingStorageEncryptionConfigurationStorageUnavailableException()
		: base(VmoResources.VMO61107)
	{
	}

	public RecordingStorageEncryptionConfigurationStorageUnavailableException(string message)
		: base(message)
	{
	}

	public RecordingStorageEncryptionConfigurationStorageUnavailableException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected RecordingStorageEncryptionConfigurationStorageUnavailableException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
