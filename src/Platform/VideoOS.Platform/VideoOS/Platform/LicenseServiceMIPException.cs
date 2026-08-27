using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace VideoOS.Platform;

[Serializable]
[ExcludeFromCodeCoverage]
public class LicenseServiceMIPException : MIPException
{
	public LicenseServiceMIPException(string message)
		: base(message)
	{
	}

	public LicenseServiceMIPException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	public LicenseServiceMIPException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
