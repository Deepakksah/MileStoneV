using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace VideoOS.Platform;

[Serializable]
[ExcludeFromCodeCoverage]
public sealed class InvalidOperationMIPException : MIPException
{
	public InvalidOperationMIPException(string message)
		: base(message)
	{
	}

	public InvalidOperationMIPException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	private InvalidOperationMIPException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
