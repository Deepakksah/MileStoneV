using System;
using System.Runtime.Serialization;

namespace VideoOS.Server.RecorderCommunication;

[Serializable]
public class NotSupportedByLegacyRecorderException : Exception
{
	public NotSupportedByLegacyRecorderException()
	{
	}

	public NotSupportedByLegacyRecorderException(string message)
		: base(message)
	{
	}

	public NotSupportedByLegacyRecorderException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected NotSupportedByLegacyRecorderException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
