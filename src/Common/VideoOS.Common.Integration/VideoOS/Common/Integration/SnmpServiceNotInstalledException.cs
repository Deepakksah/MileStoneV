using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration;

[Serializable]
public class SnmpServiceNotInstalledException : Exception
{
	public SnmpServiceNotInstalledException()
	{
	}

	public SnmpServiceNotInstalledException(string message)
		: base(message)
	{
	}

	public SnmpServiceNotInstalledException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected SnmpServiceNotInstalledException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
