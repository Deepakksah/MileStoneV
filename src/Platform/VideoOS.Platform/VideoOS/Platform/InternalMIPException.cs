using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace VideoOS.Platform;

[Serializable]
[ExcludeFromCodeCoverage]
public class InternalMIPException : MIPException
{
	public InternalMIPException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	public InternalMIPException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
