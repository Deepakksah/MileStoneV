using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class RecordingServerNotFoundException : VmoException
{
	public RecordingServerNotFoundException()
	{
	}

	public RecordingServerNotFoundException(string message)
		: base(message)
	{
	}

	public RecordingServerNotFoundException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected RecordingServerNotFoundException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
