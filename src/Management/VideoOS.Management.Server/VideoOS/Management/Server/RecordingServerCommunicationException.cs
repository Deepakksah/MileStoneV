using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class RecordingServerCommunicationException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 61021;

	public RecordingServerCommunicationException()
		: base(VmoResources.VMO61021)
	{
	}

	public RecordingServerCommunicationException(string message)
		: base(message)
	{
	}

	public RecordingServerCommunicationException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected RecordingServerCommunicationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
