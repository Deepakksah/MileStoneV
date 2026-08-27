using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace VideoOS.Platform;

[Serializable]
[ExcludeFromCodeCoverage]
public class ArgumentMIPException : MIPException
{
	public ArgumentMIPException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	public ArgumentMIPException(string message)
		: base(message)
	{
	}

	public ArgumentMIPException()
	{
	}

	protected ArgumentMIPException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
