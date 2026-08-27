using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace VideoOS.Platform;

[Serializable]
[ExcludeFromCodeCoverage]
public class NotAuthorizedMIPException : MIPException
{
	public NotAuthorizedMIPException(string message)
		: base(message)
	{
	}

	public NotAuthorizedMIPException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	public NotAuthorizedMIPException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
