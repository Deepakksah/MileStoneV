using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace VideoOS.Platform;

[Serializable]
[ExcludeFromCodeCoverage]
public class NoVideoInTimeSpanMIPException : MIPException
{
	public NoVideoInTimeSpanMIPException(string message)
		: base(message)
	{
	}

	public NoVideoInTimeSpanMIPException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
