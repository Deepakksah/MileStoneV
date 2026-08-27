using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace VideoOS.Platform;

[Serializable]
[ExcludeFromCodeCoverage]
public class TimeoutCommunicationMIPException : CommunicationMIPException
{
	public TimeoutCommunicationMIPException(string message)
		: base(message)
	{
	}

	public TimeoutCommunicationMIPException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
