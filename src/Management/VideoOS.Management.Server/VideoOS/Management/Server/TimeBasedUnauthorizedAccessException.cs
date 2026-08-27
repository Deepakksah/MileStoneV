using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class TimeBasedUnauthorizedAccessException : Exception
{
	public TimeBasedUnauthorizedAccessException()
		: this(VmoResources.VMO61008)
	{
	}

	public TimeBasedUnauthorizedAccessException(string message)
		: base(message)
	{
	}

	public TimeBasedUnauthorizedAccessException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected TimeBasedUnauthorizedAccessException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
