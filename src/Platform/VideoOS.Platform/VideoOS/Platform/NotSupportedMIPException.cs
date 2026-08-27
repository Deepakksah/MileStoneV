using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace VideoOS.Platform;

[Serializable]
[ExcludeFromCodeCoverage]
public class NotSupportedMIPException : MIPException
{
	public NotSupportedMIPException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	public NotSupportedMIPException(string message)
		: base(message)
	{
	}

	public NotSupportedMIPException()
	{
	}

	protected NotSupportedMIPException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
