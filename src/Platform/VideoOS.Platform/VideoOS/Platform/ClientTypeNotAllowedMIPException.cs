using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace VideoOS.Platform;

[Serializable]
[ExcludeFromCodeCoverage]
public class ClientTypeNotAllowedMIPException : MIPException
{
	public ClientTypeNotAllowedMIPException()
	{
	}

	public ClientTypeNotAllowedMIPException(string message)
		: base(message)
	{
	}

	public ClientTypeNotAllowedMIPException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	public ClientTypeNotAllowedMIPException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
