using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class RecordingServerTimeoutException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 60144;

	public RecordingServerTimeoutException()
		: base(VmoResources.VMO60144)
	{
	}

	public RecordingServerTimeoutException(string message)
		: base(message)
	{
	}

	public RecordingServerTimeoutException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected RecordingServerTimeoutException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
