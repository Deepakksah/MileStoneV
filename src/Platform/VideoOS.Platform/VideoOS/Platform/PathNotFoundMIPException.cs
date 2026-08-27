using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace VideoOS.Platform;

[Serializable]
[ExcludeFromCodeCoverage]
public class PathNotFoundMIPException : MIPException
{
	public PathNotFoundMIPException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	public PathNotFoundMIPException(string message)
		: base(message)
	{
	}

	public PathNotFoundMIPException()
	{
	}

	protected PathNotFoundMIPException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
