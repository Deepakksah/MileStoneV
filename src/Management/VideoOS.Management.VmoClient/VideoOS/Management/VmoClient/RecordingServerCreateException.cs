using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class RecordingServerCreateException : VmoException
{
	public RecordingServerCreateException()
	{
	}

	public RecordingServerCreateException(string message)
		: base(message)
	{
	}

	public RecordingServerCreateException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected RecordingServerCreateException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
