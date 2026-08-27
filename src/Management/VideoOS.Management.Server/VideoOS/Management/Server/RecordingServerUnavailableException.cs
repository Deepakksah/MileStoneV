using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class RecordingServerUnavailableException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 60143;

	public RecordingServerUnavailableException()
		: base(VmoResources.VMO60143)
	{
	}

	public RecordingServerUnavailableException(string message)
		: base(message)
	{
	}

	public RecordingServerUnavailableException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected RecordingServerUnavailableException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
