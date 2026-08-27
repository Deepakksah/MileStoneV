using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class RecordingServerNotSupportedException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 60174;

	public RecordingServerNotSupportedException()
		: base(VmoResources.VMO60174)
	{
	}

	public RecordingServerNotSupportedException(string message)
		: base(message)
	{
	}

	public RecordingServerNotSupportedException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected RecordingServerNotSupportedException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
