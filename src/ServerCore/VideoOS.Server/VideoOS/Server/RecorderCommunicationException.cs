using System;
using System.Runtime.Serialization;

namespace VideoOS.Server;

[Serializable]
public class RecorderCommunicationException : Exception
{
	public RecorderCommunicationException()
	{
	}

	public RecorderCommunicationException(string message)
		: base(message)
	{
	}

	public RecorderCommunicationException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected RecorderCommunicationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
