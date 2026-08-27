using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class RecordingServerInformationException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 61104;

	public RecordingServerInformationException()
		: base(VmoResources.VMO61104)
	{
	}

	public RecordingServerInformationException(string message)
		: base(message)
	{
	}

	public RecordingServerInformationException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected RecordingServerInformationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
