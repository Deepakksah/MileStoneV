using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class RecordingServerOperationFailedException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 60372;

	public RecordingServerOperationFailedException()
		: base(VmoResources.VMO60372)
	{
	}

	public RecordingServerOperationFailedException(string message)
		: base(message)
	{
	}

	public RecordingServerOperationFailedException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected RecordingServerOperationFailedException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
