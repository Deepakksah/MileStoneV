using System;
using System.Runtime.Serialization;

namespace VideoOS.Server.RecorderCommunication;

[Serializable]
public class RecorderNotFoundException : Exception
{
	public RecorderNotFoundException()
	{
	}

	public RecorderNotFoundException(string message)
		: base(message)
	{
	}

	public RecorderNotFoundException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected RecorderNotFoundException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
