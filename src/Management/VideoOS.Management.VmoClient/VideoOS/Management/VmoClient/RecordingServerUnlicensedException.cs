using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class RecordingServerUnlicensedException : VmoException
{
	public RecordingServerUnlicensedException()
	{
	}

	public RecordingServerUnlicensedException(string message)
		: base(message)
	{
	}

	public RecordingServerUnlicensedException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected RecordingServerUnlicensedException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
