using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class RecordingServerCommunicationException : VmoException
{
	public RecordingServerCommunicationException()
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
