using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace VideoOS.Platform;

[Serializable]
[ExcludeFromCodeCoverage]
public class RestApiMIPException : MIPException
{
	public RestApiMIPException(string message)
		: base(message)
	{
	}

	protected RestApiMIPException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public RestApiMIPException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}
