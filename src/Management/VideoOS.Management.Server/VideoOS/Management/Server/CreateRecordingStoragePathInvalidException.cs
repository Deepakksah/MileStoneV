using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class CreateRecordingStoragePathInvalidException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 60152;

	public CreateRecordingStoragePathInvalidException()
		: base(VmoResources.VMO60152)
	{
	}

	public CreateRecordingStoragePathInvalidException(string message)
		: base(message)
	{
	}

	public CreateRecordingStoragePathInvalidException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected CreateRecordingStoragePathInvalidException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
