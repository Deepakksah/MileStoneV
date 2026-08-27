using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Drivers;

[Serializable]
public class DriverPtzException : Exception
{
	public DriverPtzException()
	{
	}

	public DriverPtzException(string message)
		: base(message)
	{
	}

	public DriverPtzException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected DriverPtzException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
