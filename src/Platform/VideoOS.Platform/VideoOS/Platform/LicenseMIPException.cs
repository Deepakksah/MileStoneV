using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace VideoOS.Platform;

[Serializable]
[ExcludeFromCodeCoverage]
public class LicenseMIPException : MIPException
{
	public LicenseMIPException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	public LicenseMIPException(string message)
		: base(message)
	{
	}

	public LicenseMIPException()
	{
	}

	protected LicenseMIPException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
