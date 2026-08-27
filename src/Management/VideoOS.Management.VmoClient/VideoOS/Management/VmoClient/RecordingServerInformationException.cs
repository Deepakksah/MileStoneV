using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class RecordingServerInformationException : VmoException
{
	public RecordingServerInformationException()
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
