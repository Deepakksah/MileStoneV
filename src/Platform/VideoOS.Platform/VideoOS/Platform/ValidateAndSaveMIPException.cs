using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace VideoOS.Platform;

[Serializable]
[ExcludeFromCodeCoverage]
public class ValidateAndSaveMIPException : ImplementationMIPException
{
	public ValidateAndSaveMIPException(string message)
		: base(message)
	{
	}

	public ValidateAndSaveMIPException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
