using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace VideoOS.Platform;

[Serializable]
[ExcludeFromCodeCoverage]
public class ArgumentNullMIPException : ArgumentMIPException
{
	public ArgumentNullMIPException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	public ArgumentNullMIPException(string message)
		: base(message)
	{
	}

	public ArgumentNullMIPException()
	{
	}

	protected ArgumentNullMIPException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
