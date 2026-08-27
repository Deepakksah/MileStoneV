using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace VideoOS.Platform;

[Serializable]
[ExcludeFromCodeCoverage]
public class MIPException : Exception
{
	public MIPException()
	{
	}

	public MIPException(string message)
		: base(message)
	{
	}

	public MIPException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	public MIPException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
