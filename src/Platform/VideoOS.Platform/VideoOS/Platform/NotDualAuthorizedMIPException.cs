using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace VideoOS.Platform;

[Serializable]
[ExcludeFromCodeCoverage]
public class NotDualAuthorizedMIPException : MIPException
{
	public NotDualAuthorizedMIPException()
	{
	}

	public NotDualAuthorizedMIPException(string message)
		: base(message)
	{
	}

	public NotDualAuthorizedMIPException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	public NotDualAuthorizedMIPException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
