using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Drivers;

[Serializable]
public class DriverPresetException : Exception
{
	public DriverPresetException()
	{
	}

	public DriverPresetException(string message)
		: base(message)
	{
	}

	public DriverPresetException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected DriverPresetException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
