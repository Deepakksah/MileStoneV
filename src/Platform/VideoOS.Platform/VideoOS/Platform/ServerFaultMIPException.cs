using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace VideoOS.Platform;

[Serializable]
[ExcludeFromCodeCoverage]
public class ServerFaultMIPException : MIPException
{
	public ServerFaultMIPException()
	{
	}

	public ServerFaultMIPException(string message)
		: base(message)
	{
	}

	public ServerFaultMIPException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected ServerFaultMIPException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
