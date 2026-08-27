using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace VideoOS.Platform;

[Serializable]
[ExcludeFromCodeCoverage]
public class ImplementationMIPException : MIPException
{
	public ImplementationMIPException(string message)
		: base(message)
	{
	}

	public ImplementationMIPException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
