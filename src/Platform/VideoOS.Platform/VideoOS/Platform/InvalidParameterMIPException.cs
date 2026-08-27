using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace VideoOS.Platform;

[Serializable]
[ExcludeFromCodeCoverage]
public class InvalidParameterMIPException : ImplementationMIPException
{
	public InvalidParameterMIPException(string message)
		: base(message)
	{
	}

	public InvalidParameterMIPException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
