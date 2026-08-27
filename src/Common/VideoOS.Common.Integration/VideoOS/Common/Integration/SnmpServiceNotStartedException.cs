using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration;

[Serializable]
public class SnmpServiceNotStartedException : Exception
{
	public SnmpServiceNotStartedException()
	{
	}

	public SnmpServiceNotStartedException(string message)
		: base(message)
	{
	}

	public SnmpServiceNotStartedException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected SnmpServiceNotStartedException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
