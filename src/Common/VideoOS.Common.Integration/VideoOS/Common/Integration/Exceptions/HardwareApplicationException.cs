using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Exceptions;

[Serializable]
public class HardwareApplicationException : CorporateApplicationException
{
	public HardwareApplicationException()
	{
	}

	public HardwareApplicationException(string message)
		: base(message)
	{
	}

	public HardwareApplicationException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected HardwareApplicationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
