using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class RecordingStorageMovePathInvalidException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 60140;

	public RecordingStorageMovePathInvalidException()
		: base(VmoResources.VMO60140)
	{
	}

	public RecordingStorageMovePathInvalidException(string message)
		: base(message)
	{
	}

	public RecordingStorageMovePathInvalidException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected RecordingStorageMovePathInvalidException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
